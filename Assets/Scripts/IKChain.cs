using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor;
using UnityEngine.Networking.Types;

struct Joint
{
}

public class IKChain : MonoBehaviour
{
    public Transform goal;

    public Bone[] bones;
    public Quaternion[] initialAngles = new Quaternion[0];

//    private Vector3 tipVector = new Vector3();
//    private Vector3 goalVector = new Vector3();
    private float mStartAngle = 0;

    private float mEndAngle = 0;

    // Use this for initialization
    void Start()
    {
    }

    public void RecordInitialPose()
    {
        if (initialAngles.Length != bones.Length)
            initialAngles = new Quaternion[bones.Length];

        for (int i = 0; i < bones.Length; i++)
        {
            initialAngles[i] = bones[i].transform.rotation;
        }
    }

    public void ApplyInitialPose()
    {
        if (initialAngles.Length != bones.Length)
            RecordInitialPose();

        bones[0].transform.position = transform.position; // root bone to ik root

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].transform.rotation = initialAngles[i];
            if (i > 0)
            {
                bones[i].transform.position = bones[i - 1].transform.position + bones[i - 1].GetVector();
            }
        }
    }

    public void Match()
    {
        bones[0].transform.position = transform.position;
        bones[1].transform.position = bones[0].transform.position + bones[0].GetVector();
    }

    private float angleBetween(Vector3 from, Vector3 to)
    {
        return Mathf.Acos(Vector3.Dot(from.normalized, to.normalized));
    }

    // Use defaultangle when actual angle cannot be calculated
    private float CalcAngle(float a, float c, float b, float defaultAngle = 0)
    {
        if (b > a + c || b < Mathf.Abs(a - c))
            return defaultAngle;

        return Mathf.Acos((a * a + b * b - c * c) / (2.0f * a * b));
    }

    void UpdateOneBone()
    {
        bones[0].transform.position = transform.position;
        var goalVector = goal.position - bones[0].transform.position;
        // Just point the bone to the goal
        var delta = Quaternion.FromToRotation(bones[0].GetVector(), goalVector);
        bones[0].transform.rotation = delta * bones[0].transform.rotation;
    }

    void UpdateTwoBones()
    {
//        ApplyInitialPose();

        var goalVector = goal.position - bones[0].transform.position;

        // Rotate the chain to let the tip lie on the root-to-goal
        {
            var tipVector = (bones[1].transform.position + bones[1].GetVector()) - bones[0].transform.position;
            var delta = Quaternion.FromToRotation(tipVector, goalVector);
            bones[0].transform.rotation = delta * bones[0].transform.rotation;
            bones[1].transform.rotation = delta * bones[1].transform.rotation;
            bones[1].transform.position = bones[0].transform.position + bones[0].GetVector();
        }

        // Find delta angle to form a triangle
        var ikBoneAngle = CalcAngle(bones[0].length, bones[1].length, goalVector.magnitude);

        // Angle between goal and root bone
        var fkBoneAngle = angleBetween(goalVector, bones[0].GetVector());
        var angle = fkBoneAngle - ikBoneAngle;
        var axis = Vector3.Cross(bones[0].GetVector(), goalVector).normalized;

        // Adjust root bone to form side of triangle
        bones[0].transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis) * bones[0].transform.rotation;

        // Re-attach child bone first
        bones[1].transform.position = bones[0].transform.position + bones[0].GetVector();

        // Rotate last bone towards goal
        var secondBoneVector = goal.position - bones[1].transform.position;
        var delta1 = Quaternion.FromToRotation(bones[1].GetVector(), secondBoneVector);
        bones[1].transform.rotation = delta1 * bones[1].transform.rotation;
    }


    private void UpdateThreeBones()
    {
        ApplyInitialPose();

        var goalVector = goal.position - bones[0].transform.position;

        {
            //Consider that the initial pose of the chain defines an ideal pose for the limb, and any
            //modification of the limb pose must be minimized. By analyzing the FK pose of the
            //chain, for example, in Figure 2.3.6, we can determine a value that describes how bent
            //Bone 0 is with respect to the rest of the chain.
            //We can use the law of cosines to calculate a value that defines the angle of Bone 0
            //if the remaining chain were to be laid out in a straight line. The remaining chain
            //length, the distance to the FK chain tip, and the bone length are used to calculate the
            //max bone angle. This angle is referred to as the max FK bone angle. Comparing this
            //max FK bone angle to the actual FK bone angle gives us a value that defines our FK
            //bone angle relative to the distance to the FK chain tip.
            //
            //bone angle fraction = FK bone angle / maximum FK bone angle
            //
            //The bone angle fraction is defined relative to the initial shape of the chain and is
            //a correlation between the bone’s orientation and the rest of the chain pose. Conversely,
            //the remaining bone length can also be used to calculate the maximum possible
            //angle that the bone can assume in IK. This maximum IK bone angle is multiplied by
            //the bone angle fraction value to determine the new IK bone angle.


            var tipVector = (bones[2].transform.position + bones[2].GetVector()) - bones[0].transform.position;
            var tipLength = tipVector.magnitude;
            var boneLen = bones[0].length;
            var otherBonesLen = bones[1].length + bones[2].length;
            var goalLength = goalVector.magnitude;

            var maxFKAngle = CalcAngle(boneLen, otherBonesLen, tipLength);
            var fkBoneAngle = angleBetween(tipVector, bones[0].GetVector());
            var fkAngle = maxFKAngle - fkBoneAngle;
            var maxIKAngle = CalcAngle(boneLen, otherBonesLen, goalLength);

            var fraction = fkAngle / maxFKAngle;
            var newIkAngle = fraction * maxIKAngle;

            var axis = Vector3.Cross(bones[0].GetVector(), goalVector).normalized;
            bones[0].transform.rotation = Quaternion.AngleAxis(newIkAngle * Mathf.Rad2Deg, axis) *
                                          bones[0].transform.rotation;
        }

//        {
        //   ROTATE CHAIN
//            var tipVector = (bones[2].transform.position + bones[2].GetVector()) - bones[0].transform.position;
//            var delta = Quaternion.FromToRotation(tipVector, goalVector);
//            bones[0].transform.rotation = delta * bones[0].transform.rotation;
//            bones[1].transform.rotation = delta * bones[1].transform.rotation;
//            bones[2].transform.rotation = delta * bones[2].transform.rotation;
//            bones[1].transform.position = bones[0].transform.position + bones[0].GetVector();
//            bones[2].transform.position = bones[1].transform.position + bones[1].GetVector();
//        }
//        return;

        {
            // TEMP: Stretched remaining chain
            var goalVector2 = goal.position - bones[1].transform.position;
            bones[1].transform.position = bones[0].transform.position + bones[0].GetVector();
            var delta = Quaternion.FromToRotation(bones[1].GetVector(), goalVector2);
            bones[1].transform.rotation = delta * bones[1].transform.rotation;
            bones[2].transform.position = bones[1].transform.position + bones[1].GetVector();
            bones[2].transform.rotation = bones[1].transform.rotation;
        }

//        bones[1].transform.position = bones[0].transform.position + bones[0].GetVector();
//
//        {
//            var ikBoneAngle = CalcAngle(bones[1].length, bones[2].length, goalVector.magnitude);
//
//            // Angle between goal and root bone
//            var fkBoneAngle = angleBetween(goalVector, bones[1].GetVector());
//            var angle = fkBoneAngle - ikBoneAngle;
//            var axis = Vector3.Cross(bones[1].GetVector(), goalVector).normalized;
//
//            // Adjust root bone to form side of triangle
//            bones[1].transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis) *
//                                          bones[1].transform.rotation;
//
//            // Re-attach child bone first
//            bones[2].transform.position = bones[1].transform.position + bones[1].GetVector();
//
//            // Rotate last bone towards goal
//            var secondBoneVector = goal.position - bones[2].transform.position;
//            var delta1 = Quaternion.FromToRotation(bones[2].GetVector(), secondBoneVector);
//            bones[2].transform.rotation = delta1 * bones[2].transform.rotation;
//        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bones.Length == 1)
        {
            UpdateOneBone();
        }
        else if (bones.Length == 2)
        {
            UpdateTwoBones();
        }
        else if (bones.Length == 3)
        {
            UpdateThreeBones();
        }
    }


    void OnDrawGizmos()
    {
        Update();
        Gizmos.DrawWireSphere(goal.position, 0.1f);
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, goal.position);
        Gizmos.color = Color.gray;
//        Gizmos.DrawLine(transform.position, transform.position + TipVector);

        Gizmos.color = Color.green;
    }

    void DrawAngle(float from, float to, float radius)
    {
        int samples = 32;
        var p2 = new Vector3();
        for (int i = 0; i <= samples; i++)
        {
            float a = from + ((to - from) / samples) * i;
            var p1 = new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius);

            if (i > 0)
            {
                Gizmos.DrawLine(p1, p2);
            }
            p2 = p1;
        }
    }
}

[CustomEditor(typeof(IKChain))]
class IKChainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IKChain chain = target as IKChain;
        if (GUILayout.Button("Record Initial Pose"))
            chain.RecordInitialPose();
        if (GUILayout.Button("Appaly Initial Pose"))
            chain.ApplyInitialPose();
    }
}
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class StickyController : MonoBehaviour
{
    public AnimationCurve sensitivityCurve = new AnimationCurve();
    public float moveSpeed = 0.3f;
    private float turnSpeed = 1000;

    private Vector3 dMoveVector = new Vector3();
    private Vector3 dSensorPos = new Vector3();
    private Vector3 dRayDirection = new Vector3();
    private Vector3 dHitPoint = new Vector3();

    private Vector3[] leggOffsets =
    {
        new Vector3(1, 0, .99f),
        new Vector3(1, 0, .33f),
        new Vector3(1, 0, -.33f),
        new Vector3(1, 0, -.99f),
    };

    private Vector3[] legPositions = new Vector3[8];

    void Update()
    {
        var moveVector = GetMoveVector();
        dMoveVector = moveVector;
        var lastPosition = transform.position;
        transform.position = transform.position + moveVector * moveSpeed;

        float mag = moveVector.magnitude;
        if (mag > 0.001f)
        {
            var targetRot = Quaternion.LookRotation(moveVector, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * turnSpeed * mag);
        }


        float sensorHeight = 1;
        var sensorPos = transform.position + transform.up * sensorHeight;
        dSensorPos = sensorPos;
        var rayDirection = -transform.up;
        dRayDirection = rayDirection;

        RaycastHit hit;
        var hasHit = Physics.Raycast(sensorPos, rayDirection, out hit, 100);

        if (hasHit && hit.distance > 0.1f)
        {
            dHitPoint = hit.point;
            transform.position = hit.point;
            var up = hit.normal;
            var forward = Vector3.Cross(transform.right, up);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
        else
        {
            transform.position = lastPosition;
            print("Where's the floor!?");
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + dMoveVector * 2);
        Gizmos.DrawLine(dSensorPos, dSensorPos + dRayDirection);
        Gizmos.DrawWireSphere(dHitPoint, 0.2f);


    }


    Vector3 GetMoveVector()
    {
        var dirVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        var dirLength = dirVector.magnitude;
        if (dirLength > 0.001f)
        {
            dirLength = dirVector.magnitude;
            dirVector = dirVector / dirLength;
            dirLength = Mathf.Min(1, dirLength);
            dirLength = Mathf.Pow(dirLength, 3); // More sensitive
            dirVector = dirVector * dirLength;
        }
        else
        {
            dirLength = 0;
        }
        dirVector = Camera.main.transform.rotation * dirVector;
        var camToCharacterSpace = Quaternion.FromToRotation(-Camera.main.transform.forward, transform.up);
        dirVector = (camToCharacterSpace * dirVector);

        return dirVector * sensitivityCurve.Evaluate(dirLength) * 1;
    }
}
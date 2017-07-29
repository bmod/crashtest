using UnityEngine;
using System.Collections;

public class TPCamera : MonoBehaviour
{
    public Transform subject = null;

    public float targetStrength = 3;
    public float distance = 10;
    public float azimuth = 30;
    public float orbit = 0;

    public float offsetPitch = 30;

    private Quaternion currentOrientation = new Quaternion();
    private Quaternion lastOrientation = new Quaternion();
    private Vector3 lastPosition = new Vector3();

    void OnDrawGizmos()
    {
        //Update();
    }

    void LateUpdate()
    {
        if (subject != null)
            UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (lastOrientation == subject.rotation && lastPosition == subject.position)
            return;

        var targetQuat = subject.rotation * Quaternion.Euler(azimuth, orbit, 0);

        currentOrientation = Quaternion.Slerp(currentOrientation, targetQuat, Time.deltaTime * targetStrength);
        var offset = Quaternion.Euler(offsetPitch, 0, 0);

        var pos = currentOrientation * new Vector3(0, 0, -distance);
        transform.position = subject.position + pos;

        transform.rotation = currentOrientation * offset;

        lastOrientation = subject.rotation;
        lastPosition = subject.position;
    }

//
//    private void UpdateCameraDirect()
//    {
//        var quat = Quaternion.Euler(azimuth, orbit, 0);
//        var pos = quat * new Vector3(0,0,-distance);
//        transform.position = subject.position + pos;
//        transform.rotation = quat;
//    }
}
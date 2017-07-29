using UnityEngine;
using System.Collections;

public class Bone : MonoBehaviour
{

    public float length = 1;

    public Vector3 GetVector()
    {
        return transform.right * -length;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, Vector3.left * length);
    }
}

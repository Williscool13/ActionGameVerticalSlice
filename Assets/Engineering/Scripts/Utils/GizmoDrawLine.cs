using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawLine : MonoBehaviour
{
    [SerializeField] Color color = Color.red;
    [SerializeField] Vector3 eulerRotationOffset;
    [SerializeField] Vector3 laserOffset;
    void OnDrawGizmos() {
        Gizmos.color = color;
        // rotate transform.forward by eulerRotationOffset
        // draw a line from transform.position to the rotated transform.forward


        Gizmos.DrawLine(transform.position + laserOffset, transform.position + laserOffset + (Quaternion.Euler(eulerRotationOffset) * transform.forward) * 20.0f);
    }
}

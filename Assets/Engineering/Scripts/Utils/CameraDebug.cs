using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void OnDrawGizmos() {
        Ray rayout = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Gizmos.color = Color.black;
        // rotate transform.forward by eulerRotationOffset
        // draw a line from transform.position to the rotated transform.forward
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + rayout.direction * 100.0f);
    }
}

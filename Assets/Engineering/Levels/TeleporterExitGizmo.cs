using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterExitGizmo : MonoBehaviour
{
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1);
    }
}

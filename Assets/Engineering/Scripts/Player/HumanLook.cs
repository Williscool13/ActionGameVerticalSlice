using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanLook : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] float xMouseSensitivity = 1f;
    [SerializeField] float yMouseSensitivity = 4f;
    [SerializeField] Transform playerModelYRotPivot;
    [SerializeField] Vector2 playerModelYRotClamps =  new Vector2(-50f, 50f);
    [SerializeField] Transform cameraYRotPivot;
    [SerializeField] Vector2 cameraYRotClamps =  new Vector2(-70f, 70f);

    [SerializeField] private Transform head;
    [SerializeField] private Transform chest;
    [SerializeField] private Transform root;
    [SerializeField] private float gizmoRange = 5.0f;

    float yRot = 0f;
    void Update()
    {
        Vector2 mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, mouseDelta.x * Time.deltaTime * xMouseSensitivity, 0));

        yRot = Mathf.Clamp(Angle(yRot - mouseDelta.y * Time.deltaTime * yMouseSensitivity), cameraYRotClamps.x, cameraYRotClamps.y);


        cameraYRotPivot.localEulerAngles = new Vector3(yRot, 0, 0);

        //playerModelYRotPivot.localEulerAngles = new Vector3(Mathf.Clamp(Angle(yRot), playerModelYRotClamps.x, playerModelYRotClamps.y), 0, 0);
        playerModelYRotPivot.localEulerAngles = new Vector3(MapAngle(yRot, cameraYRotClamps, playerModelYRotClamps), 0, 0);
    }

    float MapAngle(float angle, Vector2 from, Vector2 to) {
        // x + -camera.x / sum(camyrotclamps) * sum(playermodelyrotclamps) - playermodelyrotclamps.x
        //float xTar = ((angle + cameraYRotClamps.x) / (Mathf.Abs(cameraYRotClamps.x) + Mathf.Abs(cameraYRotClamps.y)) * (Mathf.Abs(playerModelYRotClamps.x) + Mathf.Abs(playerModelYRotClamps.y))) + playerModelYRotClamps.x;
        float xTar = (angle - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;
        // 70 + -70 / 140 * 100 - 50
        return xTar;
    }

    float Angle(float value) {
        if (value > 180f) {
            return value - 360f;
        }
        return value;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(head.position, head.position + head.forward * gizmoRange);
        Gizmos.DrawLine(chest.position, chest.position + chest.forward * gizmoRange);
        Gizmos.DrawLine(root.position + new Vector3(0, 1.6f, 0), root.position + new Vector3(0, 1.6f, 0) + root.forward * gizmoRange);
    }
}

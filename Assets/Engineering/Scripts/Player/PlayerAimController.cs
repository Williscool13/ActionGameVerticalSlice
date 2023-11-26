using Cinemachine;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float xMouseSensitivity = 1f;
    [SerializeField] private float yMouseSensitivity = 4f;
    [SerializeField] private float xRecoilRecoverySpeed = 20f;
    [SerializeField] private float aimZoomSpeed = 2f;
    [SerializeField] private Transform playerModelYRotPivot;
    [SerializeField] private Vector2 playerModelYRotClamps =  new Vector2(-50f, 50f);
    [SerializeField] private Transform cameraYRotPivot;
    [SerializeField] private Vector2 cameraYRotClamps =  new Vector2(-70f, 70f);

    [SerializeField] private Transform head;
    [SerializeField] private Transform chest;
    [SerializeField] private Transform root;
    [SerializeField] private float gizmoRange = 5.0f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private FloatReference aimRecoilMultiplier;
    [SerializeField] private FloatReference aimSensitivityMultiplier;

    
    public bool AimingDownSights { get; set; }


    const float baseZDistance = -0.28f;
    const float aimZDistance = 0f;
    float yRot = 0f;
    float xRot = 0f;

    Cinemachine3rdPersonFollow follow;

    List<RecoilData> recoils = new();

    private void Start() {
        follow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    void Update() {
        AimZoom();
        Recoil();
    }

    void AimZoom() {
        follow.ShoulderOffset.z = Mathf.MoveTowards(follow.ShoulderOffset.z, AimingDownSights ? aimZDistance : baseZDistance, Time.deltaTime * aimZoomSpeed);
    }

    void Recoil() {
        float totalYRecoil = 0;
        float totalXRecoil = 0;
        for (int i = 0; i < recoils.Count; i++) {
            if (recoils[i].duration <= 0) {
                recoils.RemoveAt(i);
            }
        }

        if (recoils.Count > 0) {

            for (int i = 0; i < recoils.Count; i++) {
                totalYRecoil += recoils[i].RecoilKick.y / recoils[i].totalDuration;
                totalXRecoil += recoils[i].RecoilKick.x / recoils[i].totalDuration;

                recoils[i].duration -= Time.deltaTime;
            }

            totalYRecoil /= recoils.Count;
            totalXRecoil /= recoils.Count;

            if (AimingDownSights) {
                totalYRecoil *= aimRecoilMultiplier.Value;
                totalXRecoil *= aimRecoilMultiplier.Value;
            }
        }
        Vector2 mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, mouseDelta.x * Time.deltaTime * xMouseSensitivity * (AimingDownSights ? aimSensitivityMultiplier.Value : 1f), 0));

        yRot = Mathf.Clamp(Angle(yRot - (mouseDelta.y * Time.deltaTime * yMouseSensitivity * (AimingDownSights ? aimSensitivityMultiplier.Value : 1f)) - totalYRecoil * Time.deltaTime), cameraYRotClamps.x, cameraYRotClamps.y);

        xRot = Mathf.Clamp(Angle(xRot + totalXRecoil * Time.deltaTime), -20f, 20f);
        xRot = Mathf.Lerp(xRot, 0, Time.deltaTime * xRecoilRecoverySpeed);

        cameraYRotPivot.localEulerAngles = new Vector3(yRot, xRot, 0);

        playerModelYRotPivot.localEulerAngles = new Vector3(MapAngle(yRot, cameraYRotClamps, playerModelYRotClamps), 0, 0);
    }

    float MapAngle(float angle, Vector2 from, Vector2 to) => (angle - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;
    float Angle(float value) => value > 180f ? value - 360f : value;
    public void AddRecoil(RecoilData data) => recoils.Add(data);


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(head.position, head.position + head.forward * gizmoRange);
        Gizmos.DrawLine(chest.position, chest.position + chest.forward * gizmoRange);
        Gizmos.DrawLine(root.position + new Vector3(0, 1.6f, 0), root.position + new Vector3(0, 1.6f, 0) + root.forward * gizmoRange);
    }
}

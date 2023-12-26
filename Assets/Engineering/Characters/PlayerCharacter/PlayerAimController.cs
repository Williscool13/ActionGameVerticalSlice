using Cinemachine;
using DG.Tweening;
using ECM.Components;
using KinematicCharacterController;
using PlayerFiniteStateMachine;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Windows;

public class PlayerAimController : MonoBehaviour
{

    const float baseCameraDistance = 0.8f;
    const float aimCameraDistance = 0.5f;

    const float baseCrouchHeight = 0.1f;
    const float crouchingCrouchHeight = -0.3f;


    const float baseFOV = 75f;
    const float sprintFOV = 85f;
    const float aimFOV = 65f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [TitleGroup("Properties")][SerializeField] private float xMouseSensitivity = 1f;
    [TitleGroup("Properties")][SerializeField] private float yMouseSensitivity = 4f;

    [FoldoutGroup("Model Torso Rotation Pivot")][SerializeField] private Transform playerModelYRotPivot;
    [FoldoutGroup("Model Torso Rotation Pivot")][SerializeField] private Vector2 playerModelYRotClamps = new Vector2(-50f, 50f);
    [FoldoutGroup("Camera Rotation Pivot")][SerializeField] private Transform cameraYRotPivot;
    [FoldoutGroup("Camera Rotation Pivot")][SerializeField] private Vector2 cameraYRotClamps = new Vector2(-70f, 70f);


    [TitleGroup("Move Towards Values")][SerializeField] private float aimZoomTransitionSpeed = 2f;
    [TitleGroup("Move Towards Values")][SerializeField] private float crouchTransitionSpeed = 5f;
    [TitleGroup("Move Towards Values")][SerializeField] private float sprintTransitionSpeed = 5f;

    [TitleGroup("Scriptable Aim Properties")][SerializeField] private FloatReference aimRecoilMultiplier;
    [TitleGroup("Scriptable Aim Properties")][SerializeField] private FloatReference aimSensitivityMultiplier;
    [TitleGroup("Scriptable Aim Properties")][SerializeField] private FloatReference crouchRecoilMultiplier;
    [TitleGroup("Scriptable Aim Properties")][SerializeField] private FloatReference crouchSensitivityMultiplier;

    private ICharacterMovementController movementController;

    public bool AimingDownSights { get; set; }
    public bool Sprinting { get; set; }

    bool Crouching { get { return movementController != null && movementController.Crouching; } }

    float _targetVerticalAngle = 0f;
    Cinemachine3rdPersonFollow follow;

    List<RecoilData> recoils = new();

    CharacterMovement movement;
    PlayerInput pinput;
    private void Start() {
        movementController = GetComponent<ICharacterMovementController>();
        follow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        movement = GetComponent<CharacterMovement>();
        pinput = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None)[0];
    }

    void Update() {
        if (movement == null) {
            AimZoom();
            return;
        }
        // make this input system actually good
        Vector2 aim = pinput.actions["Look"].ReadValue<Vector2>();


        Vector2 recoil = Recoil(Time.deltaTime);

        // player look x
        movement.rotation *= GetReorientedHorizontalRotation(aim.x);
        // look x recoil
        movement.rotation *= Quaternion.Euler(transform.up * recoil.x * Time.deltaTime);
        // player look y (+ recoil)
        Quaternion planarRotation = GetPlanarRotation(transform.rotation, Vector3.up);
        RotateVertical(planarRotation, aim.y, recoil.y, Time.deltaTime);


        Debug.Assert(!(Crouching && Sprinting), "Sprinting and Crouching at the same time is not legal");
        CrouchPosition();

        float targetFOV = Sprinting ? sprintFOV : (AimingDownSights ? aimFOV : baseFOV);
        virtualCamera.m_Lens.FieldOfView = Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, targetFOV, Time.deltaTime * sprintTransitionSpeed);
        /*AimZoom();
        SprintZoom();*/

    }

    Quaternion GetPlanarRotation(Quaternion rotation, Vector3 axis) {
        // Get the forward vector in world space
        Vector3 forward = rotation * Vector3.forward;

        // Project the forward vector onto the plane defined by the specified axis
        Vector3 planarForward = Vector3.ProjectOnPlane(forward, axis);

        // Calculate the rotation between the original forward vector and the planar forward vector
        Quaternion planarRotation = Quaternion.FromToRotation(Vector3.forward, planarForward);

        // Convert the rotation to Euler angles
        return planarRotation;
    }
    Quaternion GetReorientedHorizontalRotation(float rotateInput) {
        return Quaternion.Euler(transform.up * rotateInput * xMouseSensitivity * Time.deltaTime * (AimingDownSights ? aimSensitivityMultiplier.Value : 1f));
    }

    void AimZoom() {
        //follow.CameraDistance = Mathf.MoveTowards(follow.CameraDistance, AimingDownSights ? aimCameraDistance : baseCameraDistance, Time.deltaTime * aimZoomTransitionSpeed);
        virtualCamera.m_Lens.FieldOfView = Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, AimingDownSights ? aimFOV : baseFOV, Time.deltaTime * aimZoomTransitionSpeed);
    }
    void CrouchPosition() {
        follow.ShoulderOffset.y = Mathf.MoveTowards(follow.ShoulderOffset.y, Crouching ? crouchingCrouchHeight : baseCrouchHeight, Time.deltaTime * crouchTransitionSpeed);
    }
    void SprintZoom() {
        virtualCamera.m_Lens.FieldOfView = Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, Sprinting ? sprintFOV : baseFOV, Time.deltaTime * sprintTransitionSpeed);
    }

    /// <summary>
    /// Parse stored recoil data and return the total recoil for this frame
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    Vector2 Recoil(float deltaTime) {
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

                recoils[i].duration -= deltaTime;
            }

            totalYRecoil /= recoils.Count;
            totalXRecoil /= recoils.Count;

            if (AimingDownSights) {
                totalYRecoil *= aimRecoilMultiplier.Value;
                totalXRecoil *= aimRecoilMultiplier.Value;
            }
            if (Crouching) {
                totalYRecoil *= crouchRecoilMultiplier.Value;
                totalXRecoil *= crouchRecoilMultiplier.Value;
            }
        }

        return new Vector2(totalXRecoil, totalYRecoil);
    }

    public Vector2 ParseRecoil(float deltaTime) {
        return Recoil(deltaTime);
    }

    public void RotateVertical(Quaternion planarRot, float rawYInput, float yRecoil, float deltaTime) {
        _targetVerticalAngle -= (rawYInput * yMouseSensitivity * deltaTime) + yRecoil * deltaTime;

        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, cameraYRotClamps.x, cameraYRotClamps.y);

        Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, cameraYRotPivot.eulerAngles.y, cameraYRotPivot.eulerAngles.z);
        cameraYRotPivot.rotation = planarRot * verticalRot;
        cameraYRotPivot.localEulerAngles = new Vector3(cameraYRotPivot.localEulerAngles.x, 0, 0);

        float _mappedTargetVerticalAngle = MapAngle(_targetVerticalAngle, cameraYRotClamps, playerModelYRotClamps);
        Quaternion playerModelVerticalRot = Quaternion.Euler(_mappedTargetVerticalAngle, playerModelYRotPivot.eulerAngles.y, playerModelYRotPivot.eulerAngles.z);
        playerModelYRotPivot.rotation = planarRot * playerModelVerticalRot;
        playerModelYRotPivot.localEulerAngles = new Vector3(playerModelYRotPivot.localEulerAngles.x, 0, 0);
    }

    public float GetXRotateDelta(float value) {
        return value * Time.deltaTime * xMouseSensitivity * (AimingDownSights ? aimSensitivityMultiplier.Value : 1f);
    }

    float MapAngle(float angle, Vector2 from, Vector2 to) => (angle - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;

    public void AddRecoil(RecoilData data) => recoils.Add(data);
}

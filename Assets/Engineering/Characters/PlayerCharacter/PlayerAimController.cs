using Cinemachine;
using DG.Tweening;
using ECM.Components;
using PlayerFiniteStateMachine;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Windows;

public class PlayerAimController : MonoBehaviour
{

    const float baseCameraDistance = 0.8f;
    const float aimCameraDistance = 0.5f;

    [SerializeField][ReadOnly] float baseCrouchHeight;
    [SerializeField][ReadOnly] float crouchingHeightDiff = -0.4f;


    const float baseFOV = 75f;
    const float sprintFOVDiff = 10f;
    const float aimFOVDiff = -10f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [TitleGroup("Aim Properties")][SerializeField] private float xMouseSensitivity = 1f;
    [TitleGroup("Aim Properties")][SerializeField] private float yMouseSensitivity = 4f;

    [TitleGroup("Interact Properties")][SerializeField] private float interactDistance = 5f;
    [TitleGroup("Interact Properties")][SerializeField] private LayerMask interactLayerMask;

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
    private void Start() {
        movementController = GetComponent<ICharacterMovementController>();
        follow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        movement = GetComponent<CharacterMovement>();

        baseCrouchHeight = follow.ShoulderOffset.y;
    }

    void Update() {
        RotateRecoil();


        Debug.Assert(!(Crouching && Sprinting), "Sprinting and Crouching at the same time is not legal");
        CrouchPosition();

        float targetFOV = baseFOV;
        if (Sprinting) { targetFOV += sprintFOVDiff; }
        if (AimingDownSights) { targetFOV += aimFOVDiff; }
        virtualCamera.m_Lens.FieldOfView = Mathf.MoveTowards(virtualCamera.m_Lens.FieldOfView, targetFOV, Time.deltaTime * sprintTransitionSpeed);
        /*AimZoom();
        SprintZoom();*/

    }

    public void RotateCharacter(Vector2 rotateInput) {
        //Vector2 recoil = Recoil(Time.deltaTime);
        RotateHorizontal(rotateInput.x, 0, Time.deltaTime);
        RotateVertical(rotateInput.y, 0, Time.deltaTime);
    }

    void RotateRecoil() {
        Vector2 recoil = Recoil(Time.deltaTime);

        RotateHorizontal(0, recoil.x, Time.deltaTime);
        RotateVertical(0, recoil.y, Time.deltaTime);
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

    void CrouchPosition() {
        follow.ShoulderOffset.y = Mathf.MoveTowards(follow.ShoulderOffset.y, Crouching ? baseCrouchHeight + crouchingHeightDiff : baseCrouchHeight, Time.deltaTime * crouchTransitionSpeed);
    }

    public IInteractable GetInteractable() {
        // draw sphere cast from camera y rot pivot and check if it hits any IIteractable
        if (Physics.SphereCast(cameraYRotPivot.position, 0.2f, cameraYRotPivot.forward, out RaycastHit hit, interactDistance, interactLayerMask)) {
            if (hit.collider.TryGetComponent(out IInteractable interactable)) {
                return interactable;
            }
        }

        return null;
    }


    private void OnDrawGizmos() {
         Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cameraYRotPivot.position, 0.2f);
        Gizmos.DrawLine(cameraYRotPivot.position, cameraYRotPivot.position + cameraYRotPivot.forward * interactDistance);
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

    public void RotateHorizontal(float rawXInput, float xRecoil, float deltaTime) {
        // player look x
        movement.rotation *= GetReorientedHorizontalRotation(rawXInput);
        // look x recoil
        movement.rotation *= Quaternion.Euler(transform.up * xRecoil * Time.deltaTime);
    }
    public void RotateVertical(float rawYInput, float yRecoil, float deltaTime) {
        Quaternion planarRot = GetPlanarRotation(transform.rotation, Vector3.up);

        _targetVerticalAngle -= (rawYInput * yMouseSensitivity * deltaTime) + yRecoil * deltaTime;

        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, cameraYRotClamps.x, cameraYRotClamps.y);

        Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, cameraYRotPivot.eulerAngles.y, cameraYRotPivot.eulerAngles.z);
        Quaternion targetRot = planarRot * verticalRot;
        float tar = targetRot.eulerAngles.x > 180f ? targetRot.eulerAngles.x - 360f : targetRot.eulerAngles.x;
        float cur = transform.localEulerAngles.x > 180f ? transform.localEulerAngles.x - 360f : transform.localEulerAngles.x;
        float diff = Mathf.Abs(tar-cur);
        if (diff > 60f) { Debug.Log("Rapid change in rotation, bug..? Value is: " + diff); return; }
        cameraYRotPivot.rotation = planarRot * verticalRot;
        cameraYRotPivot.localEulerAngles = new Vector3(cameraYRotPivot.localEulerAngles.x, 0, 0);

        float _mappedTargetVerticalAngle = MapAngle(_targetVerticalAngle, cameraYRotClamps, playerModelYRotClamps);
        Quaternion playerModelVerticalRot = Quaternion.Euler(_mappedTargetVerticalAngle, playerModelYRotPivot.eulerAngles.y, playerModelYRotPivot.eulerAngles.z);
        playerModelYRotPivot.rotation = planarRot * playerModelVerticalRot;
        playerModelYRotPivot.localEulerAngles = new Vector3(playerModelYRotPivot.localEulerAngles.x, 0, 0);
    }

    float MapAngle(float angle, Vector2 from, Vector2 to) => (angle - from.x) / (from.y - from.x) * (to.y - to.x) + to.x;

    public void AddRecoil(RecoilData data) => recoils.Add(data);
}

public interface IAimController
{
    Vector2 Recoil(float deltaTime);
    public void RotateCharacter(Vector2 rotateInput);
    public void AddRecoil(RecoilData data);

    public IInteractable GetInteract();
}

public interface IInteractable
{
    public InteractableType Type { get; set; }

    public void Interact();
    public void Highlight();
}
public enum InteractableType
{
    Pressable,
    Item
}


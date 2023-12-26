using PlayerFiniteStateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class GunRigController : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] float moveTowardsMultiplier = 1f;

    [TitleGroup("Constraints")][SerializeField] TwoBoneIKConstraint rightHandIK;
    [TitleGroup("Constraints")][SerializeField] TwoBoneIKConstraint leftHandIK;
    [TitleGroup("Constraints")][SerializeField] TwoBoneIKConstraint leftHandHipIK;
    [TitleGroup("Constraints")][SerializeField] MultiParentConstraint handParentConstraint;
    [TitleGroup("Constraints")][SerializeField] ParentConstraint lefthandParentConstraint;
    [TitleGroup("Constraints")][SerializeField] ParentConstraint righthandParentConstraint;

    [Title("State Machines")]
    [SerializeField] PlayerMovementStateMachine movementMachine;
    [SerializeField] PlayerActionStateMachine actionMachine;

    [TitleGroup("Left Wrist Target")][SerializeField][ReadOnly] float currentLeftGunTarget = 0f;
    [TitleGroup("Left Wrist Target")][SerializeField][ReadOnly] float currentLeftHipTarget = 0f;
    [TitleGroup("Right Wrist Target")][SerializeField][ReadOnly] float currentRightGunTarget = 0f;
    [TitleGroup("Pivot Target")][SerializeField][ReadOnly] float currentShoulderIdleAnchor = 0f;
    [TitleGroup("Pivot Target")][SerializeField][ReadOnly] float currentShoulderAimAnchor = 0f;
    [TitleGroup("Pivot Target")][SerializeField][ReadOnly] float currentShoulderObstructedAnchor = 0f;
    [TitleGroup("Pivot Target")][SerializeField][ReadOnly] float currentRightHandAnchor = 0f;

    [Title("Obstruction Checks")]
    [SerializeField] LayerMask obstructionLayerMask;

    WeightedTransformArray handParentWeightedTransforms;
    float rigValueDifference = 0;

    private void Start() {
        handParentWeightedTransforms = handParentConstraint.data.sourceObjects;

        movementMachine.OnPlayerMovementStateChange += OnMovementStateChange;
        actionMachine.OnPlayerActionStateChange += OnActionStateChange;

        if (actionMachine.CurrentState.GunIkTargets.Priority > movementMachine.CurrentState.GunIKTargets.Priority) {
            SetCurrentIkTargets(actionMachine.CurrentState.GunIkTargets);
        } else {
            SetCurrentIkTargets(movementMachine.CurrentState.GunIKTargets);
        }

    }


    void Update() {
        if (RigInPosition()) return;
        MoveTowardsCurrentIkTargets();
    }

    public bool RigInPosition() {
        return Mathf.Approximately(rigValueDifference, 0);
    }

    public float CalculateRigValueDifference() {
        float val = Mathf.Abs(leftHandIK.weight - currentLeftGunTarget)
            + Mathf.Abs(leftHandHipIK.weight - currentLeftHipTarget)
            + Mathf.Abs(rightHandIK.weight - currentRightGunTarget)
            + Mathf.Abs(handParentWeightedTransforms[0].weight - currentRightHandAnchor)
            + Mathf.Abs(handParentWeightedTransforms[1].weight - currentShoulderObstructedAnchor)
            + Mathf.Abs(handParentWeightedTransforms[2].weight - currentShoulderIdleAnchor)
            + Mathf.Abs(handParentWeightedTransforms[3].weight - currentShoulderAimAnchor);

        return val / 7f;
    }

    public bool GunIdlePositionObstructed() {
        Collider[] ColliderBuffer = new Collider[1];
        Transform gunIdleTransform = handParentWeightedTransforms[2].transform;
        int count = Physics.OverlapBoxNonAlloc(gunIdleTransform.position + gunIdleTransform.forward * 0.275f, new Vector3(0.1f, 0.1f, 0.55f), ColliderBuffer, gunIdleTransform.rotation, obstructionLayerMask);

        return count > 0;
    }

    public bool GunAimPositionObscured() {
        Collider[] ColliderBuffer = new Collider[1];
        Transform gunIdleTransform = handParentWeightedTransforms[3].transform;
        int count = Physics.OverlapBoxNonAlloc(gunIdleTransform.position + gunIdleTransform.forward * 0.275f, new Vector3(0.1f, 0.1f, 0.55f), ColliderBuffer, gunIdleTransform.rotation, obstructionLayerMask);

        return count > 0;
    }

    void SetCurrentIkTargets(GunIKTargets targets) {
        currentLeftGunTarget = 0;
        currentLeftHipTarget = 0;
        currentRightGunTarget = 0;
        currentShoulderIdleAnchor = 0;
        currentShoulderAimAnchor = 0;
        currentShoulderObstructedAnchor = 0;
        currentRightHandAnchor = 0;


        // left hand
        switch (targets.LeftWristGunIk) {
            case GunIKTargets.LeftWristIK.None:

                break;
            case GunIKTargets.LeftWristIK.Gun:
                currentLeftGunTarget = 1.0f;
                break;
            case GunIKTargets.LeftWristIK.Waist:
                currentLeftHipTarget = 1.0f;
                break;
        }

        // right hand
        switch (targets.RightWristGunIk) {
            case GunIKTargets.RightWristIK.None:
                break;
            case GunIKTargets.RightWristIK.Gun:
                currentRightGunTarget = 1f;
                break;
        }

        // gun anchor point
        switch (targets.AnchorPoint) {
            case GunIKTargets.GunAnchorPoint.None:
                break;
            case GunIKTargets.GunAnchorPoint.RightArm:
                currentRightHandAnchor = 1f;
                break;
            case GunIKTargets.GunAnchorPoint.ShoulderIdle:
                currentShoulderIdleAnchor = 1f;
                break;
            case GunIKTargets.GunAnchorPoint.ShoulderAim:
                currentShoulderAimAnchor = 1f;
                break;
            case GunIKTargets.GunAnchorPoint.ShoulderObstructed:
                currentShoulderObstructedAnchor = 1f;
                break;
        }

        rigValueDifference = CalculateRigValueDifference();
    }

    void MoveTowardsCurrentIkTargets() {
        float moveTowardsRate = (1 - 0) * Time.deltaTime * moveTowardsMultiplier;


        leftHandIK.weight = Mathf.MoveTowards(leftHandIK.weight, currentLeftGunTarget, moveTowardsRate);
        leftHandHipIK.weight = Mathf.MoveTowards(leftHandHipIK.weight, currentLeftHipTarget, moveTowardsRate);

        rightHandIK.weight = Mathf.MoveTowards(rightHandIK.weight, currentRightGunTarget, moveTowardsRate);

        handParentWeightedTransforms.SetWeight(0, Mathf.MoveTowards(handParentWeightedTransforms[0].weight, currentRightHandAnchor, moveTowardsRate));
        handParentWeightedTransforms.SetWeight(1, Mathf.MoveTowards(handParentWeightedTransforms[1].weight, currentShoulderObstructedAnchor, moveTowardsRate));
        handParentWeightedTransforms.SetWeight(2, Mathf.MoveTowards(handParentWeightedTransforms[2].weight, currentShoulderIdleAnchor, moveTowardsRate));
        handParentWeightedTransforms.SetWeight(3, Mathf.MoveTowards(handParentWeightedTransforms[3].weight, currentShoulderAimAnchor, moveTowardsRate));

        handParentConstraint.data.sourceObjects = handParentWeightedTransforms;

        rigValueDifference = CalculateRigValueDifference();
    }

    /// <summary>
    /// Changes the Ik targets, usually called when swapping weapons
    /// </summary>
    /// <param name="front"></param>
    /// <param name="handle"></param>
    public void ChangeHandIKTargets(Transform front, Transform handle) {
        lefthandParentConstraint.SetSource(0, new ConstraintSource() { sourceTransform = front, weight = 1f });
        righthandParentConstraint.SetSource(0, new ConstraintSource() { sourceTransform = handle, weight = 1f });
    }

    void OnMovementStateChange(object machine, PlayerMovementStateChangeEventArgs args) {
        if (args.NewState.GunIKTargets.Priority > actionMachine.CurrentState.GunIkTargets.Priority) {
            // set values to newstate targets
            SetCurrentIkTargets(args.NewState.GunIKTargets);
        }
        else {
            // set values to actionmachine targets
            SetCurrentIkTargets(actionMachine.CurrentState.GunIkTargets);
        }
        Debug.Log("Movement state change");
    }
    void OnActionStateChange(object machine, PlayerActionStateChangeEventArgs args) {
        if (args.NewState.GunIkTargets.Priority > movementMachine.CurrentState.GunIKTargets.Priority) {
            // set to new state targets
            SetCurrentIkTargets(args.NewState.GunIkTargets);
        }
        else {
            // set to movement targets
            SetCurrentIkTargets(actionMachine.CurrentState.GunIkTargets);
        }
    }
}


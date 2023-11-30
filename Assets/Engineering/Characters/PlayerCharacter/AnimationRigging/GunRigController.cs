using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class GunRigController : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint rightHandIK;
    [SerializeField] TwoBoneIKConstraint leftHandIK;
    [SerializeField] TwoBoneIKConstraint leftHandHipIK;
    [SerializeField] MultiParentConstraint handParentConstraint;
    [SerializeField] MultiParentConstraint weaponParentConstraint;
    [SerializeField] TransformCopy leftHandTarget;
    [SerializeField] TransformCopy rightHandTarget;

    [SerializeField] PlayerMovementStateMachine movementMachine;
    [SerializeField] PlayerActionStateMachine actionMachine;


    WeightedTransformArray handParentWeightedTransforms;
    WeightedTransformArray weaponParentWeightedTransform;

    [SerializeField] float moveTowardsMultiplier = 1.0f;


    float currentLeftGunTarget = 0f;
    float currentLeftHipTarget = 0f;
    float currentRightGunTarget = 0f;

    float currentShoulderIdleAnchor = 0f;
    float currentShoulderAimAnchor = 0f;
    float currentRightHandAnchor = 0f;

    float rigValueDifference = 0;

    private void Start() {
        handParentWeightedTransforms = handParentConstraint.data.sourceObjects;
        weaponParentWeightedTransform = weaponParentConstraint.data.sourceObjects;

        movementMachine.OnPlayerMovementStateChange += OnMovementStateChange;
        actionMachine.OnPlayerActionStateChange += OnActionStateChange;

        if (actionMachine.CurrentState.GunIkTargets.Priority > movementMachine.CurrentState.GunIKTargets.Priority) {
            SetCurrentIkTargets(actionMachine.CurrentState.GunIkTargets);
        } else {
            SetCurrentIkTargets(movementMachine.CurrentState.GunIKTargets);
        }

    }


    void Update() {
        MoveTowardsCurrentIkTargets();
    }


    public bool RigInPosition() {
        return Mathf.Approximately(rigValueDifference, 0);
    }


    void SetCurrentIkTargets(GunIKTargets targets) {
        currentLeftGunTarget = 0;
        currentLeftHipTarget = 0;
        currentRightGunTarget = 0;
        currentShoulderIdleAnchor = 0;
        currentShoulderAimAnchor = 0;
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
            case GunIKTargets.GunAnchorPoint.RightArm:
                currentRightHandAnchor = 1f;
                break;
            case GunIKTargets.GunAnchorPoint.ShoulderIdle:
                currentShoulderIdleAnchor = 1f;
                break;
            case GunIKTargets.GunAnchorPoint.ShoulderAim:
                currentShoulderAimAnchor = 1f;
                break;
        }
    }

    void MoveTowardsCurrentIkTargets() {
        float moveTowardsRate = (1 - 0) * Time.deltaTime * moveTowardsMultiplier;


        leftHandIK.weight = Mathf.MoveTowards(leftHandIK.weight, currentLeftGunTarget, moveTowardsRate);
        leftHandHipIK.weight = Mathf.MoveTowards(leftHandHipIK.weight, currentLeftHipTarget, moveTowardsRate);
        rightHandIK.weight = Mathf.MoveTowards(rightHandIK.weight, currentRightGunTarget, moveTowardsRate);

        //float diff = SetWeaponParentConstraint(moveTowardsRate);
        float diff = SetWeaponParentConstraintTest(moveTowardsRate);

        rigValueDifference =
            Mathf.Abs(leftHandIK.weight - currentLeftGunTarget)
            + Mathf.Abs(leftHandHipIK.weight - currentLeftHipTarget)
            + Mathf.Abs(rightHandIK.weight - currentRightGunTarget)
            + diff;

        rigValueDifference /= 5f;
    }

    float SetWeaponParentConstraint(float moveTowardsRate) {
        handParentWeightedTransforms.SetWeight(0, Mathf.MoveTowards(handParentWeightedTransforms[0].weight, currentRightHandAnchor, moveTowardsRate));
        handParentWeightedTransforms.SetWeight(1, Mathf.MoveTowards(handParentWeightedTransforms[1].weight, currentShoulderIdleAnchor, moveTowardsRate));
        handParentWeightedTransforms.SetWeight(2, Mathf.MoveTowards(handParentWeightedTransforms[2].weight, currentShoulderAimAnchor, moveTowardsRate));

        if (currentRightHandAnchor == 1f) {
            handParentConstraint.data.constrainedRotationXAxis = true;
            handParentConstraint.data.constrainedRotationYAxis = true;
            handParentConstraint.data.constrainedRotationZAxis = true;
        } else {
            handParentConstraint.data.constrainedRotationXAxis = false;
            handParentConstraint.data.constrainedRotationYAxis = false;
            handParentConstraint.data.constrainedRotationZAxis = false;
        }

        handParentConstraint.data.sourceObjects = handParentWeightedTransforms;

        return Mathf.Abs(handParentWeightedTransforms[0].weight - currentRightHandAnchor)
            + Mathf.Abs(handParentWeightedTransforms[1].weight - currentShoulderIdleAnchor)
            + Mathf.Abs(handParentWeightedTransforms[2].weight - currentShoulderAimAnchor);
    }
    float SetWeaponParentConstraintTest(float moveTowardsRate) {
        handParentWeightedTransforms.SetWeight(0, Mathf.MoveTowards(handParentWeightedTransforms[0].weight, currentRightHandAnchor, moveTowardsRate));
        weaponParentWeightedTransform.SetWeight(0, Mathf.MoveTowards(weaponParentWeightedTransform[0].weight, currentShoulderIdleAnchor, moveTowardsRate));
        weaponParentWeightedTransform.SetWeight(1, Mathf.MoveTowards(weaponParentWeightedTransform[1].weight, currentShoulderAimAnchor, moveTowardsRate));

        if (currentRightHandAnchor == 1f) { handParentWeightedTransforms.SetWeight(0, 1f); }

        handParentConstraint.data.sourceObjects = handParentWeightedTransforms;
        weaponParentConstraint.data.sourceObjects = weaponParentWeightedTransform;

        return Mathf.Abs(handParentWeightedTransforms[0].weight - currentRightHandAnchor)
            + Mathf.Abs(weaponParentWeightedTransform[0].weight - currentShoulderIdleAnchor)
            + Mathf.Abs(weaponParentWeightedTransform[1].weight - currentShoulderAimAnchor);
    }

    /// <summary>
    /// Changes the Ik targets, usually called when swapping weapons
    /// </summary>
    /// <param name="front"></param>
    /// <param name="handle"></param>
    public void ChangeHandIKTargets(Transform front, Transform handle) {
        leftHandTarget.SetTarget(front);
        rightHandTarget.SetTarget(handle);
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


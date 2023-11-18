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
    [SerializeField] MultiParentConstraint constraint;

    [SerializeField] TransformCopy leftHandTarget;
    [SerializeField] TransformCopy rightHandTarget;

    [SerializeField] PlayerMovementStateMachine movementMachine;
    [SerializeField] PlayerActionStateMachine actionMachine;

    WeightedTransformArray weightedTransforms;

    [SerializeField] float lerpSpeed = 10f;
    private void Start() {
        // 0 is shoulder
        // 1 is right hand
        weightedTransforms = constraint.data.sourceObjects;
    }



    void Update() {
        // have this be event based rather than polled
        if (actionMachine.CurrentState.GunIkTarget.Priority > movementMachine.CurrentState.GunIKTargets.Priority){
            SetIK(actionMachine.CurrentState.GunIkTarget);
        } else {
            SetIK(movementMachine.CurrentState.GunIKTargets);
        }




        constraint.data.sourceObjects = weightedTransforms;

    }


    void SetIK(GunIKTargets targets) {
        float leftGunTarget = 0f;
        float leftHipTarget = 0f;
        float rightGunTarget = 0f;

        float shoulderAnchor = 0f;
        float rightHandAnchor = 0f;

        // left hand
        switch (targets.LeftWristGunIk) {
            case GunIKTargets.LeftWristIK.None:

                break;
            case GunIKTargets.LeftWristIK.Gun:
                leftGunTarget = 1.0f;
                break;
            case GunIKTargets.LeftWristIK.Waist:
                leftHipTarget = 1.0f;
                break;
        }

        leftHandIK.weight = Mathf.Lerp(leftHandIK.weight, leftGunTarget, Time.deltaTime * lerpSpeed);
        leftHandHipIK.weight = Mathf.Lerp(leftHandHipIK.weight, leftHipTarget, Time.deltaTime * lerpSpeed);

        // right hand
        switch (targets.RightWristGunIk) {
            case GunIKTargets.RightWristIK.None:
                break;
            case GunIKTargets.RightWristIK.Gun:
                rightGunTarget = 1f;
                break;
        }
        rightHandIK.weight = Mathf.Lerp(rightHandIK.weight, rightGunTarget, Time.deltaTime * lerpSpeed);

        switch (targets.AnchorPoint) {
            case GunIKTargets.GunAnchorPoint.RightArm:
                rightHandAnchor = 1f;
                break;
            case GunIKTargets.GunAnchorPoint.Shoulder:
                shoulderAnchor = 1f;
                break;
        }


        weightedTransforms.SetWeight(0, Mathf.Lerp(weightedTransforms[0].weight, shoulderAnchor, Time.deltaTime * lerpSpeed));
        weightedTransforms.SetWeight(1, Mathf.Lerp(weightedTransforms[1].weight, rightHandAnchor, Time.deltaTime * lerpSpeed));
    }


    public void ChangeHandIKTargets(Transform front, Transform handle) {
        leftHandTarget.SetTarget(front);
        rightHandTarget.SetTarget(handle);
    }
}


using FiniteStateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/State")]
    public class PlayerActionState : BaseState<PlayerActionStateMachine>
    {
        [SerializeField] private GunIKTargets gunIkTarget;
        public GunIKTargets GunIkTarget => gunIkTarget;
        public List<PlayerActionStateAction> EnterActions = new List<PlayerActionStateAction>();
        public List<PlayerActionStateAction> UpdateActions = new List<PlayerActionStateAction>();
        public List<PlayerActionStateAction> ExitActions = new List<PlayerActionStateAction>();

        public List<PlayerActionStateTransition> Transitions = new List<PlayerActionStateTransition>();

        public override void Execute(PlayerActionStateMachine machine) {
            Debug.Log("Exeucting State: " + this.name);
            foreach (PlayerActionStateAction action in UpdateActions)
                action.Execute(machine);

            foreach (PlayerActionStateTransition transition in Transitions)
                transition.Execute(machine);
        }

        public override void Enter(PlayerActionStateMachine machine) {
            Debug.Log("Entring State: " + this.name);
            foreach (PlayerActionStateAction action in EnterActions)
                action.Execute(machine);
        }

        public override void Exit(PlayerActionStateMachine machine) {
            Debug.Log("Exiting State: " + this.name);
            foreach (PlayerActionStateAction action in ExitActions)
                action.Execute(machine);
        }
    }
}

[Serializable]
public struct GunIKTargets
{
    [SerializeField] private LeftWristIK leftWristGunIk;
    [SerializeField] private RightWristIK rightWristGunIk;
    [SerializeField] private GunAnchorPoint anchorPoint;
    [SerializeField] private float priority;

    public LeftWristIK LeftWristGunIk => leftWristGunIk;
    public RightWristIK RightWristGunIk => rightWristGunIk;
    public GunAnchorPoint AnchorPoint => anchorPoint;
    public float Priority => priority;


    public GunIKTargets(LeftWristIK leftWristGunIk, RightWristIK rightWristGunIk, GunAnchorPoint anchorPoint, float priority) {
        this.leftWristGunIk = leftWristGunIk;
        this.rightWristGunIk = rightWristGunIk;
        this.anchorPoint = anchorPoint;
        this.priority = priority;
    }



    public enum LeftWristIK
    {
        None,
        Gun,
        Waist
    }

    public enum RightWristIK
    {
        None,
        Gun
    }

    public enum GunAnchorPoint
    {
        RightArm,
        Shoulder
    }
}

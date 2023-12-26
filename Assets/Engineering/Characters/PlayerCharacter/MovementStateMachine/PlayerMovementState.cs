using FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/State")]
    public class PlayerMovementState : BaseState<PlayerMovementStateMachine>
    {
        [SerializeField] private GunIKTargets gunIKTargets;
        public GunIKTargets GunIKTargets => gunIKTargets;

        public List<PlayerMovementStateAction> Actions = new List<PlayerMovementStateAction>();

        public List<PlayerMovementStateTransition> Transitions = new List<PlayerMovementStateTransition>();

        public override void Execute(PlayerMovementStateMachine machine) {
            Debug.Log("Exeucting State: " + this.name);
            foreach (PlayerMovementStateAction action in Actions)
                action.Execute(machine);

            foreach (PlayerMovementStateTransition transition in Transitions) {
                transition.Execute(machine);
                if (machine.CurrentState != this)
                    break;
            }
        }

        public override void Enter(PlayerMovementStateMachine machine) {
            Debug.Log("Entring State: " + this.name);
            foreach (PlayerMovementStateAction action in Actions)
                action.Enter(machine);
        }

        public override void Exit(PlayerMovementStateMachine machine) {
            Debug.Log("Exiting State: " + this.name);
            foreach (PlayerMovementStateAction action in Actions)
                action.Exit(machine);
        }
    }
}
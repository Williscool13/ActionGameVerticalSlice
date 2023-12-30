using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "DropAction", menuName = "Finite State Machine/Player Action/Actions/Common/Drop")]
    public class DropAction : PlayerActionStateAction
    {
        [SerializeField] PlayerMovementState[] whiteList;
        [SerializeField] PlayerActionState idleState;
        [SerializeField] PlayerActionState reloadState;
        [SerializeField] PlayerActionState swapState;
        public override void Enter(PlayerActionStateMachine machine) {
        }

        public override void Exit(PlayerActionStateMachine machine) {
        }

        public override void Execute(PlayerActionStateMachine machine) {
            if (!whiteList.Contains(machine.PlayerMovementStateMachine.CurrentState)) {
                return;
            }
            if (!machine.Inputs.DropPress) {
                return;
            }

            machine.TransitionState(idleState);

            machine.DropCurrentWeapon();


        }
    }
}
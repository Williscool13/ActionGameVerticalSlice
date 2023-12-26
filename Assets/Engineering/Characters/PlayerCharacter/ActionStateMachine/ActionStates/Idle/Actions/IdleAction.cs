using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "IdleAction", menuName = "Finite State Machine/Player Action/Actions/Idle")]
    public class IdleAction : PlayerActionStateAction {
        public override void Enter(PlayerActionStateMachine machine) {
        }

        public override void Exit(PlayerActionStateMachine machine) {
        }

        public override void Execute(PlayerActionStateMachine machine) {
        }
    }
}
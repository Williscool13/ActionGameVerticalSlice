using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "RotateAction", menuName = "Finite State Machine/Player Action/Actions/Common/Rotate")]
    public class RotateAction : PlayerActionStateAction
    {
        public override void Enter(PlayerActionStateMachine machine) {
        }

        public override void Exit(PlayerActionStateMachine machine) {
        }

        public override void Execute(PlayerActionStateMachine machine) {
            Vector2 m = machine.Inputs.MouseDelta;
            if (m.magnitude == 0) return;

            machine.RotateCharacter(m); 
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "AimAction", menuName = "Finite State Machine/Player Action/Actions/Aim")]
    public class AimAction : PlayerActionStateAction {
        public override void Enter(PlayerActionStateMachine machine) {
            machine.SetAnimatorBool("Aim", true);
            machine.AimDownSightStart();
        }

        public override void Exit(PlayerActionStateMachine machine) {

            machine.SetAnimatorBool("Aim", false);
            machine.AimDownSightEnd();
        }

        public override void Execute(PlayerActionStateMachine machine) {
        }
    }
}
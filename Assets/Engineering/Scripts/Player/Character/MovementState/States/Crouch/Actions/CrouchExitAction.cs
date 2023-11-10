using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerMovementFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Crouch/Exit")]
    public class CrouchExitAction : PlayerMovementStateAction
    {
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Crouch", false);
        }
    }
}

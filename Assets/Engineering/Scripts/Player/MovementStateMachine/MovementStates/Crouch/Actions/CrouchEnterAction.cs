using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Crouch/Enter")]
    public class CrouchEnterAction : PlayerMovementStateAction
    {
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Crouch", true);
        }

    }
}
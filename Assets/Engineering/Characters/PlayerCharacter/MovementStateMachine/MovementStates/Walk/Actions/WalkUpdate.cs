using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Walk/Update")]
    public class WalkUpdate : PlayerMovementStateAction
    {
        [SerializeField] float jumpForwardMultiplier = 1.5f;

        public override void Execute(PlayerMovementStateMachine machine) {
            if (machine.Inputs.JumpPress && machine.CanJump()) {
                machine.JumpRequested = true;
                machine.TimeSinceJumpRequested = 0f;
                machine.JumpForwardMultiplier = jumpForwardMultiplier;
            }
        }

    }

}
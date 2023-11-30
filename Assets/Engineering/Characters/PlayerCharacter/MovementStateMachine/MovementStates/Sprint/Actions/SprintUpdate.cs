using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Update")]
    public class SprintUpdate : PlayerMovementStateAction
    {
        [SerializeField] float jumpForwardMultiplier = 3.0f;
        public override void Execute(PlayerMovementStateMachine machine) {
            if (machine.Inputs.JumpPress && machine.CanJump()) {
                machine.JumpRequested = true;
                machine.TimeSinceJumpRequested = 0f;
                machine.JumpForwardMultiplier = jumpForwardMultiplier;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Walk/Walk To Sprint")]
    public class WalkToSprintDecision : PlayerMovementStateDecision {
        [SerializeField] List<PlayerActionState> validActionStates = new List<PlayerActionState>();
        public override bool Decide(PlayerMovementStateMachine machine) {
            if (!machine.IsGrounded() || machine.IsForceUngrounded()){
                return false;
            }

            if (machine.Inputs.SprintHold // holding sprint
                && machine.Inputs.RawMove.y > 0 // holding forward
                && machine.Inputs.RawMove.x == 0 // not moving left or right
                && machine.CurrentMove.y > 0.01f ) // moving forward
                {
                if (validActionStates.Contains(machine.PlayerActionStateMachine.CurrentState)) {
                    return true;
                }
            }
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Walk/Walk To Sprint")]
    public class WalkToSprintDecision : PlayerMovementStateDecision {
        [SerializeField] List<PlayerActionState> validActionStates = new List<PlayerActionState>();
        public override bool Decide(PlayerMovementStateMachine machine) {
            if (machine.SprintInput && machine.MoveDelta.y > 0.01f && machine.CurrentMove.y > 0.01f && machine.MoveDelta.x == 0) {
                if (validActionStates.Contains(machine.PlayerActionStateMachine.CurrentState)) {
                    return true;
                }
            }
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerMovementFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Walk/Walk To Sprint")]
    public class WalkToSprintDecision : PlayerMovementStateDecision {
        public override bool Decide(PlayerMovementStateMachine machine) {
            if (machine.IsSprinting && machine.MoveDelta.y >= 0 && machine.CurrentMove.y >= 0) {
                return true;
            }
            return false;
        }
    }
}
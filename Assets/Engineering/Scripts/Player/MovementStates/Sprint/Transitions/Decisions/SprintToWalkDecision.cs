using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerMovementFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Sprint/Sprint To Walk")]
    public class SprintToWalkDecision : PlayerMovementStateDecision {
        public override bool Decide(PlayerMovementStateMachine machine) {
            if (!machine.IsSprinting || machine.MoveDelta.y < 0.01f || machine.CurrentMove.y < 0.01f) { 
                return true;
            }
            return false;
        }
    }
}
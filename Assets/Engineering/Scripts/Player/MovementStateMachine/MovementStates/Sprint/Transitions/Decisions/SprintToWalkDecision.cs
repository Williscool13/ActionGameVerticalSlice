using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Sprint/Sprint To Walk")]
    public class SprintToWalkDecision : PlayerMovementStateDecision {
        public override bool Decide(PlayerMovementStateMachine machine) {
            if (!machine.SprintInput || machine.MoveDelta.y < 0.01f || machine.CurrentMove.y < 0.01f || (machine.MoveDelta.x != 0)) { 
                return true;
            }
            return false;
        }
    }
}
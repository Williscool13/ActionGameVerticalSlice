using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Idle/Idle To Obstructed")]
    public class IdleToObstructedDecision : PlayerActionStateDecision {
        public override bool Decide(PlayerActionStateMachine machine) {
            if (machine.IsIdleGunPositionObscured()) {
                return true;
            }

            return false;
        }
    }
}
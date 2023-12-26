using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{

    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Aim/Aim To Obstructed")]
    public class AimToObstructedDecision : PlayerActionStateDecision
    {
        public override bool Decide(PlayerActionStateMachine machine) {
            if (machine.IsIdleGunPositionObscured()) {
                return true;
            }

            return false;
        }
    }
}
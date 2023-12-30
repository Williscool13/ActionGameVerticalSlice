using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Aim/Aim To Unarmed")]
    public class AimToUnarmedDecision : PlayerActionStateDecision
    {
        public override bool Decide(PlayerActionStateMachine machine) {
            if (machine.IsUnarmed()) { return true; }

            return false;
        }
    }
}
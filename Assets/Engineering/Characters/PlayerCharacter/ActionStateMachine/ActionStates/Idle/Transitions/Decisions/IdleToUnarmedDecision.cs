using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Idle/Idle to Unarmed")]

    public class IdleToUnarmedDecision : PlayerActionStateDecision
    {
        public override bool Decide(PlayerActionStateMachine Machine) {
            if (Machine.IsUnarmed()) { return true; }

            return false;
        }
    }
}

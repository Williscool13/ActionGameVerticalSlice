using UnityEngine;

namespace PlayerMovementFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Walk/Walk To Crouch")]
    public class WalkToCrouchDecision : PlayerMovementStateDecision
    {
        public override bool Decide(PlayerMovementStateMachine machine) {
            return machine.IsCrouching && !machine.IsSprinting;
        }
    }
}

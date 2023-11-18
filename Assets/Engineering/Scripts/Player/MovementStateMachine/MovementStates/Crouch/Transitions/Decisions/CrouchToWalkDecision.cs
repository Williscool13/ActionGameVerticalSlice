using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Decisions/Crouch/Crouch To Walk")]
    public class CrouchToWalkDecision : PlayerMovementStateDecision
    {
        public override bool Decide(PlayerMovementStateMachine machine) {
            return !machine.IsCrouching;
        }
    }
}
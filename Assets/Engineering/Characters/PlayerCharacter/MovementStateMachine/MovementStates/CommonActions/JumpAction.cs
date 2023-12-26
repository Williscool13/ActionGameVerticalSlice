using UnityEngine;
namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Common/Jump")]
    public class JumpAction : PlayerMovementStateAction
    {
        public override void Enter(PlayerMovementStateMachine machine) {
        }

        public override void Exit(PlayerMovementStateMachine machine) {
        }

        public override void Execute(PlayerMovementStateMachine machine) {
            if (machine.Inputs.JumpPress && machine.CanJump) {
                machine.Jump();
            }
        }
    }

}
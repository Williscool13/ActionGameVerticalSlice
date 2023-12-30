using UnityEngine;
namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Common/Jump")]
    public class JumpAction : PlayerMovementStateAction
    {
        bool firstFrame = false;
        public override void Enter(PlayerMovementStateMachine machine) {
            firstFrame = true;
        }

        public override void Exit(PlayerMovementStateMachine machine) {
        }

        public override void Execute(PlayerMovementStateMachine machine) {
            if (firstFrame) {
                firstFrame = false;
                return;
            }

            if (machine.Inputs.JumpPress && machine.CanJump) {
                machine.Jump();
            }
        }
    }

}
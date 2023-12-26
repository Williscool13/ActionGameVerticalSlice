using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Crouch")]
    public class CrouchAction : PlayerMovementStateAction {
        [SerializeField] float maxSpeed = 2.5f;
        [SerializeField] float accelUpSpeed = 16f;
        [SerializeField] float accelDownSpeed = 2f;

        public override void Enter(PlayerMovementStateMachine machine) {
            machine.Crouch();

            machine.SetAnimatorBool("Crouch", true);

            machine.SetSpeedProperties(maxSpeed, accelUpSpeed, accelDownSpeed);
        }

        public override void Exit(PlayerMovementStateMachine machine) {
            machine.UnCrouch();

            machine.SetAnimatorBool("Crouch", false);
        }

        public override void Execute(PlayerMovementStateMachine machine) {
        }
    }

}
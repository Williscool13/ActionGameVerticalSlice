using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Falling")]
    public class FallingAction : PlayerMovementStateAction {
        [SerializeField] float maxSpeed = 7.5f;
        [SerializeField] float accelUpSpeed = 16f;
        [SerializeField] float accelDownSpeed = 2f;

        public override void Enter(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Falling", true);

            machine.SetSpeedProperties(maxSpeed, accelUpSpeed, accelDownSpeed);
        }

        public override void Exit(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Falling", false);
        }

        public override void Execute(PlayerMovementStateMachine machine) {
        }
    }

}
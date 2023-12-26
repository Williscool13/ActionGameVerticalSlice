using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "WalkAction", menuName = "Finite State Machine/Player Movement/Actions/Walk")]
    public class WalkAction : PlayerMovementStateAction {
        [SerializeField] float maxSpeed = 4.5f;
        [SerializeField] float accelUpSpeed = 16f;
        [SerializeField] float accelDownSpeed = 2f;

        public override void Enter(PlayerMovementStateMachine machine) {
            machine.SetSpeedProperties(maxSpeed, accelUpSpeed, accelDownSpeed);
        }

        public override void Exit(PlayerMovementStateMachine machine) {
        }

        public override void Execute(PlayerMovementStateMachine machine) {
        }
    }
}

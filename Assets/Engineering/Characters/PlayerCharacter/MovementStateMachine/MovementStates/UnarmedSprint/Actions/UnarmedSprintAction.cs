using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/UnarmedSprint")]
    public class UnarmedSprintAction : PlayerMovementStateAction
    {
        [SerializeField] float maxSpeed = 9.0f;
        [SerializeField] float accelUpSpeed = 24f;
        [SerializeField] float accelDownSpeed = 2f;

        public override void Enter(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Sprint", true);
            machine.StartSprint();
            machine.SetSpeedProperties(maxSpeed, accelUpSpeed, accelDownSpeed);
        }

        public override void Exit(PlayerMovementStateMachine machine) {
            machine.StopSprint();
            machine.SetAnimatorBool("Sprint", false);
        }

        public override void Execute(PlayerMovementStateMachine machine) {
        }
    }


}
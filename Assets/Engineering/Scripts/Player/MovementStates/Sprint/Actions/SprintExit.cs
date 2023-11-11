using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerMovementFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Exit")]
    public class SprintExit : PlayerMovementStateAction {
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Sprint", false);
            machine.AimGunPosition();
            Debug.Log("Sprint Exit");
        }
    }

}
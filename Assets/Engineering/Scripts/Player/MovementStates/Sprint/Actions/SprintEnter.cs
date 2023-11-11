using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerMovementFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Enter")]
    public class SprintEnter : PlayerMovementStateAction {
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Sprint", true);
            machine.SprintGunPosition();
            Debug.Log("Sprint Enter");
        }
    }
}

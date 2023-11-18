using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Sprint/Enter")]
    public class SprintEnter : PlayerMovementStateAction {
        [SerializeField] BoolVariable isSprinting;
        public override void Execute(PlayerMovementStateMachine machine) {
            machine.SetAnimatorBool("Sprint", true);
            isSprinting.Value = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Unarmed")]
    public class UnarmedAction : PlayerActionStateAction {
        [SerializeField] PlayerActionState exitState;
        public override void Enter(PlayerActionStateMachine machine) {
            machine.SetAnimatorTrigger("Unarmed");
        }

        public override void Execute(PlayerActionStateMachine machine) { 
            if (machine.Inputs.StowPress) { 
                machine.DrawWeapon();
                machine.TransitionState(exitState);
            }
        }
        
        public override void Exit(PlayerActionStateMachine machine) {
            machine.SetAnimatorTrigger("Armed");
        }
    }
}
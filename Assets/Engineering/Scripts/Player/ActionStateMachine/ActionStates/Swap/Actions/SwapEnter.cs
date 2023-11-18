using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Swap/Enter")]
    public class SwapEnter : PlayerActionStateAction {
        [SerializeField] BoolVariable isSwapping;
        [SerializeField] FloatVariable transformSwapTimeleft;
        [SerializeField] FloatVariable swapTimeLeft;
        [SerializeField] BoolVariable weaponSwapped;
        [SerializeField] AnimationClip anim;
        public override void Execute(PlayerActionStateMachine machine) {
            isSwapping.Value = true;
            weaponSwapped.Value = false;
            float overallTime = anim.length / 3.0f;
            transformSwapTimeleft.Value = overallTime * 0.7f;
            swapTimeLeft.Value = overallTime + 0.2f;

            machine.SetAnimatorTrigger("Swap");
        }
    }
}
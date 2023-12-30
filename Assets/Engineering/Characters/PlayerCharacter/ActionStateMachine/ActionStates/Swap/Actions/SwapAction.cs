using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Swap")]
    public class SwapAction : PlayerActionStateAction {
        [SerializeField] AnimationClip anim;
        float transformSwapTimeleft;
        float swapTimeLeft;
        public override void Enter(PlayerActionStateMachine machine) {
            float overallTime = anim.length / 3.0f;
            transformSwapTimeleft = overallTime * 0.7f;
            swapTimeLeft = overallTime + 0.2f;

            weaponTransformSwapped = false;
            weaponSwapped = false;

            machine.SetAnimatorTrigger("Swap");
        }

        bool weaponTransformSwapped = false;
        bool weaponSwapped = false;
        [SerializeField] PlayerActionState exitState;

        public override void Execute(PlayerActionStateMachine machine) {
            transformSwapTimeleft -= Time.deltaTime;
            swapTimeLeft -= Time.deltaTime;

            if (!weaponTransformSwapped) {
                if (transformSwapTimeleft <= 0) {
                    machine.SwapWeapon();
                    weaponTransformSwapped = true;
                }
            }

            if (!weaponSwapped) {
                if (swapTimeLeft <= 0) {
                    machine.TransitionState(exitState);
                    weaponSwapped = true;
                    return;
                }
            }

        }

        public override void Exit(PlayerActionStateMachine machine) {
            if (swapTimeLeft > 0) {
                machine.SetAnimatorTrigger("SwapCancel");
            }
        }
    }
}
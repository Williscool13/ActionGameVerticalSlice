using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Reload")]

    public class ReloadAction : PlayerActionStateAction
    {
        [SerializeField] PlayerActionState exitState;
        [SerializeField] AnimationClip anim;
        float reloadTimeLeft;
        bool reloaded = false;
        public override void Enter(PlayerActionStateMachine machine) {
            machine.ReloadStart();

            float reloadSpeedMultiplier = machine.GetCurrentWeapon().GetReloadSpeedMultiplier();
            // 0.75 is the hard coded exit time of the reload animation
            reloadTimeLeft = (anim.length * 0.75f) / reloadSpeedMultiplier;
            reloaded = false;

            machine.SetAnimatorFloat("ReloadSpeedMultiplier", reloadSpeedMultiplier);
            machine.SetAnimatorTrigger("Reload");
        }
    

        public override void Exit(PlayerActionStateMachine machine) {
            if (reloadTimeLeft > 0) {
                machine.SetAnimatorTrigger("ReloadCancel");
            }
        }

        public override void Execute(PlayerActionStateMachine machine) {
            reloadTimeLeft -= Time.deltaTime;
            if (reloadTimeLeft <= 0 && !reloaded) {
                reloaded = true;
                machine.ReloadEnd();
                machine.TransitionState(exitState);
            }
        }
    }
}
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Actions/Reload/Enter")]
    public class ReloadEnter : PlayerActionStateAction
    {
        [SerializeField] BoolVariable isReloading;
        [SerializeField] FloatVariable reloadTimeleft;
        [SerializeField] AnimationClip anim;
        public override void Execute(PlayerActionStateMachine machine) {
            isReloading.Value = true;

            machine.PlayerLoadoutManager.GetCurrentWeapon().ReloadStart();
            
            float reloadSpeedMultiplier = machine.PlayerLoadoutManager.GetCurrentWeapon().GetReloadSpeedMultiplier();
            // 0.75 is the hard coded exit time of the reload animation
            reloadTimeleft.Value = (anim.length * 0.75f) / reloadSpeedMultiplier;

            machine.SetAnimatorFloat("ReloadSpeedMultiplier", reloadSpeedMultiplier);
            machine.SetAnimatorTrigger("Reload");
        }
    }
}
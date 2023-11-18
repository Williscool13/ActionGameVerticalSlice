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
            reloadTimeleft.Value = anim.length * 0.75f / 1f + 0.1f;

            machine.SetAnimatorTrigger("Reload");
        }
    }
}
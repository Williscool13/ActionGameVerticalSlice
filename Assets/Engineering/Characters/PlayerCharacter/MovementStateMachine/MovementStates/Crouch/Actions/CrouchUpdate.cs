using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/Player Movement/Actions/Crouch/Update")]
    public class CrouchUpdate : PlayerMovementStateAction
    {
        [SerializeField] float lerpUpSpeed = 12f;
        [SerializeField] float lerpDownSpeed = 20f;
        public override void Execute(PlayerMovementStateMachine machine) {
        }
    }
}


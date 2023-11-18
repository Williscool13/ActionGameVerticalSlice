using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine {
    [CreateAssetMenu(menuName = "Finite State Machine/Player Action/Decisions/Aim/Aim To Reload")]
    public class AimToReloadDecision : PlayerActionStateDecision
    {
        [SerializeField] List<PlayerMovementState> validMovementStates = new List<PlayerMovementState>();
        public override bool Decide(PlayerActionStateMachine machine) {
            if (!machine.ReloadPress) {
                return false;
            }
            if (validMovementStates.Contains(machine.PlayerMovementStateMachine.CurrentState)) {
                return true;
            }


            return false;
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(fileName = "InteractAction", menuName = "Finite State Machine/Player Action/Actions/Common/Interact")]
    public class InteractAction : PlayerActionStateAction
    {
        [SerializeField] PlayerMovementState[] whiteList;
        public override void Enter(PlayerActionStateMachine machine) {
        }

        public override void Exit(PlayerActionStateMachine machine) {
        }

        public override void Execute(PlayerActionStateMachine machine) {
            if (!whiteList.Contains(machine.PlayerMovementStateMachine.CurrentState)) return;

            IInteractable inter = machine.GetInteractable();
            inter?.Highlight();

            if (!machine.Inputs.InteractPress) return;

            Debug.Log("Interacted");

            // drawline from camera forward and check if it hits any IIteractable
            if (inter == null) return;
            switch (inter.Type) {
                case InteractableType.Pressable:
                    inter.Interact();
                    break;
                case InteractableType.Item:
                    inter.Interact();
                    machine.AddItemToInventory(inter as ItemInstance);
                    // add to inventory
                    break;
            }
        }
    }
}

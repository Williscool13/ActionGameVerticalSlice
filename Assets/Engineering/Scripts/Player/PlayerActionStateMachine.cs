using FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFiniteStateMachine
{
    public class PlayerActionStateMachine : BaseStateMachine
    {
        [SerializeField] private PlayerActionState _initialState;

        [SerializeField] private PlayerInput playerInput;

        [SerializeField] private PlayerMovementStateMachine playerMovementStateMachine;
        [SerializeField] private PlayerLoadoutManager loadoutManager;

        public PlayerMovementStateMachine PlayerMovementStateMachine => playerMovementStateMachine;

        public PlayerActionState CurrentState { get; set; }

        public bool ReloadPress => reloadPress;
        public bool SwapPress => swapPress;


        public override void Awake() {
            CurrentState = _initialState;
        }
        public override void Update() {
            CurrentState.Execute(this);



            reloadPress = false;
            swapPress = false;
        }


        bool reloadHold = false;
        bool reloadPress = false;
        public void OnReload(InputAction.CallbackContext context) {
            if (context.started) {
                reloadHold = true;
                reloadPress = true;
            }

            if (context.canceled) {
                reloadHold = false;
            }
        }
        bool swapHold = false;
        bool swapPress = false;
        public void OnSwap(InputAction.CallbackContext context) {
            if (context.started) {
                swapHold = true;
                swapPress = true;
            }

            if (context.canceled) {
                swapHold = false;
            }
        }

        public void TransitionState(PlayerActionState targetState) {
            Debug.Log("Transitioning to " + targetState.name);
            CurrentState.Exit(this);
            CurrentState = targetState;
            CurrentState.Enter(this);
        }

        [SerializeField] Animator animator;
        public void SetAnimatorTrigger(string name) {
            animator.SetTrigger(name);
        }

        public void SwapWeapon() {
            Debug.Log("Duck 1");
            loadoutManager.SwapWeapon(loadoutManager.CurrentWeapon == WeaponSlot.Primary ? WeaponSlot.Secondary : WeaponSlot.Primary);
        }
    }

}
using FiniteStateMachine;
using System;
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
        [SerializeField] Animator animator;

        [SerializeField] private GunRigController gunRigController;
        [SerializeField] private PlayerLoadoutManager playerLoadoutManager;
        [SerializeField] private PlayerAimController aimController;
        public event EventHandler<PlayerActionStateChangeEventArgs> OnPlayerActionStateChange;

        public PlayerMovementStateMachine PlayerMovementStateMachine => playerMovementStateMachine;
        public PlayerActionState CurrentState { get; set; }
        public GunRigController GunRigController { get { return gunRigController; } }
        public PlayerLoadoutManager PlayerLoadoutManager { get { return playerLoadoutManager; } }
        public PlayerAimController PlayerAimController { get { return aimController; } }
        public bool ReloadPress => reloadPress;
        public bool ReloadHold => reloadHold;
        public bool SwapPress => swapPress;
        public bool SwapHold => swapHold;
        public bool ShootPress => shootPress;
        public bool ShootHold => shootHold;
        public  bool AimPress => aimPress;
        public bool AimHold => aimHold;

        public override void Awake() {
            CurrentState = _initialState;
        }
        public override void Update() {
            CurrentState.Execute(this);

            reloadPress = false;
            swapPress = false;
            shootPress = false;
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
        bool shootHold = false;
        bool shootPress = false;
        public void OnShoot(InputAction.CallbackContext context) {
            if (context.started) {
                shootHold = true;
                shootPress = true;
            }
            if (context.canceled) {
                shootHold = false;
            }
        }
        bool aimHold = false;
        bool aimPress = false;
        public void OnAim(InputAction.CallbackContext context) {
            if (context.started) {
                aimHold = true;
                aimPress = true;
            }
            if (context.canceled) {
                aimHold = false;
            }
        }
        public void TransitionState(PlayerActionState targetState) {
            Debug.Log("[Player Action] Transitioning to " + targetState.name);
            PlayerActionStateChangeEventArgs args = new() { 
                PreviousState = CurrentState, 
                NewState = targetState 
            };
            CurrentState.Exit(this);
            CurrentState = targetState;
            CurrentState.Enter(this);
            OnPlayerActionStateChange?.Invoke(this, args);
        }

        public void SetAnimatorTrigger(string name) {
            animator.SetTrigger(name);
        }
        public void SetAnimatorFloat(string name, float value) {
            animator.SetFloat(name, value);
        }
        public void SetAnimatorBool(string name, bool value) {
            animator.SetBool(name, value);
        }
        public void SwapWeapon() {
            playerLoadoutManager.SwapWeapon();
        }
    }

    public class PlayerActionStateChangeEventArgs : EventArgs
    {
        public PlayerActionState PreviousState { get; set; }
        public PlayerActionState NewState { get; set; }
    }
}

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

        public PlayerActionInputs Inputs { get { return inputs; } }
        PlayerActionInputs inputs = new PlayerActionInputs();

        public override void Awake() {
            CurrentState = _initialState;
        }
        public override void Update() {
            Debug.Log("Exeucting State: " + CurrentState.name);
            CurrentState.Execute(this);
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

        public void SetInputs(bool reloadPress, bool reloadHold, bool swapPress, bool swapHold, bool shootPress, bool shootHold, bool aimPress, bool aimHold) {
            this.inputs.SwapPress = swapPress;
            this.inputs.SwapHold = swapHold;
            this.inputs.ReloadPress = reloadPress;
            this.inputs.ReloadHold = reloadHold;
            this.inputs.ShootPress = shootPress;
            this.inputs.ShootHold = shootHold;
            this.inputs.AimPress = aimPress;
            this.inputs.AimHold = aimHold;
        }
    }

    public class PlayerActionStateChangeEventArgs : EventArgs
    {
        public PlayerActionState PreviousState { get; set; }
        public PlayerActionState NewState { get; set; }
    }
}

public struct PlayerActionInputs
{
    public bool ReloadPress;
    public bool ReloadHold;
    public bool SwapPress;
    public bool SwapHold;
    public bool ShootPress;
    public bool ShootHold;
    public bool AimPress;
    public bool AimHold;
}

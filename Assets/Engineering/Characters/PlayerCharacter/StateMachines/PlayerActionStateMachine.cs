using FiniteStateMachine;
using Sirenix.OdinInspector;
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
        [SerializeField][ReadOnly] public PlayerActionState _currentState;

        private GunRigController GunRigController { get { return gunRigController; } }
        private PlayerLoadoutManager PlayerLoadoutManager { get { return playerLoadoutManager; } }
        private PlayerAimController PlayerAimController { get { return aimController; } }

        public PlayerActionInputs Inputs { get { return inputs; } }
        PlayerActionInputs inputs = new PlayerActionInputs();

        public override void Awake() {
            CurrentState = _initialState;
        }

        public override void Update() {
            _currentState = CurrentState;
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

        public void StowWeapon() {
            PlayerLoadoutManager.StowWeapon();
            TransitionState(unarmedState);
        }

        public void DrawWeapon() {
            PlayerLoadoutManager.DrawWeapon();
        }

        public void SwapWeapon() {
            PlayerLoadoutManager.SwapWeapon();
        }

        public WeaponBase GetCurrentWeapon() {
            return PlayerLoadoutManager.GetCurrentWeapon();
        }

        public bool CanReload() {
            return PlayerLoadoutManager.GetCurrentWeapon().CanReload();
        }
        public void ReloadStart() {
            PlayerLoadoutManager.GetCurrentWeapon().ReloadStart();
        }

        public void ReloadEnd() {
            PlayerLoadoutManager.GetCurrentWeapon().ReloadEnd();
        }

        public void AimDownSightStart() {
            PlayerAimController.AimingDownSights = true;
        }
        public void AimDownSightEnd() {
            PlayerAimController.AimingDownSights = false;
        }

        public void AddRecoil(RecoilData data) {
            PlayerAimController.AddRecoil(data);
        }

        #region Rig Queries
        public bool IsRigInPosition() {
            return GunRigController.RigInPosition();
        }

        public bool IsIdleGunPositionObscured() {
            return GunRigController.GunIdlePositionObstructed();
        }

        public bool IsAimGunPositionObscured() {
            return GunRigController.GunAimPositionObscured();
        }
        #endregion

        [SerializeField] private PlayerActionState unarmedState;
        [Button("Unarm Player")]
        public void UnarmPlayer() {
            TransitionState(unarmedState);
        }

        public void SetInputs(bool reloadPress, bool reloadHold, bool swapPress, bool swapHold, bool shootPress, bool shootHold, bool aimPress, bool aimHold, bool stowPress, bool stowHold) {
            this.inputs.SwapPress = swapPress;
            this.inputs.SwapHold = swapHold;
            this.inputs.ReloadPress = reloadPress;
            this.inputs.ReloadHold = reloadHold;
            this.inputs.ShootPress = shootPress;
            this.inputs.ShootHold = shootHold;
            this.inputs.AimPress = aimPress;
            this.inputs.AimHold = aimHold;
            this.inputs.StowPress = stowPress;
            this.inputs.StowHold = stowHold;
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
    public bool StowPress;
    public bool StowHold;
}

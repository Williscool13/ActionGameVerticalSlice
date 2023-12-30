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
        [SerializeField] private PlayerInventoryManager inventoryManager;
        
        public event EventHandler<PlayerActionStateChangeEventArgs> OnPlayerActionStateChange;

        public PlayerMovementStateMachine PlayerMovementStateMachine => playerMovementStateMachine;
        public PlayerActionState CurrentState { get; set; }
        [SerializeField][ReadOnly] public PlayerActionState _currentState;

        private GunRigController GunRigController { get { return gunRigController; } }
        private PlayerLoadoutManager PlayerLoadoutManager { get { return playerLoadoutManager; } }
        private PlayerAimController PlayerAimController { get { return aimController; } }
        private PlayerInventoryManager PlayerInventoryManager { get { return inventoryManager; } }

        public PlayerActionInputs Inputs { get { return inputs; } }
        PlayerActionInputs inputs = new PlayerActionInputs();

        public override void Awake() {
            CurrentState = _initialState;
            CurrentState.Enter(this);

            Debug.Assert(GunRigController != null, "GunRigController is null");
            Debug.Assert(PlayerLoadoutManager != null, "PlayerLoadoutManager is null");
            Debug.Assert(PlayerAimController != null, "PlayerAimController is null");
            Debug.Assert(inventoryManager != null, "PlayerInventoryManager is null");
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
            PlayerLoadoutManager.StowWeapons();
            //TransitionState(unarmedState);
        }

        public void DrawWeapon() {
            PlayerLoadoutManager.DrawWeapons();
        }
        public bool CanSwap() {
            return PlayerLoadoutManager.CanSwap();
        }
        public void SwapWeapon() {
            PlayerLoadoutManager.SwapWeapon();
        }
        public bool IsUnarmed() {
            return PlayerLoadoutManager.IsUnarmed();
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

        public IInteractable GetInteractable() {
            return PlayerAimController.GetInteractable();
        }

        public void AddItemToInventory(ItemInstance item) {
            PlayerInventoryManager.AddItem(item);
        }

        public void DropCurrentWeapon() {
            PlayerInventoryManager.DropCurrentWeapon();
        }

        public void RotateCharacter(Vector2 rotation) {
            PlayerAimController.RotateCharacter(rotation);
        }

        #region Rig Queries
        public bool IsRigInPosition() {
            return GunRigController.RigInPosition();
        }

        public bool IsIdleGunPositionObscured() {
            return GunRigController.GunIdlePositionObstructed();
        }

        #endregion


        public void SetInputs(bool reloadPress, bool reloadHold, bool swapPress, bool swapHold, bool shootPress, bool shootHold,
            bool aimPress, bool aimHold, bool stowPress, bool stowHold, bool interactPress, bool interactHold, 
            bool dropPress, bool dropHold,
            Vector2 mouseDelta) 
            {
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
            this.inputs.InteractPress = interactPress;
            this.inputs.InteractHold = interactHold;
            this.inputs.DropPress = dropPress;
            this.inputs.DropHold = dropHold;

            this.inputs.MouseDelta = mouseDelta;
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
    public bool InteractPress;
    public bool InteractHold;
    public bool DropPress;
    public bool DropHold;

    public Vector2 MouseDelta;
}

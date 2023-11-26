using FiniteStateMachine;
using NUnit.Framework.Interfaces;
using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFiniteStateMachine
{
    public class PlayerMovementStateMachine : BaseStateMachine
    {
        [SerializeField] private PlayerMovementState _initialState;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] PlayerActionStateMachine playerActionStateMachine;
        [SerializeField] private Animator animator;


        [SerializeField] GunRigController RigStateSelector;

        public event EventHandler<PlayerMovementStateChangeEventArgs> OnPlayerMovementStateChange;

        public PlayerActionStateMachine PlayerActionStateMachine => playerActionStateMachine;
        public PlayerMovementState CurrentState { get; set; }
        public PlayerInput PlayerInput => _playerInput;
        public bool IsCrouching => crouching;
        public bool SprintInput => sprinting;
        public Vector2 MoveDelta => moveDelta;
        public Vector2 CurrentMove {
            get { return currentMove; }
            set { currentMove = value; }
        }


        Vector2 moveDelta;
        Vector2 currentMove = Vector2.zero;
        bool crouching;
        bool sprinting;

        public override void Awake() {
            CurrentState = _initialState;
        }


        public override void Update() {
            moveDelta = _playerInput.actions["Move"].ReadValue<Vector2>();
            CurrentState.Execute(this);
        }

        public void TransitionState(PlayerMovementState targetState) {
            Debug.Log("Transitioning to " + targetState.name);
            PlayerMovementStateChangeEventArgs args = new() {
                PreviousState = CurrentState,
                NewState = targetState
            };
            CurrentState.Exit(this);
            CurrentState = targetState;
            CurrentState.Enter(this);
            OnPlayerMovementStateChange.Invoke(this, args);
        }

        public void OnCrouch(InputAction.CallbackContext context) {
            if (context.performed) {
                crouching = true;
            }
            if (context.canceled) {
                crouching = false;
            }
        }
        public void OnSprint(InputAction.CallbackContext context) {
            if (context.performed) {
                sprinting = true;
            }
            if (context.canceled) {
                sprinting = false;
            }
        }


        #region Animation
        public void SetAnimatorBool(string name, bool value) {
            animator.SetBool(name, value);
        }
        public void SetAnimatorFloat(string name, float value) {
            animator.SetFloat(name, value);
        }
        public void SetAnimatorTrigger(string name) {
            animator.SetTrigger(name);
        }

        #endregion
    }

    public class PlayerMovementStateChangeEventArgs : EventArgs {
        public PlayerMovementState PreviousState { get; set; }
        public PlayerMovementState NewState { get; set; }
    }

}
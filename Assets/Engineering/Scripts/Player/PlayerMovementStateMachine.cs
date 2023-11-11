using FiniteStateMachine;
using NUnit.Framework.Interfaces;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMovementFiniteStateMachine
{
    public class PlayerMovementStateMachine : BaseStateMachine
    {
        [SerializeField] private PlayerMovementState _initialState;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Animator _playerAnimator;

        [SerializeField] private FloatVariable testFloat;
        [SerializeField] private FloatReference testFloat2;

        [SerializeField] GunTransformLerp gunTransformLerp;
        public PlayerMovementState CurrentState { get; set; }

        public PlayerInput PlayerInput => _playerInput;
        public bool IsCrouching => crouching;
        public bool IsSprinting => sprinting;
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
            CurrentState.Exit(this);
            CurrentState = targetState;
            CurrentState.Enter(this);
        }


        public void SprintGunPosition() {
            gunTransformLerp.SetLerpTargetSprint();
        }
        public void AimGunPosition() {
            gunTransformLerp.SetLerpTargetAim();
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
            _playerAnimator.SetBool(name, value);
        }
        public void SetAnimatorFloat(string name, float value) {
            _playerAnimator.SetFloat(name, value);
        }
        public void SetAnimatorTrigger(string name) {
            _playerAnimator.SetTrigger(name);
        }

        #endregion
    }

}
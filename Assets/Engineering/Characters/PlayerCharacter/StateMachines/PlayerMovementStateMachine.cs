using DG.Tweening;
using FiniteStateMachine;
using PlayerFiniteStateMachine;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    public class PlayerMovementStateMachine : BaseStateMachine, ICharacterMovementController
    {


        [SerializeField] private PlayerMovementState _initialState;
        [SerializeField] private PlayerAimController aimController;
        [SerializeField] private Animator animator;

        [Header("Adjacent State Machines")]
        [SerializeField] private PlayerActionStateMachine playerActionStateMachine;

        [Header("Footsteps")]
        [SerializeField] private PlayerFootstepManager playerFootstepManager;

        public event EventHandler<PlayerMovementStateChangeEventArgs> OnPlayerMovementStateChange;

        public PlayerMovementInputs Inputs { get { return inputs; } }
        private PlayerMovementInputs inputs = new();


        ICharacterMotor characterMotor;

        /// State Machine Properties
        public PlayerActionStateMachine PlayerActionStateMachine => playerActionStateMachine;
        public PlayerMovementState CurrentState { get; set; }

        [SerializeField][ReadOnly] public PlayerMovementState _currentState;

        public float StateMaxSpeed { get; set; } = 1f;
        public float StateAccelUpSpeed { get; set; } = 1f;
        public float StateAccelDownSpeed { get; set; } = 1f;


        #region ICharacterMovementController
        public bool Crouching { get; set; }
        public bool Sprinting { get; set; }
        #endregion


        // Movement Properties
        public Vector3 CurrentLocalVelocity => transform.InverseTransformDirection(characterMotor.CharacterMovementSpeedWS);
        public bool IsGrounded => characterMotor.Grounded;
        public bool WasGroundedLastFrame => characterMotor.WasGrounded;
        public bool IsForceUngrounded => characterMotor.IsForceUngrounded;

        // Crouch Properties
        public bool CanUncrouch => characterMotor.CanUncrouch();

        // Jump Properties
        public bool CanJump => characterMotor.CanJump();

        
        


        public override void Awake() {
            characterMotor = GetComponent<ICharacterMotor>();
            if (characterMotor == null) { Debug.LogError("No ICharacterMotorProperties found on " + gameObject.name); }
            CurrentState = _initialState;
            CurrentState.Enter(this);
        }

        bool _wasGrounded = false;
        public override void Update() {
            _currentState = CurrentState;
            CurrentState.Execute(this);

            if (IsGrounded && !IsForceUngrounded) { SetAnimatorBool("Jumping", false); }

            animator.SetFloat("MovementX", Mathf.MoveTowards(animator.GetFloat("MovementX"), inputs.RawMove.x, Time.deltaTime / 0.15f));
            animator.SetFloat("MovementY", Mathf.MoveTowards(animator.GetFloat("MovementY"), inputs.RawMove.y, Time.deltaTime / 0.15f));

            characterMotor.MoveCommand(inputs.RawMove);


            if (!_wasGrounded && IsGrounded) { playerFootstepManager.FootstepLand(); }
            //if (!WasGroundedLastFrame && IsGrounded) { playerFootstepManager.FootstepLand(); }
            _wasGrounded = IsGrounded;
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

        public void Crouch() {
            this.Crouching = true;
            characterMotor.CrouchCommand();
        }

        public void UnCrouch() {
            this.Crouching = false;
            characterMotor.UnCrouchCommand();
        }

        public void Jump() {
            characterMotor.JumpCommand();
            SetAnimatorBool("Jumping", true);
            playerFootstepManager.FootstepJump();
        }

        public void SetSpeedProperties(float maxSpeed, float accTime, float deaccTime) {
            characterMotor.SetSpeedProperties(maxSpeed, accTime, deaccTime);

        }

        public void StartSprint() {
            aimController.Sprinting = true;
        }

        public void StopSprint() {
            aimController.Sprinting = false;
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



        public void SetInput(Vector2 move, Vector2 look, bool sprintPress, bool sprintHold, bool crouchPress, bool crouchHold, bool jumpPress, bool jumpHold) {
            this.inputs.RawMove = move;
            this.inputs.RawLook = look;
            this.inputs.SprintPress = sprintPress;
            this.inputs.SprintHold = sprintHold;
            this.inputs.CrouchPress = crouchPress;
            this.inputs.CrouchHold = crouchHold;
            this.inputs.JumpPress = jumpPress;
            this.inputs.JumpHold = jumpHold;
        }

     


    }


}


public class PlayerMovementStateChangeEventArgs : EventArgs
{
    public PlayerMovementState PreviousState { get; set; }
    public PlayerMovementState NewState { get; set; }
}

public interface ICharacterMovementController
{
    public bool Crouching { get; set; }
    public bool Sprinting { get; set; }
}

public struct PlayerMovementInputs
{
    public bool CrouchPress;
    public bool CrouchHold;
    public bool SprintPress;
    public bool SprintHold;
    public bool JumpPress;
    public bool JumpHold;
    public Vector3 Move;
    public Quaternion planarRot;
    public Quaternion verticalLook;

    public Vector2 RawMove;
    public Vector2 RawLook;

    public Vector3 planarDir;

}
using DG.Tweening;
using FiniteStateMachine;
using KinematicCharacterController;
using NUnit.Framework.Interfaces;
using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFiniteStateMachine
{
    public class PlayerMovementStateMachine : BaseStateMachine, ICharacterController
    {
        [SerializeField] private KinematicCharacterMotor motor;
        public KinematicCharacterMotor Motor => motor;
        [SerializeField] private PlayerMovementState _initialState;
        [SerializeField] private PlayerActionStateMachine playerActionStateMachine;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerAimController aimController;
        [SerializeField] private GunRigController gunRigController;

        [Header("Air Movement")]
        [SerializeField] float AirAccelerationSpeed = 10.0f;
        [SerializeField] float AirMaxSpeed = 10.0f;
        [SerializeField] float AirDrag = 0.1f;

        [Header("Jumping")]
        [SerializeField] private bool allowJumpingWhenSliding = false;
        [SerializeField] private float jumpUpSpeed = 10f;
        // bunny hopping
        [SerializeField] private float jumpPreGroundingGraceTime = 0f;
        // coyote time
        [SerializeField] private float jumpPostGroundingGraceTime = 0f;

        [Header("Misc")]
        [SerializeField] float GravityOrientationSharpness = 10.0f;

        [SerializeField] private Vector3 gravity = new Vector3(0, -30f, 0);
        public Vector3 Gravity { get { return gravity; } set { gravity = value; } }
       

        public event EventHandler<PlayerMovementStateChangeEventArgs> OnPlayerMovementStateChange;

        public PlayerActionStateMachine PlayerActionStateMachine => playerActionStateMachine;
        public PlayerMovementState CurrentState { get; set; }
        public PlayerMovementInputs Inputs { get { return inputs; } }
        private PlayerMovementInputs inputs = new PlayerMovementInputs();
        public Vector2 MoveDelta => inputs.RawMove;
        public Vector2 CurrentMove {
            get { return currentMove; }
            set { currentMove = value; }
        }
        Vector2 currentMove = Vector2.zero;
        Vector3 PlanarDirection { get; set; }

        /// State Machine Properties
        public float StateMaxSpeed { get; set; } = 1f;
        public float StateAccelUpSpeed { get; set; } = 1f;
        public float StateAccelDownSpeed { get; set; } = 1f;

        public bool JumpRequested { get; set; }
        public float TimeSinceJumpRequested { get; set; } = float.MaxValue;
        public float JumpForwardMultiplier { get; set; } = 1f;


        /// Jump Properties
        // coyote time
        private float _timeSinceLastAbleToJump = 0f;
        bool _jumpedThisFrame;
        bool _jumpConsumed;



        public override void Awake() {
            CurrentState = _initialState;
            CurrentState.Enter(this);
        }

        private void Start() {
            Motor.CharacterController = this;
            PlanarDirection = transform.forward;
        }

        public override void Update() {
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


        [ReadOnly][SerializeField] Vector3 currentVelocity = Vector3.zero;

        public void SetInput(Vector2 move, Vector2 look, bool sprintPress, bool sprintHold, bool crouchPress, bool crouchHold, bool jumpPress, bool jumpHold) {
            Vector3 moveInputVector = new Vector3(move.x, 0f, move.y);

            // Calculate camera direction and rotation on the character plane
            //Quaternion transformRotation = Quaternion.Euler(GetCharacterRotation());
            Quaternion transformRotation = transform.rotation;
            Vector3 transformPlanarDirection = Vector3.ProjectOnPlane(transformRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (transformPlanarDirection.sqrMagnitude == 0f) {
                transformPlanarDirection = Vector3.ProjectOnPlane(transformRotation * Vector3.up, Motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(transformPlanarDirection, Motor.CharacterUp);

            this.inputs.Move = cameraPlanarRotation * moveInputVector;


            // Convert the world-relative move input vector into a local-relative
            Quaternion rotationFromInput = Quaternion.Euler(transform.up * aimController.GetXRotateDelta(look.x));
            PlanarDirection = rotationFromInput * PlanarDirection;
            PlanarDirection = Vector3.Cross(transform.up, Vector3.Cross(PlanarDirection, transform.up));
            Quaternion planarRot = Quaternion.LookRotation(PlanarDirection, transform.up);

            // planar (horizontal) rotation
            this.inputs.planarRot = planarRot;
            // planar (horizontal) direction
            this.inputs.planarDir = PlanarDirection;

            this.inputs.RawMove = move;
            this.inputs.RawLook = look;
            this.inputs.SprintPress = sprintPress;
            this.inputs.SprintHold = sprintHold;
            this.inputs.CrouchPress = crouchPress;
            this.inputs.CrouchHold = crouchHold;
            this.inputs.JumpPress = jumpPress;
            this.inputs.JumpHold = jumpHold;
        }

        #region ICharacterController
        public void BeforeCharacterUpdate(float deltaTime) {
        }
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            currentRotation = Quaternion.LookRotation(inputs.planarDir, Motor.CharacterUp);

            Vector3 currentUp = (currentRotation * Vector3.up);


            // Rotates the character to align with gravity
            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-GravityOrientationSharpness * deltaTime));
            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;


            aimController.RotateVertical(inputs.planarRot, inputs.RawLook.y, deltaTime);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            // get speed modifier from current state
            //currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, TargetSpeedMultiplier, deltaTime * speedMultiplierLerpSpeed);


            if (Motor.GroundingStatus.IsStableOnGround) {
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(inputs.Move, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * inputs.Move.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * StateMaxSpeed;

                // Smooth movement Velocity
                //currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                LerpMovementSpeed(ref currentVelocity, targetMovementVelocity, inputs.RawMove, StateMaxSpeed, StateAccelUpSpeed, StateAccelDownSpeed);
                // have different lerp values for different speeds and directions
                this.currentVelocity = currentVelocity;
                Debug.Log("Grounded Movement");

                animator.SetBool("Jump", false);
            }
            // Air movement
            else {
                // Add move input
                if (inputs.Move.sqrMagnitude > 0f) {
                    Vector3 addedVelocity = inputs.Move * AirAccelerationSpeed * deltaTime;

                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < AirMaxSpeed) {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, AirMaxSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else {
                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f) {
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                        }
                    }

                    // Prevent air-climbing sloped walls
                    if (Motor.GroundingStatus.FoundAnyGround) {
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f) {
                            Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                        }
                    }

                    // Apply added velocity
                    currentVelocity += addedVelocity;
                }

                // Gravity
                currentVelocity += gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (AirDrag * deltaTime)));

                Debug.Log("Air Movement");
            }

            HandleJumping(ref currentVelocity, deltaTime, inputs.Move);

        }

        public void AfterCharacterUpdate(float deltaTime) {
            // Handle jump-related values
            {
                // Handle jumping pre-ground grace period (Bunny Hopping)
                if (JumpRequested && TimeSinceJumpRequested > jumpPreGroundingGraceTime) {
                    JumpRequested = false;
                }

                if (allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame) {
                        _jumpConsumed = false;
                    }
                    _timeSinceLastAbleToJump = 0f;
                }
                else {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }

        }

        public void PostGroundingUpdate(float deltaTime) {
        }

        

        public bool IsColliderValidForCollisions(Collider coll) {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider) {
        }

        #endregion
        public bool CanJump() {
            return IsGrounded() || _timeSinceLastAbleToJump <= jumpPostGroundingGraceTime;
        }

        public bool IsGrounded() {
            return (allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround);
        }

        public bool IsForceUngrounded() {
            return Motor.MustUnground();
        }

        void HandleJumping(ref Vector3 currentVelocity, float deltaTime, Vector3 moveInputVector) {
            // Handle jumping
            _jumpedThisFrame = false;
            TimeSinceJumpRequested += deltaTime;
            if (!JumpRequested) { return; }

            // See if we actually are allowed to jump
            if (!_jumpConsumed) { 
                // Calculate jump direction before ungrounding
                Vector3 jumpDirection = Motor.CharacterUp;
                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround) {
                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                }

                // Makes the character skip ground probing/snapping on its next update. 
                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                Motor.ForceUnground();

                // Add to the return velocity and reset jump state
                currentVelocity += (jumpDirection * jumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                currentVelocity += (moveInputVector * JumpForwardMultiplier);
                JumpRequested = false;
                _jumpConsumed = true;
                _jumpedThisFrame = true;


                animator.SetBool("Jump", true);
            }
        }

        void LerpMovementSpeed(ref Vector3 currentVelocity, Vector3 targetMovementVelocity, Vector2 rawMovementInput, float maxSpeed, float stateLerpUpSpeed, float stateLerpDownSpeed) {
            if (rawMovementInput.magnitude > 0 && currentVelocity.magnitude < targetMovementVelocity.magnitude) {
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, stateLerpUpSpeed * Time.deltaTime);
            } else {
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, stateLerpDownSpeed * Time.deltaTime);
            }
        }

    }

    public class PlayerMovementStateChangeEventArgs : EventArgs {
        public PlayerMovementState PreviousState { get; set; }
        public PlayerMovementState NewState { get; set; }
    }

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
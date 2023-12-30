using ECM.Common;
using ECM.Controllers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : BaseCharacterController, ICharacterMotor, ICharacterMovement
{
    public bool Grounded => movement.isGrounded;

    public bool WasGrounded => movement.isGrounded;
    public bool IsForceUngrounded => movement.IsForceUngrounded;

    public bool CanMultiJump => CanStartMultiJump();

    public Vector3 CharacterMovementSpeedWS => movement.velocity;

    public override void Awake() {
        base.Awake();
    }

    public override void Update() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        HandleInput();
        
        if (isPaused) return;

        //UpdateRotation();
        Animate();
    }

    public override void FixedUpdate() {
        Pause();
        if (isPaused) return;

        // Perform character movement
        Move();
    }


    public void ApplyForce(Vector3 force, ForceMode mode) {
        if (force.y > 0) { movement.DisableGrounding(); }
        movement.ApplyForce(force, mode);
    }

    public void ApplyImpulse(Vector3 force) {
        if (force.y > 0) { movement.DisableGrounding(); }
        movement.ApplyImpulse(force);
    }

    public void ApplyVerticalImpulse(float strength) {
        movement.DisableGrounding();
        movement.ApplyVerticalImpulse(strength);
    }

    protected override void Move() {
        var desiredVelocity = CalcDesiredVelocity();
        float tarSpeed = speed * 1 / encumberance;
        if (useRootMotion && applyRootMotion)
            movement.Move(desiredVelocity, tarSpeed, !allowVerticalMovement);
        else {
            // Move with acceleration and friction

            var currentFriction = isGrounded ? groundFriction : airFriction;
            var currentBrakingFriction = useBrakingFriction ? brakingFriction : currentFriction;

            movement.Move(desiredVelocity, tarSpeed, acceleration, deceleration, currentFriction,
                currentBrakingFriction, !allowVerticalMovement);
        }


        JumpUpdate();
        // Update root motion state,
        // should animator root motion be enabled? (eg: is grounded)

        applyRootMotion = useRootMotion && movement.isGrounded;
    }


    public bool CanUncrouch() {
        return isCrouching && movement.ClearanceCheck(standingHeight);
    }

    public bool CanJump() {
        // Is jump button pressed within pre jump tolerance time?
        if (_jumpButtonHeldDownTimer > jumpPreGroundedToleranceTime) {
            return false;
        }

        // If not grounded or no post grounded tolerance time remains, return
        if (!movement.isGrounded && _jumpUngroundedTimer > jumpPreGroundedToleranceTime) {
            return false;
        }

        return true;
    }
    bool CanStartMultiJump() {
        if (movement.isGrounded)
            return false;

        // Have mid-air jumps?
        if (_midAirJumpCount >= maxMidAirJumps)
            return false;

        return true;
    }

    public void MoveCommand(Vector2 move) {
        Vector3 _temp = new(move.x, 0, move.y);
        moveDirection = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, Vector3.up)) * _temp;
    }

    public void JumpCommand() {
        Jump();
    }
    public void DoubleJumpCommand() {
        MidAirJump();
    }

    void JumpUpdate() {
        if (isJumping) {
            // On landing, reset _isJumping flag

            if (!movement.wasGrounded && movement.isGrounded)
                _isJumping = false;
        }


        if (movement.isGrounded)
            _jumpUngroundedTimer = 0.0f;
        else
            _jumpUngroundedTimer += Time.deltaTime;
    }


    protected override void UpdateRotation() { }
    protected override void HandleInput() { }

    protected override void Jump() {

        if (!CanJump()) {
            Debug.Log("Jump validation failed");
            return;
        }

        _canJump = false;           // Halt jump until jump button is released
        _isJumping = true;          // Update isJumping flag
        _updateJumpTimer = true;    // Allow mid-air jump to be variable height

        // Prevent _jumpPostGroundedToleranceTime condition to pass until character become grounded again (_jumpUngroundedTimer reseted).

        _jumpUngroundedTimer = jumpPreGroundedToleranceTime;

        // Apply jump impulse

        movement.ApplyVerticalImpulse(jumpImpulse);

        // 'Pause' grounding, allowing character to safely leave the 'ground'
        movement.DisableGrounding();
    }

    protected override void MidAirJump() {
        // Reset mid-air jumps counter

        if (_midAirJumpCount > 0 && movement.isGrounded)
            _midAirJumpCount = 0;

        if (!CanStartMultiJump()) {
            Debug.Log("Multi jump validation failed");
            return;
        }

        _midAirJumpCount++;         // Increase mid-air jumps counter

        _canJump = false;           // Halt jump until jump button is released
        _isJumping = true;          // Update isJumping flag
        _updateJumpTimer = true;    // Allow mid-air jump to be variable height

        // Apply jump impulse

        movement.ApplyVerticalImpulse(jumpImpulse);

        // 'Pause' grounding, allowing character to safely leave the 'ground'

        movement.DisableGrounding();
    }

    protected override void Animate() { }
    protected override void Crouch() { }

    public void CrouchCommand() {
        if (isCrouching) return;

        movement.SetCapsuleHeight(crouchingHeight);
        isCrouching = true;
    }
    public void UnCrouchCommand() {
        if (!isCrouching) return;
         
        if (!CanUncrouch()) {
            Debug.Log("Uncrouch validation failed");
            return;
        }

        movement.SetCapsuleHeight(standingHeight);
        isCrouching = false;
    }
    public void SetSpeedProperties(float maxSpeed, float accelSpeed, float decelSpeed) {
        this.speed = maxSpeed;
        this.acceleration = accelSpeed;
        this.deceleration = decelSpeed;
    }

    [SerializeField][ReadOnly] float encumberance = 0;

    public void SetEncumbrance(float weight) {
        this.encumberance = 1 + weight;
    }
}



public interface ICharacterMotor
{
    public bool Grounded { get; }
    public bool WasGrounded { get; }
    public bool IsForceUngrounded { get; }
    public bool CanMultiJump { get; }
    public Vector3 CharacterMovementSpeedWS { get; }

    public void SetSpeedProperties(float maxSpeed, float accelSpeed, float decelSpeed);

    public void SetEncumbrance(float weight);

    public void MoveCommand(Vector2 move);
    public bool CanJump();

    public bool CanUncrouch();
    public void JumpCommand();
    public void CrouchCommand();
    public void UnCrouchCommand();
}

public interface ICharacterMovement
{
    public void ApplyForce(Vector3 force, ForceMode mode);
    public void ApplyImpulse(Vector3 force);
    public void ApplyVerticalImpulse(float strength);

}
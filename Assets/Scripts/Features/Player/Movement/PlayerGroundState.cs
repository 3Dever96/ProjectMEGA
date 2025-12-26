using System.Collections;
using UnityEngine;

// Represents the player's movement state while on the ground.
public class PlayerGroundState : PlayerMoveState
{
    // Indicates whether the player is running.
    public bool IsRunning
    {
        get;
        set;
    }

    float currentStepTime;

    bool isMoving; // Indicates whether the player is currently moving.
    bool isStepping;

    float inputX; // Horizontal input value.

    bool canJump; // Indicates whether the player can jump.

    // Called when the player enters the ground state.
    public override void StartState(PlayerController player)
    {
        canJump = false; // Initialize jump availability.
        isMoving = false; // Initialize movement state.
        player.VerticalSpeed = profile.StickForce; // Apply downward force to keep the player grounded.
        currentStepTime = profile.StepTime;
        if (player.LookDirection == Vector3.zero)
        {
            player.LookDirection = player.transform.forward;
        }
    }

    // Called every frame to update the player's behavior in the ground state.
    public override void UpdateState(PlayerController player)
    {
        // Get horizontal input from the InputManager.
        inputX = InputManager.instance.Move.x;

        // If there is horizontal input, handle movement and running state.
        if (inputX != 0f && !Physics.CheckSphere(player.transform.position + Vector3.right * 0.5f * Mathf.Sign(inputX) + Vector3.up, 0.1f, LayerMask.GetMask("Solid")))
        {
            isMoving = true;

            player.LookDirection = Vector3.right * Mathf.Sign(inputX);

            // If the player is not running, start the anticipation coroutine.
            if (!IsRunning)
            {
                if (!isStepping)
                {
                    player.Controller.Move(Vector3.right * Mathf.Sign(inputX) * profile.StepDistance);
                    isStepping = true;
                }
                currentStepTime -= Time.deltaTime;
                if (currentStepTime <= 0f)
                {
                    currentStepTime = 0f;
                    IsRunning = true;
                }
            }
            else
            {
                // Set the player's speed based on the input direction.
                player.CurrentSpeed = profile.MoveSpeed * Mathf.Sign(inputX);
            }
        }
        else
        {
            // If the player is not moving, reset the running state.
            if (!isMoving)
            {
                IsRunning = false;
                isStepping = false;
                currentStepTime = profile.StepTime;
            }

            isMoving = false;

            // Stop the player's movement.
            player.CurrentSpeed = 0f;
        }

        player.FaceDirection();

        // Handle jumping logic.
        if (InputManager.instance.Jump && canJump)
        {
            player.VerticalSpeed = profile.JumpSpeed; // Apply vertical speed for jumping.
        }

        // Reset jump availability when the jump button is released.
        if (!InputManager.instance.Jump && !canJump)
        {
            canJump = true;
        }

        // Move the player based on the current velocity.
        player.MovePlayer();
    }

    // Called to check and handle state transitions.
    public override void ChangeState(PlayerController player)
    {
        // Transition to the air state if the player is moving upward or not touching the ground.
        if (player.VerticalSpeed > 0f || !Physics.CheckSphere(transform.position, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")))
        {
            player.SetState(player.AirState);
        }
    }

    // Called when the player exits the ground state.
    public override void ExitState(PlayerController player)
    {
        
    }
}

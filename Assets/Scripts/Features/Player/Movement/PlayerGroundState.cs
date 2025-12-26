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

    // Serialized fields for configuring ground movement properties.
    [SerializeField] float moveSpeed; // Horizontal movement speed on the ground.
    [SerializeField] float stepDistance; // Distance covered during a step.
    [SerializeField] float stepTime; // Time taken for a step.
    [SerializeField] float stickForce; // Downward force applied to keep the player grounded.
    [SerializeField] float jumpSpeed; // Vertical speed applied when the player jumps.

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
        player.VerticalSpeed = stickForce; // Apply downward force to keep the player grounded.
        currentStepTime = stepTime;
    }

    // Called every frame to update the player's behavior in the ground state.
    public override void UpdateState(PlayerController player)
    {
        // Get horizontal input from the InputManager.
        inputX = InputManager.instance.Move.x;

        // If there is horizontal input, handle movement and running state.
        if (inputX != 0f && !Physics.CheckSphere(transform.position + Vector3.right * 0.5f * Mathf.Sign(inputX) + Vector3.up, 0.1f, LayerMask.GetMask("Solid")))
        {
            isMoving = true;

            // If the player is not running, start the anticipation coroutine.
            if (!IsRunning)
            {
                if (!isStepping)
                {
                    player.Controller.enabled = false;
                    transform.Translate(new Vector3(stepDistance * Mathf.Sign(inputX), 0f, 0f));
                    player.Controller.enabled = true;
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
                player.CurrentSpeed = moveSpeed * Mathf.Sign(inputX);
            }
        }
        else
        {
            // If the player is not moving, reset the running state.
            if (!isMoving)
            {
                IsRunning = false;
                isStepping = false;
                currentStepTime = stepTime;
            }

            isMoving = false;

            // Stop the player's movement.
            player.CurrentSpeed = 0f;
        }

        // Handle jumping logic.
        if (InputManager.instance.Jump && canJump)
        {
            player.VerticalSpeed = jumpSpeed; // Apply vertical speed for jumping.
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

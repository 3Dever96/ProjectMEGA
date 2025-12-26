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

    // Tracks the time remaining for the player to transition from stepping to running.
    float currentStepTime;

    // Flags to indicate the player's movement and stepping state.
    bool isMoving; // Indicates whether the player is currently moving.
    bool isStepping; // Indicates whether the player is currently stepping.

    // Horizontal input value.
    float inputX;

    // Flags to determine if the player can perform certain actions.
    bool canJump; // Indicates whether the player can jump.
    bool canAction; // Indicates whether the player can perform an action.

    // Called when the player enters the ground state.
    public override void StartState(PlayerController player)
    {
        // Initialize state-specific flags and variables.
        canJump = false; // The player cannot jump initially.
        canAction = false; // The player cannot perform actions initially.
        isMoving = false; // The player is not moving initially.
        player.VerticalSpeed = profile.StickForce; // Apply downward force to keep the player grounded.
        currentStepTime = profile.StepTime; // Set the initial step time.

        // If the player has no direction set, default to facing forward.
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
        if (inputX != 0f)
        {
            isMoving = true; // The player is moving.

            // Update the player's look direction based on the input.
            player.LookDirection = Vector3.right * Mathf.Sign(inputX);

            // If the player is not running, handle stepping logic.
            if (!IsRunning)
            {
                if (!isStepping)
                {
                    // Move the player a small step in the input direction.
                    player.Controller.Move(Vector3.right * Mathf.Sign(inputX) * profile.StepDistance);
                    isStepping = true; // Set stepping flag to true.
                }

                // Decrease the step time.
                currentStepTime -= Time.deltaTime;

                // If the step time has elapsed, transition to running state.
                if (currentStepTime <= 0f)
                {
                    currentStepTime = 0f;
                    IsRunning = true; // Set running flag to true.
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
            // If the player is not moving, reset the running and stepping states.
            if (!isMoving)
            {
                IsRunning = false; // Reset running flag.
                isStepping = false; // Reset stepping flag.
                currentStepTime = profile.StepTime; // Reset step time.
            }

            isMoving = false; // Set moving flag to false.

            // Stop the player's movement.
            player.CurrentSpeed = 0f;
        }

        // Make the player face the direction they are moving.
        player.FaceDirection();

        // Handle jumping logic.
        if (InputManager.instance.Jump && canJump)
        {
            // Apply vertical speed for jumping.
            player.VerticalSpeed = profile.JumpSpeed;
        }

        // Reset jump availability when the jump button is released.
        if (!InputManager.instance.Jump && !canJump)
        {
            canJump = true; // Allow the player to jump again.
        }

        // Move the player based on the current velocity.
        player.MovePlayer();
    }

    // Called to check and handle state transitions.
    public override void ChangeState(PlayerController player)
    {
        // Transition to the air state if the player is moving upward or not touching the ground.
        if (player.VerticalSpeed > 0f || !Physics.CheckSphere(player.transform.position, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")))
        {
            player.SetState(player.AirState);
        }

        // Transition to the action state if the action button is pressed and the player can perform an action.
        if (InputManager.instance.Action && canAction)
        {
            player.SetState(player.ActionState);
        }

        // Enable the ability to perform an action if the action button is released.
        if (!InputManager.instance.Action && !canAction)
        {
            canAction = true;
        }
    }

    // Called when the player exits the ground state.
    public override void ExitState(PlayerController player)
    {
        // Placeholder for any cleanup or reset logic when exiting the ground state.
    }
}

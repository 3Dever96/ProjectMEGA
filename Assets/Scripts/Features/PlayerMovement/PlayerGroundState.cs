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

    bool isMoving; // Indicates whether the player is currently moving.

    float inputX; // Horizontal input value.

    Coroutine step; // Reference to the step coroutine.

    bool canJump; // Indicates whether the player can jump.

    // Called when the player enters the ground state.
    public override void StartState(PlayerController player)
    {
        if (step != null)
        {
            StopCoroutine(step);
        }

        canJump = false; // Initialize jump availability.
        isMoving = false; // Initialize movement state.
        player.VerticalSpeed = stickForce; // Apply downward force to keep the player grounded.
    }

    // Called every frame to update the player's behavior in the ground state.
    public override void UpdateState(PlayerController player)
    {
        // Get horizontal input from the InputManager.
        inputX = InputManager.instance.Move.x;

        // If there is horizontal input, handle movement and running state.
        if (inputX != 0f)
        {
            isMoving = true;

            // If the player is not running, start the anticipation coroutine.
            if (!IsRunning)
            {
                if (step == null)
                {
                    step = StartCoroutine(Anticipation(player));
                    step = null;
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
        // Stop the step coroutine if it is running.
        if (step != null)
        {
            StopCoroutine(step);
            step = null;
        }
    }

    // Coroutine to handle the anticipation phase before the player starts running.
    IEnumerator Anticipation(PlayerController player)
    {
        player.Controller.enabled = false; // Disable the CharacterController temporarily.
        transform.Translate(new Vector3(inputX * stepDistance, 0f, 0f)); // Move the player slightly forward.
        player.Controller.enabled = true; // Re-enable the CharacterController.

        yield return new WaitForSeconds(stepTime); // Wait for the step time.

        IsRunning = true; // Set running state to true after anticipation.
        step = null; // Reset the step coroutine reference.
    }
}

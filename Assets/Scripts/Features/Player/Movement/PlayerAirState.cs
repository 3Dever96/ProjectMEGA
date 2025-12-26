using UnityEngine;

// Represents the player's movement state while in the air.
public class PlayerAirState : PlayerMoveState
{
    // Horizontal input value.
    float inputX;

    // Factor to reduce the player's speed while in the air.
    [SerializeField] float dragFactor = 0.955f;

    // Flags to determine if the player can perform certain actions.
    bool canAction;
    bool canJump;
    bool doubleJumpPerformed;

    // The movement speed of the player in the air.
    float moveSpeed;

    // Called when the player enters the air state.
    public override void StartState(PlayerController player)
    {
        // If the player has no direction set, default to facing forward.
        if (player.LookDirection == Vector3.zero)
        {
            player.LookDirection = player.transform.forward;
        }

        // Initialize state-specific flags.
        canAction = false;
        canJump = false;
        doubleJumpPerformed = false;
    }

    // Called every frame to update the player's behavior in the air state.
    public override void UpdateState(PlayerController player)
    {
        // Determine the movement speed based on whether the player is dashing or not.
        if (player.ActionState.shareVelocity)
        {
            if (player.ActionState.IsDashing)
            {
                moveSpeed = profile.ActionSpeed;
            }
            else
            {
                moveSpeed = profile.MoveSpeed;
            }
        }
        else
        {
            moveSpeed = profile.MoveSpeed;
        }

        // Get horizontal input from the InputManager.
        inputX = InputManager.instance.Move.x;

        // If there is horizontal input, update the player's speed and direction.
        if (inputX != 0f)
        {
            player.LookDirection = Vector3.right * Mathf.Sign(inputX);

            // Apply drag factor to the movement speed.
            player.CurrentSpeed = moveSpeed * dragFactor * Mathf.Sign(inputX);

            // Update the running state in the ground state.
            player.GroundState.IsRunning = true;
        }
        else
        {
            // Stop the player's horizontal movement if no input is detected.
            player.CurrentSpeed = 0f;

            // Reset running state if no input.
            player.GroundState.IsRunning = false;
        }

        // Make the player face the direction they are moving.
        player.FaceDirection();

        // Handle double jump logic if the player has the ability.
        if (profile.HasDoubleJump)
        {
            if (!doubleJumpPerformed)
            {
                if (InputManager.instance.Jump && canJump)
                {
                    // Perform a double jump if the player hasn't already done so.
                    player.VerticalSpeed = profile.JumpSpeed;
                    canJump = false;
                    doubleJumpPerformed = true;

                    // Disable dashing after performing a double jump.
                    player.ActionState.IsDashing = false;
                }

                if (!InputManager.instance.Jump && !canJump)
                {
                    // Allow the player to jump again if the jump button is released.
                    canJump = true;
                }
            }
        }

        // Prevent upward movement if the player is near a ceiling or not jumping.
        if (Physics.CheckSphere(player.transform.position + Vector3.up * player.Controller.height, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")) || !InputManager.instance.Jump)
        {
            player.VerticalSpeed = Mathf.Min(0, player.VerticalSpeed);
        }

        // Apply gravity to the player if they are falling.
        if (player.VerticalSpeed > profile.TerminalVelocity)
        {
            player.VerticalSpeed += profile.Gravity * Time.deltaTime;
        }
        else
        {
            // Limit the player's falling speed to the terminal velocity.
            player.VerticalSpeed = profile.TerminalVelocity;
        }

        // Move the player based on the current velocity.
        player.MovePlayer();
    }

    // Called to check and handle state transitions.
    public override void ChangeState(PlayerController player)
    {
        // Transition to the ground state if the player is falling and touching the ground.
        if (player.VerticalSpeed <= 0f && Physics.CheckSphere(player.transform.position, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")))
        {
            player.SetState(player.GroundState);
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

    // Called when the player exits the air state.
    public override void ExitState(PlayerController player)
    {
        // Reset the dashing flag when exiting the air state.
        player.ActionState.IsDashing = false;
    }
}

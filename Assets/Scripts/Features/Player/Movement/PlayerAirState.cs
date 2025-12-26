using UnityEngine;

// Represents the player's movement state while in the air.
public class PlayerAirState : PlayerMoveState
{
    float inputX; // Horizontal input value.
    [SerializeField] float dragFactor = 0.955f;

    bool canJump;
    bool doubleJumpPerformed;

    // Called when the player enters the air state.
    public override void StartState(PlayerController player)
    {
        // Initialization logic for the air state can be added here.
        if (player.LookDirection == Vector3.zero)
        {
            player.LookDirection = player.transform.forward;
        }
        canJump = false;
        doubleJumpPerformed = false;
    }

    // Called every frame to update the player's behavior in the air state.
    public override void UpdateState(PlayerController player)
    {
        // Get horizontal input from the InputManager.
        inputX = InputManager.instance.Move.x;

        // If there is horizontal input, set the player's speed and running state.
        if (inputX != 0f)
        {
            player.LookDirection = Vector3.right * Mathf.Sign(inputX);

            player.CurrentSpeed = profile.MoveSpeed * dragFactor * Mathf.Sign(inputX);
            player.GroundState.IsRunning = true; // Update the running state in the ground state.
        }
        else
        {
            player.CurrentSpeed = 0f;
            player.GroundState.IsRunning = false; // Reset running state if no input.
        }

        player.FaceDirection();

        if (profile.HasDoubleJump)
        {
            if (!doubleJumpPerformed)
            {
                if (InputManager.instance.Jump && canJump)
                {
                    player.VerticalSpeed = profile.JumpSpeed;
                    canJump = false;
                    doubleJumpPerformed = true;
                }

                if (!InputManager.instance.Jump && !canJump)
                {
                    canJump = true;
                }
            }
        }

        // Check if the player is near a ceiling or if the jump button is not pressed.
        if (Physics.CheckSphere(player.transform.position + Vector3.up * player.Controller.height, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")) || !InputManager.instance.Jump)
        {
            // Prevent upward movement if the player is near a ceiling or not jumping.
            player.VerticalSpeed = Mathf.Min(0, player.VerticalSpeed);
        }

        // Apply gravity if the player is falling.
        if (player.VerticalSpeed > profile.TerminalVelocity)
        {
            player.VerticalSpeed += profile.Gravity * Time.deltaTime;
        }
        else
        {
            player.VerticalSpeed = profile.TerminalVelocity;
        }

        // Move the player based on the current velocity.
        player.MovePlayer();
    }

    // Called to check and handle state transitions.
    public override void ChangeState(PlayerController player)
    {
        // Transition to the ground state if the player is falling and touching the ground.
        if (player.VerticalSpeed <= 0f && Physics.CheckSphere(transform.position, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")))
        {
            player.SetState(player.GroundState);
        }
    }

    // Called when the player exits the air state.
    public override void ExitState(PlayerController player)
    {
        // Cleanup logic for exiting the air state can be added here.
    }
}

using UnityEngine;

// This class represents a specific state of the player where they perform an action, such as dashing.
public class PlayerActionState : PlayerMoveState
{
    // Property to determine if the player is currently dashing.
    public bool IsDashing { get; set; }

    // Duration of the action state.
    [SerializeField] protected float actionTime = 0.433f;

    // Tracks the remaining time for the current action.
    protected float currentActionTime;

    // Determines whether the player's velocity is shared between states.
    public bool shareVelocity;

    // Stores the horizontal input value.
    protected float inputX;

    // Flag to check if the player can jump.
    protected bool canJump;

    // Called when the state starts. Initializes the player's speed and action-related variables.
    public override void StartState(PlayerController player)
    {
        // Set the player's current speed based on the action speed and their forward direction.
        player.CurrentSpeed = profile.ActionSpeed * Mathf.Sign(player.transform.forward.x);

        // Initialize the action time and set the dashing flag to true.
        currentActionTime = actionTime;
        IsDashing = true;

        // Disable jumping at the start of the action state.
        canJump = false;
    }

    // Called every frame to update the player's state.
    public override void UpdateState(PlayerController player)
    {
        // Get the horizontal movement input from the InputManager.
        inputX = InputManager.instance.Move.x;

        // If there is horizontal input, update the player's speed and direction.
        if (inputX != 0)
        {
            player.CurrentSpeed = profile.ActionSpeed * Mathf.Sign(inputX);
            player.LookDirection = Vector3.right * Mathf.Sign(inputX);
        }

        // Make the player face the direction they are moving.
        player.FaceDirection();

        // Handle jumping logic.
        if (InputManager.instance.Jump && canJump)
        {
            // If the jump input is pressed and the player can jump, set the vertical speed to the jump speed.
            player.VerticalSpeed = profile.JumpSpeed;
        }

        if (!InputManager.instance.Jump && !canJump)
        {
            // If the jump input is not pressed and the player cannot jump, enable jumping.
            canJump = true;
        }

        // Move the player based on the current speed and direction.
        player.MovePlayer();
    }

    // Called to handle state transitions. Decreases the remaining action time.
    public override void ChangeState(PlayerController player)
    {
        currentActionTime -= Time.deltaTime;
    }

    // Called when the state ends. Currently, no specific logic is implemented here.
    public override void ExitState(PlayerController player)
    {
        // Placeholder for any cleanup or reset logic when exiting the state.
    }
}

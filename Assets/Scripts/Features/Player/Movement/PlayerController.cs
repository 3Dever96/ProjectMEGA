using UnityEngine;

// This script manages the player's movement and state transitions.
// It requires a CharacterController component to function properly.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Reference to the CharacterController component attached to the player.
    public CharacterController Controller
    {
        get;
        private set;
    }

    // The current movement state of the player (e.g., GroundState, AirState).
    public PlayerMoveState CurrentState
    {
        get;
        private set;
    }

    // Reference to the player's ground movement state.
    public PlayerGroundState GroundState
    {
        get;
        private set;
    }

    // Reference to the player's air movement state.
    public PlayerAirState AirState
    {
        get;
        private set;
    }

    // Reference to the player's action state (e.g., dashing or special actions).
    public PlayerActionState ActionState
    {
        get;
        private set;
    }

    // The current horizontal movement speed of the player.
    public float CurrentSpeed
    {
        get;
        set;
    }

    // The current vertical movement speed of the player (e.g., for jumping or falling).
    public float VerticalSpeed
    {
        get;
        set;
    }

    // The current velocity of the player, combining horizontal and vertical speeds.
    public Vector3 Velocity
    {
        get;
        set;
    }

    // The direction the player is facing.
    public Vector3 LookDirection
    {
        get;
        set;
    }

    // Called when the script is first initialized.
    void Awake()
    {
        // Get the CharacterController component attached to the player.
        Controller = GetComponent<CharacterController>();
    }

    // Called every fixed frame-rate frame. Used for physics updates.
    void FixedUpdate()
    {
        // If there is a current state, update and check for state transitions.
        if (CurrentState != null)
        {
            CurrentState.UpdateState(this); // Update the current state logic.
            CurrentState.ChangeState(this); // Check if the state needs to change.
        }
    }

    // Sets the player's current movement state to the specified state.
    public void SetState(PlayerMoveState newState)
    {
        // Exit the current state if it exists.
        if (CurrentState != null)
        {
            CurrentState.ExitState(this);
        }

        // Update the current state to the new state.
        CurrentState = newState;

        // Start the new state if it exists.
        if (CurrentState != null)
        {
            CurrentState.StartState(this);
        }
    }

    // Moves the player based on the current speed and vertical speed.
    public void MovePlayer()
    {
        // Calculate the player's velocity based on horizontal and vertical speeds.
        Vector3 velocity = CurrentSpeed * Vector3.right;
        velocity.y = VerticalSpeed;

        // Store the calculated velocity.
        Velocity = velocity;

        // Move the player using the CharacterController component.
        Controller.Move(velocity * Time.deltaTime);
    }

    // Rotates the player to face the direction they are moving.
    public void FaceDirection()
    {
        // Determine the direction to face based on the player's current forward direction and look direction.
        Vector3 dir = Vector3.Angle(transform.forward, LookDirection) >= 135f ? -Vector3.forward : LookDirection;

        // Smoothly rotate the player towards the target direction.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 500f * Time.deltaTime);
    }

    // Initializes the player's movement states and sets the initial state to GroundState.
    public void GetCharacterStates(PlayerGroundState ground, PlayerAirState air, PlayerActionState action)
    {
        // Assign the provided state references to the corresponding properties.
        GroundState = ground;
        AirState = air;
        ActionState = action;

        // Set the player's initial state to the ground state.
        SetState(GroundState);
    }
}

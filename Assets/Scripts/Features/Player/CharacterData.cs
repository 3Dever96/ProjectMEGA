using UnityEngine;

// This script defines the character's data and properties, such as size, movement speeds, and abilities.
// It also initializes the player's states and sets up the CharacterController dimensions.
public class CharacterData : MonoBehaviour
{
    // Public properties to access private serialized fields.
    public float Height
    {
        get { return height; } // Returns the height of the character.
    }

    public float Radius
    {
        get { return radius; } // Returns the radius of the character's collider.
    }

    public Vector3 Center
    {
        get { return center; } // Returns the center offset of the character's collider.
    }

    public float StepDistance
    {
        get { return stepDistance; } // Returns the distance the player moves during a step.
    }

    public float StepTime
    {
        get { return stepTime; } // Returns the time it takes for the player to transition from stepping to running.
    }

    public float MoveSpeed
    {
        get { return moveSpeed; } // Returns the player's movement speed.
    }

    public float ActionSpeed
    {
        get { return actionSpeed; } // Returns the player's speed during an action state (e.g., dashing).
    }

    public float JumpSpeed
    {
        get { return jumpSpeed; } // Returns the player's vertical speed during a jump.
    }

    public float Gravity
    {
        get { return gravity; } // Returns the gravity force applied to the player.
    }

    public float StickForce
    {
        get { return stickForce; } // Returns the downward force applied to keep the player grounded.
    }

    public float TerminalVelocity
    {
        get { return terminalVelocity; } // Returns the maximum falling speed of the player.
    }

    public float WallSlideSpeed
    {
        get { return wallSlideSpeed; } // Returns the speed of the player during a wall slide.
    }

    public bool HasDoubleJump
    {
        get { return hasDoubleJump; } // Indicates whether the player can perform a double jump.
    }

    public bool HasAirDash
    {
        get { return hasAirDash; } // Indicates whether the player can perform an air dash.
    }

    public bool HasHover
    {
        get { return hasHover; } // Indicates whether the player can hover in the air.
    }

    // Reference to the PlayerController component.
    PlayerController player;

    // Serialized fields for character properties.
    [Header("Character Size")]
    [SerializeField] float height; // Height of the character's collider.
    [SerializeField] float radius; // Radius of the character's collider.
    [SerializeField] Vector3 center; // Center offset of the character's collider.

    [Header("Step")]
    [SerializeField] float stepDistance = 0.0625f; // Distance the player moves during a step.
    [SerializeField] float stepTime = 0.11f; // Time it takes for the player to transition from stepping to running.

    [Header("Movement Speed")]
    [SerializeField] float moveSpeed = 5.15f; // Default movement speed of the player.
    [SerializeField] float actionSpeed = 9.375f; // Speed during action states (e.g., dashing).
    [SerializeField] float jumpSpeed = 18.75f; // Vertical speed during a jump.
    [SerializeField] float gravity = -54f; // Gravity force applied to the player.
    [SerializeField] float stickForce = -5f; // Downward force to keep the player grounded.
    [SerializeField] float terminalVelocity = -26.25f; // Maximum falling speed.
    [SerializeField] float wallSlideSpeed = -2f; // Speed during a wall slide.

    [Header("Action Bools")]
    [SerializeField] bool hasDoubleJump; // Flag to enable double jump ability.
    [SerializeField] bool hasAirDash; // Flag to enable air dash ability.
    [SerializeField] bool hasHover; // Flag to enable hover ability.

    // References to the player's movement states.
    PlayerGroundState groundState;
    PlayerAirState airState;
    PlayerActionState actionState;

    // Called when the script is first initialized.
    void Start()
    {
        // Get the PlayerController component from the parent object.
        player = GetComponentInParent<PlayerController>();

        // Retrieve the movement state components attached to the player.
        groundState = GetComponent<PlayerGroundState>();
        airState = GetComponent<PlayerAirState>();
        actionState = GetComponent<PlayerActionState>();

        // Initialize the player's CharacterController dimensions based on the character's size properties.
        player.Controller.height = height; // Set the height of the collider.
        player.Controller.radius = radius; // Set the radius of the collider.
        player.Controller.center = center; // Set the center offset of the collider.

        // Pass the movement states to the PlayerController for state management.
        player.GetCharacterStates(groundState, airState, actionState);
    }
}

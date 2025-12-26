using UnityEngine;

// Abstract base class for player movement states.
// This class serves as a blueprint for specific player movement states (e.g., GroundState, AirState, ActionState).
public abstract class PlayerMoveState : MonoBehaviour
{
    // Reference to the player's character data, which contains movement-related properties.
    protected CharacterData profile;

    // Called when the script is first initialized.
    protected virtual void Start()
    {
        // Retrieve the CharacterData component attached to the player.
        profile = GetComponent<CharacterData>();
    }

    // Abstract method to define behavior when the player enters this state.
    // Must be implemented by derived classes.
    public abstract void StartState(PlayerController player);

    // Abstract method to define behavior that updates every frame in this state.
    // Must be implemented by derived classes.
    public abstract void UpdateState(PlayerController player);

    // Abstract method to define logic for transitioning between states.
    // Must be implemented by derived classes.
    public abstract void ChangeState(PlayerController player);

    // Abstract method to define cleanup or reset logic when exiting this state.
    // Must be implemented by derived classes.
    public abstract void ExitState(PlayerController player);
}

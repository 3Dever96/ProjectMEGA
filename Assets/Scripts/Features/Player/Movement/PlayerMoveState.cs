using UnityEngine;

// Abstract base class for player movement states.
public abstract class PlayerMoveState : MonoBehaviour
{
    // Called when the player enters this state.
    public abstract void StartState(PlayerController player);

    // Called every frame to update the player's behavior in this state.
    public abstract void UpdateState(PlayerController player);

    // Called to check and handle state transitions.
    public abstract void ChangeState(PlayerController player);

    // Called when the player exits this state.
    public abstract void ExitState(PlayerController player);
}

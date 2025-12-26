using UnityEngine;

// Represents the player's dash state, which is a specialized action state.
public class PlayerDashState : PlayerActionState
{
    // Duration of the dash when performed in the air.
    [SerializeField] float airDashTime = 0.333f;

    // Flag to determine if the dash started while the player was in the air.
    bool startedInAir;

    // Called when the dash state starts.
    public override void StartState(PlayerController player)
    {
        // Call the base class's StartState method to initialize common action state properties.
        base.StartState(player);

        // Initialize the flag to indicate the dash did not start in the air.
        startedInAir = false;

        // Check if the player has the ability to perform an air dash.
        if (profile.HasAirDash)
        {
            // Check if the player is currently in the air (not touching the ground).
            if (!Physics.CheckSphere(player.transform.position, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")))
            {
                // If the player is in the air, set the flag and adjust the vertical speed and action time.
                startedInAir = true;
                player.VerticalSpeed = 0f; // Reset vertical speed for air dash.
                currentActionTime = airDashTime; // Set the duration of the air dash.
            }
            else
            {
                // If the player is on the ground, set the flag to false.
                startedInAir = false;
            }
        }
    }

    // Called every frame to update the player's behavior during the dash state.
    public override void UpdateState(PlayerController player)
    {
        // Call the base class's UpdateState method to handle common action state updates.
        base.UpdateState(player);
    }

    // Called to handle state transitions during the dash state.
    public override void ChangeState(PlayerController player)
    {
        // Call the base class's ChangeState method to handle common action state transitions.
        base.ChangeState(player);

        // Check if the player is falling and touching the ground.
        if (player.VerticalSpeed < 0f && Physics.CheckSphere(player.transform.position, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")))
        {
            // Transition to the ground state if the dash time has elapsed or the action button is released.
            if (currentActionTime <= 0f || !InputManager.instance.Action)
            {
                player.SetState(player.GroundState);
                IsDashing = false; // End the dash.
            }
        }
        else
        {
            // Handle transitions when the dash started in the air.
            if (startedInAir)
            {
                // Transition to the air state if the dash time has elapsed or the action button is released.
                if (currentActionTime <= 0f || !InputManager.instance.Action)
                {
                    player.SetState(player.AirState);
                    IsDashing = false; // End the dash.
                }
            }
            else
            {
                // If the dash did not start in the air, transition to the air state.
                player.SetState(player.AirState);
            }
        }
    }

    // Called when the dash state ends.
    public override void ExitState(PlayerController player)
    {
        // Call the base class's ExitState method to handle common action state cleanup.
        base.ExitState(player);
    }
}

using UnityEngine;

// Represents the player's sliding state, which is a specialized action state.
public class PlayerSlideState : PlayerActionState
{
    // Called when the slide state starts.
    public override void StartState(PlayerController player)
    {
        // Call the base class's StartState method to initialize common action state properties.
        base.StartState(player);

        // Adjust the player's CharacterController dimensions for the sliding state.
        player.Controller.height = profile.Height * 0.5f; // Reduce the height to half for sliding.
        player.Controller.radius = profile.Radius; // Set the radius to the profile's radius.
        player.Controller.center = profile.Center * 0.5f; // Adjust the center to match the reduced height.
    }

    // Called every frame to update the player's behavior during the slide state.
    public override void UpdateState(PlayerController player)
    {
        // Call the base class's UpdateState method to handle common action state updates.
        base.UpdateState(player);
    }

    // Called to handle state transitions during the slide state.
    public override void ChangeState(PlayerController player)
    {
        // Call the base class's ChangeState method to handle common action state transitions.
        base.ChangeState(player);

        // Check if there is enough space above the player to stop sliding.
        if (!Physics.CheckSphere(transform.position + Vector3.up * profile.Height * 0.45f, 0.5f, LayerMask.GetMask("Solid")))
        {
            // If the slide duration has elapsed, transition to the ground state.
            if (currentActionTime <= 0f)
            {
                player.SetState(player.GroundState); // Switch to the ground state.
                IsDashing = false; // End the dashing state.
            }

            // If the player is no longer touching the ground, transition to the air state.
            if (!Physics.CheckSphere(transform.position, player.Controller.radius - 0.1f, LayerMask.GetMask("Solid")))
            {
                player.SetState(player.AirState); // Switch to the air state.
                IsDashing = false; // End the dashing state.
            }
        }
    }

    // Called when the slide state ends.
    public override void ExitState(PlayerController player)
    {
        // Call the base class's ExitState method to handle common action state cleanup.
        base.ExitState(player);

        // Reset the player's CharacterController dimensions to their original values.
        player.Controller.height = profile.Height; // Restore the original height.
        player.Controller.radius = profile.Radius; // Restore the original radius.
        player.Controller.center = profile.Center; // Restore the original center.
    }
}

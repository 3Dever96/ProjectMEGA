using UnityEngine;
using UnityEngine.InputSystem;

// This script manages player input using the Unity Input System.
// It provides properties to access movement and jump inputs and ensures a singleton instance of the InputManager.

[RequireComponent(typeof(PlayerInput))] // Ensures that the GameObject has a PlayerInput component attached.
public class InputManager : MonoBehaviour
{
    // Public properties to access input values.
    public Vector2 Move
    {
        get
        {
            return move; // Returns the current movement vector.
        }
    }

    public bool Jump
    {
        get
        {
            return jump; // Returns the current jump state (true if jumping, false otherwise).
        }
    }

    // Singleton instance of the InputManager.
    public static InputManager instance;

    // Reference to the PlayerInput component.
    private PlayerInput input;

    // Variables to store input values.
    private Vector2 move; // Stores the movement vector.
    private bool jump; // Stores the jump state.

    private void Awake()
    {
        // Ensure that only one instance of InputManager exists.
        if (instance == null)
        {
            instance = this; // Set this instance as the singleton instance.
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject); // Destroy duplicate instances.
            }
        }

        // Get the PlayerInput component attached to this GameObject.
        input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Subscribe to the onActionTriggered event to handle input actions.
        input.onActionTriggered += OnAction;
    }

    private void OnDisable()
    {
        // Unsubscribe from the onActionTriggered event when the object is disabled.
        input.onActionTriggered -= OnAction;
    }

    private void OnAction(InputAction.CallbackContext context)
    {
        // Handle input actions based on their names.
        switch (context.action.name)
        {
            case "Move":
                // Read the movement vector from the input action.
                move = context.ReadValue<Vector2>();
                break;
            case "Jump":
                // Update the jump state based on the input action.
                SetBool(context, ref jump);
                break;
        }
    }

    private void SetBool(InputAction.CallbackContext context, ref bool value)
    {
        // Update the boolean value based on the input action's phase.
        if (context.performed)
        {
            value = true; // Set to true when the action is performed.
        }

        if (context.canceled)
        {
            value = false; // Set to false when the action is canceled.
        }
    }
}

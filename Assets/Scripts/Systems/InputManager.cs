using UnityEngine;
using UnityEngine.InputSystem;

// This script manages player input using the Unity Input System.
// It provides properties to access movement, jump, and action inputs.
// It also ensures a singleton instance of the InputManager for global access.

[RequireComponent(typeof(PlayerInput))] // Ensures that the GameObject has a PlayerInput component attached.
public class InputManager : MonoBehaviour
{
    // Public properties to access input values.
    public Vector2 Move
    {
        get
        {
            return move; // Returns the current movement vector (e.g., for horizontal and vertical movement).
        }
    }

    public bool Jump
    {
        get
        {
            return jump; // Returns the current jump state (true if the jump button is pressed, false otherwise).
        }
    }

    public bool Action
    {
        get
        {
            return action; // Returns the current action state (true if the action button is pressed, false otherwise).
        }
    }

    // Singleton instance of the InputManager for global access.
    public static InputManager instance;

    // Reference to the PlayerInput component, which handles input actions.
    PlayerInput input;

    // Variables to store input values.
    Vector2 move; // Stores the movement vector (x and y values for horizontal and vertical movement).
    bool jump; // Stores the jump state (true when jumping, false otherwise).
    bool action; // Stores the action state (true when performing an action, false otherwise).

    // Called when the script is first initialized.
    void Awake()
    {
        // Ensure that only one instance of InputManager exists in the scene.
        if (instance == null)
        {
            instance = this; // Set this instance as the singleton instance.
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject); // Destroy duplicate instances to maintain singleton pattern.
            }
        }

        // Get the PlayerInput component attached to this GameObject.
        input = GetComponent<PlayerInput>();
    }

    // Called when the GameObject is enabled.
    void OnEnable()
    {
        // Subscribe to the onActionTriggered event to handle input actions.
        input.onActionTriggered += OnAction;
    }

    // Called when the GameObject is disabled.
    void OnDisable()
    {
        // Unsubscribe from the onActionTriggered event to prevent memory leaks.
        input.onActionTriggered -= OnAction;
    }

    // Callback method to handle input actions triggered by the PlayerInput component.
    void OnAction(InputAction.CallbackContext context)
    {
        // Handle input actions based on their names.
        switch (context.action.name)
        {
            case "Move":
                // Read the movement vector from the input action and store it.
                move = context.ReadValue<Vector2>();
                break;
            case "Jump":
                // Update the jump state based on the input action.
                SetBool(context, ref jump);
                break;
            case "Action":
                // Update the action state based on the input action.
                SetBool(context, ref action);
                break;
        }
    }

    // Helper method to update boolean values based on the input action's phase.
    void SetBool(InputAction.CallbackContext context, ref bool value)
    {
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

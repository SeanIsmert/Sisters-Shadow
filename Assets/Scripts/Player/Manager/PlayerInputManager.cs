using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Input manager that subscribes and unsubscribes to certain actions.
/// Built in functionality allowing players to swap action maps on StateChange.
/// Written by: Sean
/// Modified by:
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public static PlayerInput input;

    private PlayerMovementHandler _movementHandler;
    private InteractableManager _interactableManager;

    void Awake()
    {
        input = new PlayerInput();
        _movementHandler = GetComponent<PlayerMovementHandler>();
        _interactableManager = GetComponent<InteractableManager>();

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Switches the current action map based on the game state.
    /// </summary>
    private void ActionMaps(GameState state)
    {
        if (state == GameState.Gameplay)
        {
            input.Gameplay.Enable();
            input.UI.Disable();
        }
        else
        {
            input.Gameplay.Disable();
            input.UI.Enable();
        }
    }

    private void OnEnable()
    {
        input.Enable();
        GameManager.OnGameStateChanged += ActionMaps;
        input.Gameplay.Sprint.performed += ctx => _movementHandler.SetMovementState(MoveStates.Sprinting);                                           // Input for sprinting.
        input.Gameplay.Sprint.canceled += ctx => _movementHandler.SetMovementState(MoveStates.Idle);
        input.Gameplay.Aim.performed += ctx => _movementHandler.SetMovementState(MoveStates.Aiming);                                                 // Input for aiming.
        input.Gameplay.Aim.canceled += ctx => _movementHandler.SetMovementState(MoveStates.Idle);
        input.Gameplay.Interact.started += ctx => _interactableManager.HandleInteraction();                                  // Input for interaction.
    }

    private void OnDisable()
    {
        input.Disable();
        GameManager.OnGameStateChanged -= ActionMaps;
        input.Gameplay.Sprint.performed -= ctx => _movementHandler.SetMovementState(MoveStates.Sprinting);                                           // Input for sprinting.
        input.Gameplay.Sprint.canceled -= ctx => _movementHandler.SetMovementState(MoveStates.Idle);
        input.Gameplay.Aim.performed -= ctx => _movementHandler.SetMovementState(MoveStates.Aiming);                                                 // Input for aiming.
        input.Gameplay.Aim.canceled -= ctx => _movementHandler.SetMovementState(MoveStates.Idle);
        input.Gameplay.Interact.started -= ctx => _interactableManager.HandleInteraction();                                  // Input for interaction.
    }
}

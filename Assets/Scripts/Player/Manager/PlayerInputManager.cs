using UnityEngine;

/// <summary>
/// Input manager that subscribes and unsubscribes to certain actions.
/// Built in functionality allowing players to swap action maps on StateChange.
/// Written by: Sean
/// Modified by:
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    public PlayerInput input;

    void Awake()
    {
        input = new PlayerInput();
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
    }

    private void OnDisable()
    {
        input.Disable();
        GameManager.OnGameStateChanged -= ActionMaps;
    }
}

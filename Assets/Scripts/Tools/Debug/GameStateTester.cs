using UnityEngine.InputSystem;
using UnityEngine;

public class GameStateTester : MonoBehaviour
{
    //DO NOT LEAVE IN BUILDS
    private void Update()
    {
        // Check for key presses to change game state
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            GameManager.Instance.UpdateGameState(GameState.Menu);
        }
        else if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            GameManager.Instance.UpdateGameState(GameState.Gameplay);
        }
    }
}
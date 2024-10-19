using UnityEngine;
using System;

/// <summary>
/// Game Manager class that controls the state of the game.
/// Made a singletom and able to be called elsewhere.
/// Utilizes an action in order to let other scripts know of state changes.
/// Simple easy and feed zero info about other things.
/// Written by: Sean
/// Modified by:
/// </summary>
public class GameManager : MonoSinglton<GameManager>
{
    #region Variables
    //Events
    public static event Action<GameState> OnGameStateChanged;
    //Enum
    public GameState state;
    #endregion

    private void Start()
    {
        //change to menu later
        UpdateGameState(GameState.Gameplay);
    }

    #region CodeBase
    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (state)
        {
            case GameState.Gameplay:
                break;
            case GameState.Pause: //UI
                break;
            case GameState.Animation: 
                break;
            case GameState.InventoryPlayer: //UI
                break;
            case GameState.InventoryGlobal //UI
            : break;
            case GameState.Interactable: //UI
                break;
            case GameState.Dialogue: // UI
                break;
            case GameState.Menu: //UI
                break;
            case GameState.Lose: //UI
                break;
            case GameState.Win: //UI
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }
    #endregion
}

#region States
public enum GameState
{
    Menu, Gameplay, Pause, Animation, InventoryPlayer, InventoryGlobal, Interactable, Dialogue, Lose, Win
}
#endregion
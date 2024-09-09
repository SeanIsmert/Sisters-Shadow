using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    #region Variables
    //Events
    public static event Action<GameState> OnGameStateChanged;
    //Singleton
    public static GameManager instance;
    //Enum
    public GameState state;
    #endregion

    #region Initialize
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //change to menu later
        UpdateGameState(GameState.Gameplay);
    }
    #endregion

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
            case GameState.Inventory: //UI
                break;
            case GameState.Interactable: //UI
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
    Menu, Gameplay, Pause, Animation, Inventory, Interactable, Lose, Win
}
#endregion
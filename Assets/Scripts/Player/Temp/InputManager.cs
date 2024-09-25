using UnityEngine;

public class InputManager : MonoBehaviour
{
    //private InteractableHandler _interactableHandler;
    private PlayerMovementHandler2 _playerMovement;
    private PlayerShootingHandler _playerShooting;

    public static InputManager instance;
    public PlayerInput input;

    private void Awake()
    {
        //_interactableHandler = GetComponent<InteractableHandler>();
        _playerMovement = GetComponent<PlayerMovementHandler2>();
        _playerShooting = GetComponent<PlayerShootingHandler>();
        input = new PlayerInput();

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (GameManager.instance.state == GameState.Gameplay)
        {
            //_playerMovement?.UpdateMovement(input.Gameplay.Movement.ReadValue<Vector2>());
        }
    }

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
        //input.Gameplay.Interact.started += ctx => _interactableHandler.HandleInteraction();
        input.Gameplay.Sprint.started += ctx => _playerMovement.Sprint();
        input.Gameplay.Sprint.canceled += ctx => _playerMovement.ReleaseSprint();
        input.Gameplay.Aim.started += ctx => _playerShooting.Aim();
        input.Gameplay.Aim.canceled += ctx => _playerShooting.AimRelease();
        //input.Gameplay.Shooting.started += ctx => _playerShooting.Shoot(-1);
        //input.Gameplay.Pause.started += ctx => GameManager.instance.UpdateGameState(GameState.Pause);
    }

    private void OnDisable()
    {
        input.Disable();
        GameManager.OnGameStateChanged -= ActionMaps;
        //input.Gameplay.Interact.started -= ctx => _interactableHandler.HandleInteraction();
        input.Gameplay.Sprint.started -= ctx => _playerMovement.Sprint();
        input.Gameplay.Sprint.canceled -= ctx => _playerMovement.ReleaseSprint();
        input.Gameplay.Aim.started -= ctx => _playerShooting.Aim();
        input.Gameplay.Aim.canceled -= ctx => _playerShooting.AimRelease();
        //input.Gameplay.Shooting.started -= ctx => _playerShooting.Shoot(-1);
        //input.Gameplay.Pause.started -= ctx => GameManager.instance.UpdateGameState(GameState.Pause);
    }
}
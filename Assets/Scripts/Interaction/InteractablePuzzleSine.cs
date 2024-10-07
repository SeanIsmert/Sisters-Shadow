using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

public class InteractablePuzzleSine : MonoBehaviour, IInteract
{
#region Variables
    [Header("SineWave Settings")]
    [SerializeField, Range(-2, 2)] private float _amplitude;
    [SerializeField, Range(-1.5f, 1.5f)] private float _frequency;
    [Space]

    [Header("Initialize")]
    [Tooltip("The GameObject holding the UI for the puzzle")]
    [SerializeField] private GameObject _sineWaveUI;
    [Tooltip("The GameObject holding the UI for the puzzle")]
    [SerializeField] private GameObject _sineWavePlayerUI;
    [Tooltip("The GameObject to set false when completing the puzzle")]
    [SerializeField] private GameObject _objectToDisapear;
    [Space]

    [Header("Hardcode later")]
    [SerializeField] private int _points;

    private Action<InputAction.CallbackContext> _checkAction;
    private Action<InputAction.CallbackContext> _exitAction;
    private Coroutine _animateSineWavePlayer;
    private Coroutine _animateSineWave;
    private LineRenderer _sineWavePlayer;
    private LineRenderer _sineWave;
    private Vector2 _xLimit = new Vector2(0, 4);
    private float _moveSpeed = 1;
    private float _playerAmplitude;
    private float _playerFrequency;



    public Vector3 Position { get { return transform.position; } }
#endregion

#region Initialize
    private void Start()
    {
        _sineWave = _sineWaveUI.GetComponent<LineRenderer>();
        _sineWavePlayer = _sineWavePlayerUI.GetComponent<LineRenderer>();
    }

    public void Interaction()
    {
        _exitAction = ctx => Exit(); PlayerInputManager.input.UI.Cancel.performed += _exitAction; // Subscribe to be able to leave puzzle
        _checkAction = ctx => Check(); PlayerInputManager.input.UI.Submit.performed += _checkAction; // Subscribe to be able to check puzzle
        GameManager.instance.UpdateGameState(GameState.Interactable); // Set your game state to ensure no moving and button usability
        _sineWaveUI.SetActive(true); // Make the UI for the puzzle visible

        _animateSineWave = StartCoroutine(AnimatedSine());
        _animateSineWavePlayer = StartCoroutine(AnimatedPlayerSine());
    }

    private void Exit()
    {
        WipeAnimations();

        _exitAction = ctx => Exit(); PlayerInputManager.input.UI.Cancel.performed -= _exitAction; // Unsubscribe to mitigate odd behavior
        _checkAction = ctx => Check(); PlayerInputManager.input.UI.Submit.performed -= _checkAction; // Unsubscribe to mitigate odd behavior
        GameManager.instance.UpdateGameState(GameState.Gameplay); // Set your game state to return to gameplay
        _sineWaveUI.SetActive(false); // Get rid of the UI displaying the puzzle
    }
    #endregion

#region Logic
    private void Check()
    {
        if (Mathf.Abs(_playerAmplitude - _amplitude) < 0.1f && Mathf.Abs(_playerFrequency - _frequency) < 0.1f)
        {
            _objectToDisapear.SetActive(false);
        }
        else
        {            
            _playerAmplitude = 0;
            _playerFrequency = 0;
        }

    }
#endregion

#region Animation
    private IEnumerator AnimatedSine()
    {
        // Declare local varriables
        float time = 0f;
        float leftX = _xLimit.x;
        float rightX = _xLimit.y;
        float tau = 2 * Mathf.PI;

        _sineWave.positionCount = _points;
        while (true)
        {
            time += Time.deltaTime * _moveSpeed; // Animate over time

            for (int currentPoint = 0; currentPoint < _points; currentPoint++)
            {
                float normalizedProgress = (float)currentPoint / (_points - 1);
                float x = Mathf.Lerp(leftX, rightX, normalizedProgress); // Spread points across the x range
                float y = _amplitude * Mathf.Sin((tau * _frequency * x) + time); // Effect points in the y range
                _sineWave.SetPosition(currentPoint, new Vector3(x, y, 0));
            }

            yield return null;
        }

    }

    private IEnumerator AnimatedPlayerSine()
    {
        // Declare local varriables
        float time = 0f;
        float leftX = _xLimit.x;
        float rightX = _xLimit.y;
        float tau = 2 * Mathf.PI;

        _sineWavePlayer.positionCount = _points;
        while (true)
        {
            Vector2 input = PlayerInputManager.input.UI.Navigate.ReadValue<Vector2>(); // Read our input so we can change our sine

            _playerFrequency += (input.x > 0 ? 0.003f : (input.x < 0 ? -0.003f : 0)); // Adjust frequency
            _playerAmplitude += (input.y > 0 ? 0.003f : (input.y < 0 ? -0.003f : 0)); // Adjust amplitude
            Mathf.Clamp(_playerFrequency, -1.5f, 1.5f); // Clamp
            Mathf.Clamp(_playerAmplitude, -2, 2); // Clamp

            time += Time.deltaTime * _moveSpeed; // Animate over time

            for (int currentPoint = 0; currentPoint < _points; currentPoint++)
            {
                float normalizedProgress = (float)currentPoint / (_points - 1);
                float x = Mathf.Lerp(leftX, rightX, normalizedProgress); // Spread points across the x range
                float y = _playerAmplitude * Mathf.Sin((tau * _playerFrequency * x) + time); // Effect points in the y range
                _sineWavePlayer.SetPosition(currentPoint, new Vector3(x, y, 0)); // Set position for the player's sine wave
            }

            yield return null;
        }
    }

    private void WipeAnimations()
    {
        if (_animateSineWave != null)
        {
            StopCoroutine(_animateSineWave);
        }
        if (_animateSineWavePlayer != null)
        {
            StopCoroutine(_animateSineWavePlayer);
        }
    }
#endregion
}

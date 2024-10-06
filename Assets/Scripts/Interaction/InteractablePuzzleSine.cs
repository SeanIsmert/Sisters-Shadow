using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Windows;

public class InteractablePuzzleSine : MonoBehaviour, IInteract
{
#region Variables
    [Header("SineWave Settings")]
    [SerializeField, Range(0, 2)] private float _amplitude;
    [SerializeField, Range(0, 1.5f)] private float _frequency;
    [Space]

    [Header("Initialize")]
    [Tooltip("The GameObject holding the UI for the puzzle")]
    [SerializeField] private GameObject _sineWaveUI;
    [Tooltip("The LineRender component that will allow us to see this sign wave.")]
    [SerializeField] private LineRenderer _sineWave;
    [Tooltip("The GameObject holding the UI for the puzzle")]
    [SerializeField] private GameObject _sineWavePlayerUI;
    [Tooltip("The LineRender component that will allow us to see this sign wave.")]
    [SerializeField] private LineRenderer _sineWavePlayer;
    [SerializeField] private GameObject _objectToDisapear;
    [Space]

    [Header("Hardcode later")]
    [SerializeField] private int _points;

    private Action<InputAction.CallbackContext> _checkAction;
    private Action<InputAction.CallbackContext> _exitAction;
    private Coroutine _animatePlayerSineWave;
    private Coroutine _animateSineWave;
    private Vector2 _xLimit = new Vector2(0, 4);
    private float _moveSpeed = 1;



    public Vector3 Position { get { return transform.position; } }
#endregion

#region Initialize
    public void Interaction()
    {
        _exitAction = ctx => Exit(); PlayerInputManager.input.UI.Cancel.performed += _exitAction; // Subscribe to be able to leave puzzle
        _checkAction = ctx => Check(); PlayerInputManager.input.UI.Submit.performed += _checkAction; // Subscribe to be able to check puzzle
        GameManager.instance.UpdateGameState(GameState.Interactable); // Set your game state to ensure no moving and button usability
        _sineWaveUI.SetActive(true); // Make the UI for the puzzle visible

        _animateSineWave = StartCoroutine(AnimatedSine());
        _animatePlayerSineWave = StartCoroutine(AnimatedPlayerSine());
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

    private void Check()
    {
        
    }

#region Animation
    private IEnumerator AnimatedSine()
    {
        float time = 0f;
        float leftX = _xLimit.x;
        float rightX = _xLimit.y;
        float tau = 2 * Mathf.PI;

        _sineWave.positionCount = _points;
        while (true)
        {
            time += Time.deltaTime * _moveSpeed; // Increment time based on frame time

            for (int currentPoint = 0; currentPoint < _points; currentPoint++)
            {
                // Calculate the normalized position in the range of leftX and rightX
                float normalizedProgress = (float)currentPoint / (_points - 1);
                float x = Mathf.Lerp(leftX, rightX, normalizedProgress); // Spread points across the x range
                float y = _amplitude * Mathf.Sin((tau * _frequency * x) + time); // Apply the sine wave formula
                _sineWave.SetPosition(currentPoint, new Vector3(x, y, 0));
            }

            yield return null; // Wait for the next frame
        }

    }

    private IEnumerator AnimatedPlayerSine()
    {
        float time = 0f;
        float leftX = _xLimit.x;
        float rightX = _xLimit.y;
        float tau = 2 * Mathf.PI;

        float amplitude = 0;
        float frequency = 0;

        _sineWavePlayer.positionCount = _points;
        while (true)
        {
            Vector2 input = PlayerInputManager.input.UI.Navigate.ReadValue<Vector2>();


            // Increment amplitude and frequency based on input direction
            amplitude += (input.x > 0 ? 0.01f : (input.x < 0 ? -0.01f : 0)); // Adjust amplitude
            frequency += (input.y > 0 ? 0.01f : (input.y < 0 ? -0.01f : 0)); // Adjust frequency

            // Clamp the values
            amplitude = Mathf.Clamp(amplitude, 0, 2);
            frequency = Mathf.Clamp(frequency, 0, 1.5f);

            time += Time.deltaTime * _moveSpeed; // Increment time based on frame time

            for (int currentPoint = 0; currentPoint < _points; currentPoint++)
            {
                float normalizedProgress = (float)currentPoint / (_points - 1);
                float x = Mathf.Lerp(leftX, rightX, normalizedProgress); // Spread points across the x range
                float y = amplitude * Mathf.Sin((tau * frequency * x) + time); // Sine wave formula
                _sineWavePlayer.SetPosition(currentPoint, new Vector3(x, y, 0)); // Set position for the player's sine wave
            }

            if (Mathf.Abs(amplitude - _amplitude) < 0.1f && Mathf.Abs(frequency - _frequency) < 0.1f)
            {
                _objectToDisapear.SetActive(false);
            }

            yield return null; // Wait for the next frame
        }
    }

    private void WipeAnimations()
    {
        if (_animateSineWave != null)
        {
            StopCoroutine(_animateSineWave);
        }
        if (_animatePlayerSineWave != null)
        {
            StopCoroutine(_animatePlayerSineWave);
        }
    }
#endregion
}

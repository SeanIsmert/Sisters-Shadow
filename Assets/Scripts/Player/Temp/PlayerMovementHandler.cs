using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovementHandler : MonoBehaviour
{
    [Tooltip("Speed at which the character transitions between movement animations.")]
    [SerializeField] private float _movementSpeed = 1f;
    [Tooltip("Speed at which the character rotates. (variable on how left or right)")]
    [SerializeField] private float _rotationSpeed = 5.0f;
    [Tooltip("Angle left and right that determins a characters state of movement.")]
    [SerializeField] private float _movementThreshold = 0.1f;

    private CharacterController _characterController;
    private Animator _animator;
    private Vector2 _currentVelocity = Vector2.zero;
    private float _groundCheckDistance = 15f;
    private float _angleRange = 10f;
    private float _movementSprint;
    private float _movementWalk;
    private bool _isSprinting;
    private bool _isTurning;

    private Coroutine _coroutine;
    public bool _isLoading = true;

    public static PlayerMovementHandler instance { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _movementSprint = _movementSpeed * 1.65f;
        _movementWalk = _movementSpeed;

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

    public void UpdateMovement(Vector2 movementInput)
    {
        movementInput.Normalize();
        float angle = (_currentVelocity.y >= 0) ? Vector2.SignedAngle(Vector2.up, movementInput) : Vector2.SignedAngle(Vector2.down, movementInput);
        angle = (angle < 0) ? -angle : angle;

        StateCheck(angle);
        GroundCheck();


        if (_isTurning)
        {
            // Adjust rotation speed based on movement input direction
            _rotationSpeed = (movementInput.x > 0) ? Mathf.Abs(_rotationSpeed) : -Mathf.Abs(_rotationSpeed);

            // Rotate the character based on the adjusted rotation speed
            transform.Rotate(0, (_rotationSpeed * 150) * Time.deltaTime, 0);

            // Reset velocity to zero to stop any ongoing movement
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, Vector2.zero, Time.deltaTime * _movementSpeed * 3f);
        }

        else
        {
            if (_isSprinting)
            {
                movementInput = movementInput * 2;
                _movementSpeed = _movementSprint;
            }
            else
            {
                _movementSpeed = _movementWalk;
            }

            if (movementInput.magnitude < _movementThreshold)
            {
                _currentVelocity = Vector2.MoveTowards(_currentVelocity, Vector2.zero, Time.deltaTime * _movementSpeed * 3f);
            }
            else
            {
                if ((_currentVelocity.y <= 0 && movementInput.y > 0) || (_currentVelocity.y >= 0 && movementInput.y < 0))
                {
                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, movementInput, Time.deltaTime * _movementSpeed * 2f);
                }
                else
                {
                    _rotationSpeed = (_currentVelocity.x > 0) ? Mathf.Abs(_rotationSpeed) : -Mathf.Abs(_rotationSpeed);

                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, movementInput, Time.deltaTime * _movementSpeed);
                    transform.Rotate(0, ((_rotationSpeed * .40f) * angle) * Time.deltaTime, 0);
                }
            }
        }

        // Set the Animator parameters based on the current velocity
        _animator.SetFloat("VelocityX", _currentVelocity.x);
        _animator.SetFloat("VelocityY", _currentVelocity.y);
    }

    private void StateCheck(float angle)
    {
        if (angle > 90f - _angleRange && angle < 90f + _angleRange)
        {
            _animator.applyRootMotion = false;

            _isTurning = true;
        }
        else
        {
            _animator.applyRootMotion = true;

            _isTurning = false;
        }
    }

    public void Sprint()
    {
        _isSprinting = true;
    }

    public void ReleaseSprint()
    {
        _isSprinting = false;
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _groundCheckDistance))
        {
            if (!_characterController.isGrounded)
            {
                _characterController.Move(Vector3.down * (hit.distance - _characterController.skinWidth));
            }
        }
    }

    private void GameStateCheck(GameState state)
    {
        if (state != GameState.Gameplay)
        {
            _currentVelocity = Vector2.zero;
            _animator.SetFloat("VelocityX", _currentVelocity.x);
            _animator.SetFloat("VelocityY", _currentVelocity.y);
        }
        if (state == GameState.Lose)
        {
            _animator.Play("Player Dying");
            _animator.SetLayerWeight(1, 0);
            _animator.SetLayerWeight(2, 0);
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameStateCheck;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameStateCheck;
    }

    private IEnumerator WaitAndPerformAction()
    {
        yield return new WaitForSeconds(.5f);

        _isLoading = false;
    }
}
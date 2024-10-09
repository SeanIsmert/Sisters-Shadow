using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Player movement logic and state handler
/// Handles movement with root motion.
/// Written by: Kay
/// Modified by: Sean
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Movement Establish")]
    [Tooltip("Speed at which the character transitions between movement animations."), Range(0f, 5f)]
    [SerializeField] private float _movementTransition = 1f;
    [Tooltip("Angle left and right that determins a characters state of movement. Also used to determin the forward deadzone."), Range(0f, 10f)]
    [SerializeField] private float _movementAngle = 0.1f;
    [Space]

    [Header("Movement Settings")]
    [Tooltip("The player's standard movement speed."), Range(0f, 5f)]
    [SerializeField] private float _walkSpeed;
    [Tooltip("The player's movement speed while sprinting."), Range(0f, 5f)]
    [SerializeField] private float _sprintSpeed;
    [Tooltip("The player's movement speed while aiming."), Range(0f, 5f)]
    [SerializeField] private float _aimSpeed;
    [Tooltip("The player's rotation speed."), Range(0f, 5f)]
    [SerializeField] private float _rotationSpeed;
    [Space]

    [Header("Movement State")]
    public MoveStates curMoveState;

    private CharacterController _characterController;
    private PlayerAttack _playerAttack;
    private Animator _animator;
    private Vector2 _currentVelocity = Vector2.zero;
    private Vector2 _controllerInput = Vector2.zero;
    private bool _isSprinting;
    private bool _isAiming;

    // Gather references to required components.
    void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _animator = GetComponent<Animator>();
    }

    // Perform movement actions in  update.
    void Update()
    {
        CheckMovementState();
        CharacterMovement(PlayerInputManager.input.Gameplay.Locomotion.ReadValue<Vector2>());
        
    }

    /// <summary>
    /// Performs movement actions based on the movement state.
    /// Passes in varriables based on the state which help diversify movement states.
    /// </summary>
    private void CharacterMovement(Vector2 controllerInput)
    {
        _controllerInput = controllerInput;

        switch (curMoveState)
        {
            case MoveStates.Idle:
                SmoothTransition(Vector2.zero, _movementTransition * 2);
                break;

            case MoveStates.Walking:
                SmoothTransition(controllerInput * _walkSpeed, _movementTransition);
                CharacterRotation();
                break;

            case MoveStates.Sprinting:
                SmoothTransition(controllerInput * _sprintSpeed, _movementTransition);
                CharacterRotation();
                break;

            case MoveStates.Aiming:
                SmoothTransition(controllerInput * _aimSpeed, _movementTransition);
                CharacterRotation();
                break;

            case MoveStates.Turning:
                SmoothTransition(Vector2.zero * 0, _movementTransition * 2);
                CharacterRotation();
                break;
        }

        // Set the Animator parameters based on the current velocity
        _animator.SetFloat("VelocityX", _currentVelocity.x);
        _animator.SetFloat("VelocityY", _currentVelocity.y);
    }

    /// <summary>
    /// Our movement state check which based on conditions will set our players state.
    /// Is based on priority and will return certain states over others.
    /// </summary>
    private void CheckMovementState()
    {
        if (AngleCheck())
            curMoveState = MoveStates.Turning;
        else if (_isAiming)
            curMoveState = MoveStates.Aiming;
        else if (_isSprinting)
            curMoveState = MoveStates.Sprinting;
        else if (_controllerInput != Vector2.zero)
            curMoveState = MoveStates.Walking;
        else
            curMoveState = MoveStates.Idle;
    }

    /// <summary>
    /// Updates the current velocity based on the target velocity and movement speed
    /// </summary>
    private void SmoothTransition(Vector2 targetVelocity, float speedMultiplier)
    {
        if (_currentVelocity.magnitude > 1 && targetVelocity.magnitude < 1)
            speedMultiplier = speedMultiplier * 2f;
        else
            speedMultiplier = speedMultiplier * .5f;

        if ((_currentVelocity.y <= 0 && targetVelocity.y > 0) || (_currentVelocity.y >= 0 && targetVelocity.y < 0))
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, Time.deltaTime * speedMultiplier * 2);
        else
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, Time.deltaTime * speedMultiplier);
    }

    /// <summary>
    /// Multiplies input by rotation speed to apply rotation.
    /// </summary>
    public void CharacterRotation()
    {
        if (curMoveState == MoveStates.Idle)
            return;

        if (curMoveState == MoveStates.Turning)
        {
            // Adjust rotation speed based on movement input direction
            float rotationSpeed = (_controllerInput.x > 0) ? _rotationSpeed : -_rotationSpeed;

            transform.Rotate(0, (rotationSpeed * 150) * Time.deltaTime, 0);
        }
        else
        {
            float angle = (_controllerInput.y >= 0)
            ? Mathf.Abs(Vector2.SignedAngle(Vector2.up, _controllerInput))
            : Mathf.Abs(Vector2.SignedAngle(Vector2.down, _controllerInput));

            if (angle < _movementAngle && angle > 0f)
                return;

            float rotationSpeed = (_controllerInput.x >= 0) ? _rotationSpeed : -_rotationSpeed;
            angle = _controllerInput.y >= 0 ? angle : -angle;

            transform.Rotate(0, ((rotationSpeed * 2) * angle) * Time.deltaTime, 0);
        }

        return;
    }

    private bool GroundCheck(out RaycastHit hit)
    {
        //if (GameManager.instance.state == GameState.Animation)
        //    return null;

        Vector3 origin = transform.position + new Vector3(0, 0.2f, 0);
        //float radius = 0.5f;
        float maxDistance = 2f; 

        return Physics.Raycast(origin, Vector3.down, out hit, maxDistance);

        //Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        //return Physics.Raycast(ray, 5);
    }

    /// <summary>
    /// Bool method that returns true when the player is supposed to be rotating.
    /// Checks if input is within an area left or right, returns false when ok.
    /// </summary>
    private bool AngleCheck()
    {
        float angle = Mathf.Abs(Vector2.SignedAngle(Vector2.up, _controllerInput));

        if (angle > 90f - _movementAngle && angle < 90f + _movementAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Method for returning button press logic
    /// </summary>
    public void SprintCheck(bool check)
    {
        if (check)
            _isSprinting = true;
        else
            _isSprinting = false;
    }

    /// <summary>
    /// Method for returning button press logic
    /// </summary>
    public void AimCheck(bool check)
    {
        if (check)
        {
            _isAiming = true;
            _animator.SetBool("Aiming", true);
        }
        else
        {
            _isAiming = false;
            _animator.SetBool("Aiming", false);
        }
    }

    /// <summary>
    /// Method for animating player shoot information
    /// </summary>
    public void Shoot()
    {
        if (_animator.GetCurrentAnimatorStateInfo(2).IsName("Pistol Shoot"))
            return;
        if (_playerAttack.FireWeapon())
            _animator.Play("Pistol Shoot");

        return;
    }
}
public enum MoveStates
{
    Idle, Walking, Sprinting, Aiming, Turning
}

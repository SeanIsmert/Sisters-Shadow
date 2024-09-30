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

    private Animator _animator;
    private Vector2 _currentVelocity = Vector2.zero;
    private Vector2 _controllerInput = Vector2.zero;

    //Temp for test
    private bool _isAiming;
    private bool _isSprinting;

    // Gather references to required components.
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Perform movement actions in  update.
    void Update()
    {
        CheckMovementState();
        CharacterMovement(PlayerInputManager.input.Gameplay.Locomotion.ReadValue<Vector2>());
        CharacterRotation();
    }

    /// <summary>
    /// Performs movement actions based on the movement state.
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
                break;

            case MoveStates.Sprinting:
                SmoothTransition(controllerInput * _sprintSpeed, _movementTransition);
                break;

            case MoveStates.Aiming:
                SmoothTransition(controllerInput * _aimSpeed, _movementTransition);
                break;

            case MoveStates.Turning:
                SmoothTransition(Vector2.zero * 0, _movementTransition * 2);
                break;
        }

        //Old Code
        /*Vector3 force = new(0f, 0f, _movement.ReadValue<Vector2>().y);        // Creates a vector3 based on vertical axis input.

        if (force != Vector3.zero && curMoveState == MoveStates.Idle)         // Set movement state when appropriate, otherwise return to idle.
        {
            curMoveState = MoveStates.Moving;
        }
        else if (force == Vector3.zero && curMoveState == MoveStates.Moving)
            curMoveState = MoveStates.Idle;*/

        //Messy Switch
        /*
        switch (curMoveState)
        {
            case MoveStates.Idle:
                _currentVelocity = Vector2.MoveTowards(_currentVelocity, Vector2.zero, Time.deltaTime * _movementSpeed * 2f);
                break;
            case MoveStates.Walking:
                if ((_currentVelocity.y <= 0 && _targetVelocity.y > 0) || (_currentVelocity.y >= 0 && _targetVelocity.y < 0))
                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, _targetVelocity, Time.deltaTime * _movementSpeed * 2f);
                else
                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, _targetVelocity, Time.deltaTime * _movementSpeed);
                break;
            case MoveStates.Sprinting:
                _targetVelocity = _targetVelocity * 2;
                if ((_currentVelocity.y <= 0 && _targetVelocity.y > 0) || (_currentVelocity.y >= 0 && _targetVelocity.y < 0))
                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, _targetVelocity, Time.deltaTime * _movementSpeed * 2f);
                else
                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, _targetVelocity, Time.deltaTime * _movementSpeed);
                break;
            case MoveStates.Aiming:
                _targetVelocity = _targetVelocity * .5f;
                if ((_currentVelocity.y <= 0 && _targetVelocity.y > 0) || (_currentVelocity.y >= 0 && _targetVelocity.y < 0))
                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, _targetVelocity, Time.deltaTime * _movementSpeed * 2f);
                else
                    _currentVelocity = Vector2.MoveTowards(_currentVelocity, _targetVelocity, Time.deltaTime * _movementSpeed);
                break;
        }
        */

        // Set the Animator parameters based on the current velocity
        _animator.SetFloat("VelocityX", _currentVelocity.x);
        _animator.SetFloat("VelocityY", _currentVelocity.y);
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
            float rotationSpeed = (_controllerInput.x >= 0) ? _rotationSpeed : -_rotationSpeed;
            float angle = (_controllerInput.y >= 0)
            ? Mathf.Abs(Vector2.SignedAngle(Vector2.up, _controllerInput))
            : Mathf.Abs(Vector2.SignedAngle(Vector2.down, _controllerInput));
            angle = _controllerInput.y >= 0 ? angle : -angle;

            transform.Rotate(0, ((rotationSpeed * 2) * angle) * Time.deltaTime, 0);
        }

        return;

        /*
        //Vector3 force = new(0f, _movement.ReadValue<Vector2>().x);        // Creates a vector3 based on horizontal axis input.

        //Quaternion rotation = Quaternion.Euler(_rotateSpeed);     // Creates a quaternion using input multiplied by speed.
        //_rb.MoveRotation(transform.rotation * rotation);                  // Applies the rotation!
        */
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
           // _animator.applyRootMotion = false;

            return true;
        }
        else
        {
            //_animator.applyRootMotion = true;

            return false;
        }
    }

    public void SetMovementState(MoveStates newState)
    {

        curMoveState = newState;
    }

    private void CheckMovementState()
    { 
        if (AngleCheck())
            curMoveState = MoveStates.Turning;
        else if(_isAiming)
            curMoveState = MoveStates.Aiming;
        else if (_isSprinting)
            curMoveState = MoveStates.Sprinting;
        else if (_controllerInput != Vector2.zero)
            curMoveState = MoveStates.Walking;
        else
            curMoveState = MoveStates.Idle;
    }

    //Temp
    public void SprintCheck(bool check)
    {
        if (check)
            _isSprinting = true;
        else
            _isSprinting = false;
    }
    public void AimCheck(bool check)
    {
        if (check)
            _isAiming = true;
        else
            _isAiming = false;
    }
}
public enum MoveStates
{
    Idle, Walking, Sprinting, Aiming, Turning
}

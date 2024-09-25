using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

/// <summary>
/// Player movement logic and state handler
/// Handles with physics and a Rigidbody
/// Written by: Kay
/// Modified by: Sean
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Movement Values")]
    [Tooltip("Speed at which the character transitions between movement animations.")]
    [SerializeField] private float _movementSpeed = 1f;
    [Tooltip("Speed at which the character rotates. (variable on how left or right)")]
    [SerializeField] private float _rotationSpeed = 5.0f;
    [Tooltip("Angle left and right that determins a characters state of movement.")]
    [SerializeField] private float _movementAngle = 0.1f;
    [Tooltip("How far the player needs to move the joystick to begin moving.")]
    [SerializeField] private float _movementThreshold = 0.1f;
    [Space]

    [Header("Movement State")]
    public MoveStates curMoveState;

    /*
    [Header("Movement Settings")]
    [Tooltip("The player's standard movement speed.")]
    [SerializeField] private float _walkSpeed;
    [Tooltip("The player's movement speed while sprinting.")]
    [SerializeField] private float _sprintSpeed;
    [Tooltip("The player's movement speed while aiming.")]
    [SerializeField] private float _aimSpeed;
    [Tooltip("The player's rotation speed.")]
    [SerializeField] private float _rotateSpeed;
    */

    private Animator _animator;
    private Vector2 _currentVelocity = Vector2.zero;
    private Vector2 _targetVelocity = Vector2.zero;


    // Gather references to required components.
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Perform movement actions in fixed update.
    void Update()
    {
        CharacterMovement(PlayerInputManager.input.Gameplay.Locomotion.ReadValue<Vector2>());
        CharacterRotation();
    }

    /// <summary>
    /// Performs movement actions based on the movement state.
    /// </summary>
    private void CharacterMovement(Vector2 controllerInput)
    {
        controllerInput.Normalize(); _targetVelocity = controllerInput;

        if (_targetVelocity.magnitude < _movementThreshold)
            curMoveState = MoveStates.Idle;
        else if (curMoveState != MoveStates.Aiming || curMoveState != MoveStates.Sprinting)
            curMoveState = MoveStates.Walking;

        switch (curMoveState)
        {
            case MoveStates.Idle:
                SmoothTransition(Vector2.zero, _movementSpeed * 2);
                break;

            case MoveStates.Walking:
                SmoothTransition(_targetVelocity, _movementSpeed);
                break;

            case MoveStates.Sprinting:
                SmoothTransition(_targetVelocity * 2, _movementSpeed);
                break;

            case MoveStates.Aiming:
                SmoothTransition(_targetVelocity * 0.5f, _movementSpeed);
                break;

            case MoveStates.Turning:
                SmoothTransition(Vector2.zero * 0, _movementSpeed * 3);
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
        _currentVelocity = Vector2.MoveTowards(_currentVelocity, targetVelocity, Time.deltaTime * speedMultiplier);
    }

    /// <summary>
    /// Multiplies input by rotation speed to apply rotation.
    /// </summary>
    public void CharacterRotation()
    {
        if (AngleCheck())
        {
            _currentVelocity = Vector2.MoveTowards(_currentVelocity, Vector2.zero, Time.deltaTime * _movementSpeed * 3f);

            // Adjust rotation speed based on movement input direction
            float rotationSpeed = (_targetVelocity.x > 0) ? _rotationSpeed : -_rotationSpeed;

            transform.Rotate(0, (rotationSpeed * 150) * Time.deltaTime, 0);

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
        float angle = Mathf.Abs((_currentVelocity.y >= 0) 
            ? Vector2.SignedAngle(Vector2.up, _targetVelocity) 
            : Vector2.SignedAngle(Vector2.down, _targetVelocity));

        if (angle > 90f - _movementAngle && angle < 90f + _movementAngle)
        {
            _animator.applyRootMotion = false;

            return true;
        }
        else
        {
            _animator.applyRootMotion = true;

            return false;
        }
    }

    public void SetMovementState(MoveStates newState)
    {
            curMoveState = newState;
    }
}
public enum MoveStates
{
    Idle, Walking, Sprinting, Aiming, Turning
}

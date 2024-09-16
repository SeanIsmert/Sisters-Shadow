using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player movement logic and state handler
/// Handles with physics and a Rigidbody
/// Written by: Kay
/// Modified by: Sean
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private InteractableManager _interactableManager;
    private PlayerInput _input;
    private InputAction _movement;
    private Rigidbody _rb;

    [Header("Movement Values")]
    [Tooltip("The player's standard movement speed.")]
    [SerializeField] private float _walkSpeed;
    [Tooltip("The player's movement speed while sprinting.")]
    [SerializeField] private float _sprintSpeed;
    [Tooltip("The player's movement speed while aiming.")]
    [SerializeField] private float _aimSpeed;
    [Tooltip("The player's rotation speed.")]
    [SerializeField] private float _rotateSpeed;

    [Header("Movement State")]
    public MoveStates curMoveState;

    public enum MoveStates
    {
        Idle,
        Moving,
        Sprinting,
        Aiming,
    }

    // Gather references to required components.
    void Awake()
    {
        _interactableManager = GetComponent<InteractableManager>();
        _rb = GetComponent<Rigidbody>();
        _input = new PlayerInput();
    }

    // Subscribe to input events.
    private void OnEnable()
    {
        _input.Enable();
        _movement = _input.Gameplay.Locomotion;                                                     // Input for movement and rotation.
        _movement.Enable();
        _input.Gameplay.Sprint.performed += SprintActive;                                           // Input for sprinting.
        _input.Gameplay.Sprint.canceled += SprintEnded;
        _input.Gameplay.Aim.performed += AimActive;                                                 // Input for aiming.
        _input.Gameplay.Aim.canceled += AimEnded;
        _input.Gameplay.Interact.started += ctx => _interactableManager.HandleInteraction();        // Input for interaction.

    }

    // Unsubscribe from input events.
    private void OnDisable()
    {
        _input.Disable();
        _movement.Disable();
        _input.Gameplay.Sprint.performed -= SprintActive;
        _input.Gameplay.Sprint.canceled -= SprintEnded;
        _input.Gameplay.Aim.performed -= AimActive;
        _input.Gameplay.Aim.canceled -= AimEnded;
        _input.Gameplay.Interact.started -= ctx => _interactableManager.HandleInteraction();
    }

    // Perform movement actions in fixed update.
    void FixedUpdate()
    {
        CharacterMovement();
        CharacterRotation();
    }

    /// <summary>
    /// Performs movement actions based on the movement state.
    /// </summary>
    private void CharacterMovement()
    {
        Vector3 force = new(0f, 0f, _movement.ReadValue<Vector2>().y);        // Creates a vector3 based on vertical axis input.

        if (force != Vector3.zero && curMoveState == MoveStates.Idle)         // Set movement state when appropriate, otherwise return to idle.
        {
            curMoveState = MoveStates.Moving;
        }
        else if (force == Vector3.zero && curMoveState == MoveStates.Moving)
            curMoveState = MoveStates.Idle;

        switch (curMoveState)
        {
            case MoveStates.Idle:
                                                // Quit standin' around!
                break;

            case MoveStates.Moving:
                force.z *= _walkSpeed;          // Moving state applies walk speed.
                break;

            case MoveStates.Sprinting:
                force.z *= _sprintSpeed;        // Sprinting state applies sprint speed.
                break;

            case MoveStates.Aiming:
                force.z *= _aimSpeed;           // Aiming state applies aim speed.
                break;
        }

        force = _rb.rotation * force;                                               // Translates movement into local space.
        _rb.MovePosition(transform.position + (force * Time.fixedDeltaTime));       // Applies the final movement value to the ridigbody.
    }

    /// <summary>
    /// Multiplies input by rotation speed to apply rotation.
    /// </summary>
    private void CharacterRotation()
    {
        Vector3 force = new(0f, _movement.ReadValue<Vector2>().x);        // Creates a vector3 based on horizontal axis input.

        Quaternion rotation = Quaternion.Euler(_rotateSpeed * force);     // Creates a quaternion using input multiplied by speed.
        _rb.MoveRotation(transform.rotation * rotation);                  // Applies the rotation!
    }

    /// <summary>
    /// Actions to perform when sprint input is pressed.
    /// </summary>
    /// <param name="context"></param>
    private void SprintActive(InputAction.CallbackContext context)
    {
        if (curMoveState != MoveStates.Aiming)      // Aiming takes priority?
            curMoveState = MoveStates.Sprinting;
    }

    /// <summary>
    /// Actions to perform when sprint input is released.
    /// </summary>
    /// <param name="context"></param>
    private void SprintEnded(InputAction.CallbackContext context)
    {
        if (curMoveState != MoveStates.Aiming)
            curMoveState = MoveStates.Idle;
    }

    /// <summary>
    /// Actions to perform when aim input is pressed.
    /// </summary>
    /// <param name="context"></param>
    private void AimActive(InputAction.CallbackContext context)
    {
        curMoveState = MoveStates.Aiming;
    }

    /// <summary>
    /// Actions to perform when aim input is released.
    /// </summary>
    /// <param name="context"></param>
    private void AimEnded(InputAction.CallbackContext context)
    {
        curMoveState = MoveStates.Idle;
    }
}

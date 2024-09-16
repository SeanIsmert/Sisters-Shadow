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
        _movement = _input.Gameplay.Locomotion;
        _movement.Enable();                     // Input for movement and rotation.
        _input.Gameplay.Sprint.Enable();        // Input for sprinting.
        _input.Gameplay.Aim.Enable();           // Input for aiming.
        _input.Gameplay.Interact.started += ctx => _interactableManager.HandleInteraction();

    }

    // Unsubscribe from input events.
    private void OnDisable()
    {
        _input.Disable();
        _movement.Disable();
        _input.Gameplay.Sprint.Disable();
        _input.Gameplay.Aim.Disable();
        _input.Gameplay.Interact.started -= ctx => _interactableManager.HandleInteraction();
    }

    // Read player input to determine movement state.
    void Update()
    {
        SetMovementState();
    }

    // Perform movement actions in fixed update.
    void FixedUpdate()
    {
        CharacterMovement();
        CharacterRotation();
    }

    /// <summary>
    /// Reads player input to determine movement state.
    /// </summary>
    private void SetMovementState()
    {
        if (_movement.ReadValue<Vector2>() != Vector2.zero)
        {
            if (_input.Gameplay.Aim.phase == InputActionPhase.Performed)
                curMoveState = MoveStates.Aiming;                                       // Aiming state takes priority over all others.

            else if (_input.Gameplay.Sprint.phase == InputActionPhase.Performed)
                curMoveState = MoveStates.Sprinting;                                    // Sprinting state comes second.

            else
                curMoveState = MoveStates.Moving;                                       // Moving state if there are no extra inputs.
        }
        else
            curMoveState = MoveStates.Idle;                                             // Idle if no input at all.
    }

    /// <summary>
    /// Performs movement actions based on the movement state.
    /// </summary>
    private void CharacterMovement()
    {
        Vector3 force = new(0f, 0f, _movement.ReadValue<Vector2>().y);        // Creates a vector3 based on vertical axis input.

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
}

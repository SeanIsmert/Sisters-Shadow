using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private int _attackRange;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Vector3 _targetingOffset;

    private void OnEnable()
    {
        PlayerInputManager.input.Gameplay.Shoot.performed += FireWeapon;        // Subscribe to shoot input.
    }

    private void OnDisable()
    {
        PlayerInputManager.input.Gameplay.Shoot.performed -= FireWeapon;
    }

    private void FireWeapon(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

        Ray ray = new(transform.position + _targetingOffset, transform.forward);       // Ray forward from player object.
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _attackRange, _layerMask))
        {
            hit.transform.gameObject.GetComponent<IDamageable>()?.ValueChange(_damageAmount);       // Apply damage.
        }

        Debug.DrawLine(transform.position, ray.GetPoint(_attackRange), Color.magenta, 5f);
    }
}

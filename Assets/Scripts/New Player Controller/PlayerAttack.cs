using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private int _attackRange;
    [SerializeField] private LayerMask _layerMask;

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
        Ray ray = new(transform.position, transform.forward);       // Ray forward from player object.
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _attackRange, _layerMask))
        {
            hit.transform.gameObject.GetComponent<IDamageable>()?.ValueChange(_damageAmount);       // Apply damage.
        }

        Debug.DrawRay(transform.position, transform.forward, Color.magenta, 5f);
    }
}

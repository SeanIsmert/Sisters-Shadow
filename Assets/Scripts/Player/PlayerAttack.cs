using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private int _attackRange;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Vector3 _raycastOffset;

    private PlayerMovementHandler _movementHandler;

    private void Awake()
    {
        _movementHandler = GetComponent<PlayerMovementHandler>();
    }

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
        if(_movementHandler.curMoveState == MoveStates.Aiming)
        {
            foreach (InventoryItem item in InventoryManager.instance.inventory)                  // Find the appropriate ammo item.
            {
                if (item.itemName == "Light Ammo" && item.amount > 0)
                {
                    item.amount -= 1;

                    Ray ray = new(transform.position + _raycastOffset, transform.forward);       // Ray forward from player object.
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, _attackRange, _layerMask))
                    {
                        hit.transform.gameObject.GetComponent<IDamageable>()?.ValueChange(_damageAmount);       // Apply damage.
                    }

                    Debug.DrawLine(transform.position + _raycastOffset, ray.GetPoint(_attackRange), Color.magenta, 5f);          // Debug ray.

                    InventoryManager.instance.refreshInventory();                                               // Refresh inventory UI.
                }
            }
        }
    }
}
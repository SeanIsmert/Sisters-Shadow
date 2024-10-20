using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private int _attackRange;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Vector3 _raycastOffset;

    [SerializeField] private InventoryItem _ammo;

    private PlayerMovementHandler _movementHandler;

    private void Awake()
    {
        _movementHandler = GetComponent<PlayerMovementHandler>();
    }

    public bool FireWeapon()
    {
        if (_movementHandler.curMoveState == MoveStates.Aiming)
        {
            Dictionary<InventoryItem, uint> playerInven = PlayerInventory.instance.playerInventory;     // Grab reference to inventory dictionary.

            if(playerInven.ContainsKey(_ammo))
            {
                playerInven[_ammo] -= 1;

                Ray ray = new(transform.position + _raycastOffset, transform.forward);       // Ray forward from player object.
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _attackRange, _layerMask))
                {
                    hit.transform.gameObject.GetComponent<IDamageable>()?.ValueChange(_damageAmount);       // Apply damage.
                }

                Debug.DrawLine(transform.position + _raycastOffset, ray.GetPoint(_attackRange), Color.magenta, 5f);          // Debug ray.

                PlayerInventory.instance.RefreshInventory();                                               // Refresh inventory UI.
                return true;
            }
        }
        
        return false;
    }
}
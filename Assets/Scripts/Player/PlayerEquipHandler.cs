using UnityEngine;

public class PlayerEquipHandler : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private int _attackRange;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Vector3 _raycastOffset;

    private int _weaponType = 0;
    public int GetWeaponType() { return _weaponType; }

    public void ItemEquip(ItemToken item)
    {
        if (item.GetBaseItem.weapon)
        {
            foreach (var token in InventoryManager.Instance.GetInventoryItems())
            {
                if (token.GetBaseItem.weapon)
                    token.Setequiped = false;
            }
            item.Setequiped = true;
        }

        switch (item.GetBaseItem.itemID) // Note this is the not smart way of handling things. it will work
        {
            case "Flashlight":
                item.Setequiped = !item.GetEquiped;
                break;

            default: break;
        }
    }

    public void WeaponType() // int 0 = Pistol || int 1 = Shotgun
    {
        foreach (var token in InventoryManager.Instance.GetInventoryItems())
        {
            if (!token.GetEquiped) // skip items that arn't stackable items
                continue;

            if (token.GetBaseItem.itemID == "Pistol")
                _weaponType = 0;
            if (token.GetBaseItem.itemID == "Shotgun")
                _weaponType = 1;
        }
    }

    public bool AmmoCheck()
    {
        switch (_weaponType)
        {
            case 0: // Pistol
                foreach (var token in InventoryManager.Instance.GetInventoryItems())
                {
                    if (!token.GetBaseItem.consumable) // skip items that arn't consumable items
                        continue;

                    if (token.GetBaseItem.itemID == "LightAmmo" && token.GetItemAmount > 0)
                    {
                        token.ChangeValue(-1);
                        if (token.GetItemAmount == 0)
                            InventoryManager.Instance.RemoveItem(token);

                        return true;
                    }
                }
                break;

            case 1: // Shotgun
                foreach (var token in InventoryManager.Instance.GetInventoryItems())
                {
                    if (!token.GetBaseItem.consumable) // skip items that arn't consumable items
                        continue;

                    if (token.GetBaseItem.itemID == "HeavyAmmo" && token.GetItemAmount > 0)
                    {
                        token.ChangeValue(-1);
                        if (token.GetItemAmount == 0)
                            InventoryManager.Instance.RemoveItem(token);

                        return true;
                    }
                }
                break;

            default:
                break;
        }
        return false;
    }

    public void Shoot()
    {
        Ray ray = new(transform.position + _raycastOffset, transform.forward);       // Ray forward from player object.
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _attackRange, _layerMask))
        {
            hit.transform.gameObject.GetComponent<IDamageable>()?.ValueChange(_damageAmount);       // Apply damage.
        }

        Debug.DrawLine(transform.position + _raycastOffset, ray.GetPoint(_attackRange), Color.magenta, 5f);
    }

#region Subscription
    private void OnEnable()
    {
        InventoryManager.InventoryChanged += WeaponType;
        InventoryManager.ItemEquiped += WeaponType;
    }
    private void OnDisable()
    {
        InventoryManager.InventoryChanged -= WeaponType;
        InventoryManager.ItemEquiped -= WeaponType;
    }
#endregion
}
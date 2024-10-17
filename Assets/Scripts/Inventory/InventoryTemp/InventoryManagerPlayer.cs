using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerPlayer : MonoSinglton<InventoryManagerPlayer>
{
    [Header("Player Inventory Settings")]
    [SerializeField] private List<Item> _items;
    [Tooltip("The max number of slots currently (contains getter and setter)"), Range(0, 20)]
    [SerializeField] private int _inventoryMax;

    public int GetInventorySize() { return _inventoryMax; }
    public List<Item> GetInvetoryItems() { return _items; }


    private uint _ammoLight;
    private uint _ammoMedium;
    private uint _ammoHeavy;

    void Start()
    {
        UIInventory.Instance.GenerateInventory();
    }

    private void ExpandSlots(int size)
    {
        _inventoryMax += size;
    }

    private bool AddItem()
    {
        return false;
    }
}

using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoSinglton<InventoryManager>
{
    [Header("Player Inventory Settings")]
    [SerializeField] private List<Item> _playerItems;
    [Tooltip("The max number of slots currently (contains getter and setter)"), Range(0, 20)]
    [SerializeField] private int _inventoryMax;
    [Space]

    [Header("Global Inventory Settings")]
    [SerializeField] private List<Item> _globalItems;

    public int GetInventorySize() { return _inventoryMax; }
    public List<Item> GetInvetoryItems() { return _playerItems; }
    public List<Item> GetGlobalItems() { return _globalItems; }

    public static event Action InventoryChanged;
    private enum ItemType { Player, Global }
    private ItemType _itemType;

    void Start()
    {
        _playerItems.Capacity = _inventoryMax;
        UIInventory.Instance.GenerateInventory();
    }

    private void ExpandSlots(int size)
    {
        _inventoryMax += size;
    }

    /// <summary>
    /// 
    /// </summary>
    private bool AddItem(Item item)
    {
        if (_playerItems.Count > _inventoryMax)
        {
            return false;
        }
        _playerItems.Add(item);

        InventoryChanged?.Invoke();
        return true;
    }

    private void RemoveItem(Item item)
    {
        _playerItems.Remove(item);
    }

    /// <summary>
    /// If you are passing in an item type of player, it moves to global<list>
    /// If you are passing in an item type of global, it moves to player<list>
    /// </summary>
    private void MoveItem(Item item, ItemType type)
    {
        switch (type)
        {
            case ItemType.Player:
                _globalItems.Add(item);
                _playerItems.Remove(item);
                break; 
            case ItemType.Global:
                if (!AddItem(item))
                {
                    return;
                }
                _globalItems.Remove(item);
                break;
        }
    }

    //Save and load implement here
    /* Inventory sizemax
     * list player
     * list global
    */
}

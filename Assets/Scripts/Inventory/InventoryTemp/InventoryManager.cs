using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoSinglton<InventoryManager>, IDataPersistence
{
    [Header("Player Inventory Settings")]
    [SerializeField] private List<ItemDataToken> _playerItems;
    [Tooltip("The max number of slots currently (contains getter and setter)"), Range(0, 20)]
    [SerializeField] private int _inventoryMax;
    [Space]

    [Header("Global Inventory Settings")]
    [SerializeField] private List<ItemDataToken> _globalItems;

    public int GetInventorySize() { return _inventoryMax; }
    public List<ItemDataToken> GetInvetoryItems() { return _playerItems; }
    public List<ItemDataToken> GetGlobalItems() { return _globalItems; }

    public static event Action InventoryChanged;
    private enum ItemType { Player, Global }
    private ItemType _itemType;

    void Start()
    {
        _playerItems.Capacity = _inventoryMax;
        UIInventory.Instance.GenerateInventory();
    }

    /// <summary>
    /// Expands the inventory slots
    /// </summary>
    private void ExpandSlots(int size)
    {
        _inventoryMax += size;
    }

    /// <summary>
    /// Adds items specifically to the Player Inventory
    /// </summary>
    private bool AddItem(Item item)
    {
        if (_playerItems.Count > _inventoryMax)
        {
            return false;
        }
        _playerItems.Add(item.GenerateToken());

        InventoryChanged?.Invoke();
        return true;
    }

    private bool AddItem(ItemDataToken item)
    {
        if (_playerItems.Count > _inventoryMax)
        {
            return false;
        }
        _playerItems.Add(item);

        InventoryChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Removes Items specifically from the Player Inventory
    /// </summary>
    private void RemoveItem(ItemDataToken item)
    {
        _playerItems.Remove(item);
    }

    public void LoadData(GameData data)
    {
        throw new NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        throw new NotImplementedException();
    }

    /*
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
    */

    //Save and load implement here
    /* Inventory sizemax
     * list player
     * list global
    */
}

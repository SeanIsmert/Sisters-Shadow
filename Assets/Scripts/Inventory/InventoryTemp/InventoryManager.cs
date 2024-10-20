using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoSinglton<InventoryManager>
{
    [Header("Player Inventory Settings")]
    [SerializeField] private List<ItemToken> _playerItems;
    [Tooltip("The max number of slots currently (contains getter and setter)"), Range(0, 9)]
    [SerializeField] private int _inventoryMax;
    [Space]

    [Header("Global Inventory Settings")]
    [SerializeField] private List<ItemToken> _globalItems;

    public int GetInventorySize() { return _inventoryMax; }
    public List<ItemToken> GetInventoryItems() { return _playerItems; }
    public List<ItemToken> GetGlobalItems() { return _globalItems; }

    public static event Action InventoryChanged;
    public enum ItemType { Player, Global, None }

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
    /// Adds items specifically to the Player Inventory from gameplay
    /// </summary>
    public bool AddItem(ItemBase item)
    {
        if (_playerItems.Count >= _inventoryMax) // Check if inventory is full
        {
            return false;
        }
        else if (_playerItems.Count == 0) // Needs to exist because weirdness with it skipping stackables if that is the first item you pickup
        {
            _playerItems.Add(item.GenerateToken()); // Add new token if the inventory is empty
            InventoryChanged?.Invoke(); // Notify listeners that the inventory has changed
            return true;
        }

        ItemToken newToken = item.GenerateToken(); // Generate the item info so we can evaluate it
        ItemToken curToken = null;

        if (item.stackable)
        {
            foreach (var token in _playerItems)
            {
                if (token.GetBaseItem != item) // if the object is not the same base type continue
                    continue;
                if (token.GetItemAmount == token.GetBaseItem.maxStack) // if the object is full continue
                    continue;

                curToken = token; // you have found a item worth stacking into
                break;
            }

            if (curToken != null) // do logic on the item you found
            {
                int excessAmount = (int)(curToken.GetItemAmount + newToken.GetItemAmount) - (int)curToken.GetBaseItem.maxStack; // Combine existing and new amounts

                if (excessAmount > 0) // We exceed max stack
                {
                    curToken.SetItemAmount = curToken.GetBaseItem.maxStack; // Fill up our existing token

                    ItemToken excessToken = new ItemToken(item, (uint)excessAmount); // create a new token
                    _playerItems.Add(excessToken); // send it to our inventory
                }
                else // we can add to it
                {
                    curToken.SetItemAmount = curToken.GetItemAmount + (uint)MathF.Abs(excessAmount); // Increment the total amount
                }
                // Return early since we've successfully handled the item
                InventoryChanged?.Invoke(); // Notify listeners that the inventory has changed
                return true;
            }
            else
            {
                _playerItems.Add(newToken); // Add new token if not you didn't find another token
            }

        }
        else
        {
            _playerItems.Add(newToken); // Add new token if not stackable
        }

        InventoryChanged?.Invoke(); // Notify listeners that the inventory has changed
        return true;
    }

    /// <summary>
    /// If you are passing in an item type of player, it moves to global<list>
    /// If you are passing in an item type of global, it moves to player<list>
    /// </summary>
    public void MoveItem(ItemToken item)
    {
        ItemType itemType = CheckListType(item); // Get the current list type of the item

        switch (itemType)
        {
            case ItemType.Player: // Move from player to global
                //if (_globalItems.Count >= _inventoryGlobalMax)
                    //return;
                _playerItems.Remove(item); // Remove from player list
                _globalItems.Add(item); // Add to global list
                break;

            case ItemType.Global: // Move from global to player
                if (_playerItems.Count >= _inventoryMax)
                    return; // If the player list is full, do not proceed
                _globalItems.Remove(item); // Remove from global list
                _playerItems.Add(item); // Add to player list
                break;

            case ItemType.None:
                Debug.Log("Cannot move item; not found in either list.");
                return;
        }

        InventoryChanged?.Invoke(); // Notify listeners that the inventory has changed
    }

    /// <summary>
    /// Simple Remove item for us to use
    /// </summary>
    public void RemoveItem(ItemToken item)
    {
        _playerItems.Remove(item);
    }

    private ItemType CheckListType(ItemToken item)
    {
        if (_playerItems.Contains(item))
        {
            return ItemType.Player; // Item found in player inventory
        }
        else if (_globalItems.Contains(item))
        {
            return ItemType.Global; // Item found in global inventory
        }
        else
        {
            Debug.LogWarning("Item not found in any list.");
            return ItemType.None; // None if the item is not found in either list
        }
    }

    public void LoadData(GameData data)
    {
        throw new NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        throw new NotImplementedException();
    }

    //Save and load implement here
    /* Inventory sizemax
     * list player
     * list global
    */
}

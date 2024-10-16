using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public SerializableDictionary<InventoryItem, uint> playerInventory;     // Dictionary for holding item scriptable objects and their related amounts.

    //public List<InventoryItem> inventoryItems;                            // Old inventory list. Code uses the dictionary now!
    public List<InventorySlot> inventorySlots;                              // List of UI elements for inventory slots.
    public int maxSize;                                                     // The maximum inventory size.

    private GameObject _inventorySlotPrefab;
    private GameObject _inventoryCanvas;
    private GameObject _inventoryPanel;

    public static PlayerInventory instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        _inventorySlotPrefab = UIInventory.Instance.GetInventorySlot();
        _inventoryCanvas = UIInventory.Instance.GetInventoryCanvas();
        _inventoryPanel = UIInventory.Instance.GetInventoryPanel();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //GameManager.instance.UpdateGameState(GameState.Inventory);
        for (int i = 0; i < maxSize; i++) 
        { 
            var slot = Instantiate(_inventorySlotPrefab, _inventoryPanel.transform);
            inventorySlots.Add(slot.GetComponent<InventorySlot>());
        }

        RefreshInventory();
    }

    public void RefreshInventory()
    {
        Dictionary<InventoryItem, uint> invMirror = new(playerInventory);       // Local copy of the inventory for iteration.
        int index = 0;                                                          // Index for setting inventory slot data.

        //Dictionary Things
        foreach (KeyValuePair<InventoryItem, uint> item in invMirror)
        {
            // If a multiple item's amount reaches zero.
            if (item.Key.multiple && item.Value <= 0)
            {
                inventorySlots[index].onLoad(null, 0);      // Send inventorySlot null to remove from UI.
                playerInventory.Remove(item.Key);           // Remove the item from inventory.
                index++;                                    // Iterate index.
                continue;
            }

            inventorySlots[index].onLoad(item.Key, item.Value);     // Send inventorySlot updated item info.
            index++;                                                // Iterate index.
        }

        /*
        // OLD INVENTORY ITERATION
        for (int i = 0; i < playerInventory.Count; i++)
        {
            inventorySlots[i].onLoad(playerInventory[i]);
        }
        */
    }

    public bool AddItem(InventoryItem newItem)
    {
        if(playerInventory.Count > maxSize)
        {
            return false;
        }

        // Dictionary stuff
        if(playerInventory.ContainsKey(newItem))
        {
            playerInventory[newItem] += newItem.amount;
        }
        else
            playerInventory.Add(newItem, newItem.amount);

        // UI Stuff
        RefreshInventory();

        return true;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if (state == GameState.Inventory)
            UIInventory.Instance.OpenUI();
        else
            UIInventory.Instance.CloseUI();

    }
}

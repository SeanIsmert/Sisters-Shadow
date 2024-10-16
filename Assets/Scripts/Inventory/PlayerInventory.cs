using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public SerializableDictionary<InventoryItem, uint> playerInventory;     // Dictionary for holding item scriptable objects and their related amounts.

    public List<InventoryItem> inventoryItems;
    public List<InventorySlot> inventorySlots;
    public int maxSize;

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
        Dictionary<InventoryItem, uint> invMirror = new(playerInventory);

        //Dictionary Things
        foreach (KeyValuePair<InventoryItem, uint> item in invMirror)
        {
            if (item.Value <= 0)
                playerInventory.Remove(item.Key);
        }
        
        // UI Things.
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventorySlots[i].onLoad(inventoryItems[i]);
        }
    }

    public bool AddItem(InventoryItem newItem)
    {
        if(inventoryItems.Count > maxSize)
        {
            return false;
        }

        // UI Stuff
        inventoryItems.Add(newItem);
        RefreshInventory();

        // Dictionary stuff
        if(playerInventory.ContainsKey(newItem))
        {
            playerInventory[newItem] += newItem.amount;
        }
        else
            playerInventory.Add(newItem, newItem.amount);

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

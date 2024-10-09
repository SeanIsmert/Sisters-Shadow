using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> inventory;
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

        _inventorySlotPrefab = UIManager.instance.getInventorySlot();
        _inventoryCanvas = UIManager.instance.getInventoryPanel();
        _inventoryPanel = UIManager.instance.getInventoryCanvas();
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

        refreshInventory();
    }

    public void refreshInventory()
    {

        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].onLoad(inventory[i]);
        }
    }

    public bool AddItem(InventoryItem newItem)
    {
        if(inventory.Count > maxSize)
        {
            return false;
        }
        inventory.Add(newItem);
        refreshInventory();
        return true;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState state)
    {
        if(state == GameState.Inventory)
            _inventoryCanvas.SetActive(true);
        else
            _inventoryCanvas.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public GameObject panel;
    public GameObject inventorySlotPrefab;
    public List<InventoryItem> inventory;
    public List<InventorySlot> inventorySlots;
    public int maxSize;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.UpdateGameState(GameState.Inventory);
        for (int i = 0; i < maxSize; i++) 
        { 
            var slot = Instantiate(inventorySlotPrefab, panel.transform);
            inventorySlots.Add(slot.GetComponent<InventorySlot>());
        }

        refreshInventory();
    }

    void refreshInventory()
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
        //PlayerInputManager.instance.input.UI 
    }

    private void GameStateChanged(GameState state)
    {
        Debug.Log(state.ToString());
    }
}

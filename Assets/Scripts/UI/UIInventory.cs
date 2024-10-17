using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIInventory : MonoSinglton<UIInventory>
{
    [Header("Inventory Player UI")]
    [SerializeField] private GameObject _inventoryCanvasPlayer;
    [SerializeField] private GameObject _inventoryPanelPlayer;
    [SerializeField] private GameObject _inventorySlotPlayer;
    [Space]

    [Header("Inventory Global UI")]
    [SerializeField] private GameObject _inventoryCanvassGlobal;
    [SerializeField] private GameObject _inventoryPanelsGlobal;
    [SerializeField] private GameObject _inventorySlotsGlobal;    
    // ---------------------------------------------------------------------------------- //
    public GameObject GetInventoryPanel() { return _inventoryPanelPlayer; }
    public GameObject GetInventorySlot() { return _inventorySlotPlayer; }

    private List<InventorySlot> inventorySlots;
    private UnityEvent _inventoryChanged;

    public void GenerateInventory()
    {
        for (int i = 0; i < InventoryManagerPlayer.Instance.GetInventorySize(); i++)
        {
            var slot = Instantiate(_inventorySlotPlayer, _inventoryPanelPlayer.transform);
            inventorySlots.Add(slot.GetComponent<InventorySlot>());
        }
    }

    public void RefreshInventory()
    {
        List<Item> inventory = InventoryManagerPlayer.Instance.GetInvetoryItems();

        foreach (Item item in inventory)
        {
            //if 
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (i < inventory.Count)
            {
                //inventory[i].onLoad(inventory[i].Item, inventory[i].Amount);
            }
            else
            {
                //_inventorySlots[i].onLoad(null, 0);  // Empty slot
            }
        }
    }

    public void UseItem(Item item)
    {
    
    }

    public void DiscardItem(Item item)
    {
    
    }

    private void ToggleUI(GameObject canvas, bool active)
    {
        canvas.SetActive(true);
    }

    private void GameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Inventory:
                ToggleUI(_inventoryCanvasPlayer, true);
                break;
            case GameState.Interactable:
                break;
            default:
                ToggleUI(_inventoryCanvasPlayer, false);
                ToggleUI(_inventoryCanvassGlobal, false);
                break;
        }

    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameStateChanged;
        //_inventoryChanged += RefreshInventory();
    }
}

using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

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
    [Space]

    [Header("Item Slots UI")]
    [SerializeField] private TextMeshProUGUI _itemDescription;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemAmount;
    [SerializeField] private RawImage _itemImage;
    // ---------------------------------------------------------------------------------- //
    public GameObject GetInventoryPanel() { return _inventoryPanelPlayer; }
    public GameObject GetInventorySlot() { return _inventorySlotPlayer; }

    public TextMeshProUGUI GetItemDescription() { return _itemDescription; }
    public TextMeshProUGUI GetItemName() { return _itemName; }
    public TextMeshProUGUI GetItemAmount() { return _itemAmount; }
    public RawImage GetItemImage() { return _itemImage; }

    private List<ItemSlot> _playerInventorySlots;
    private List<Item> _globalInventorySlots;
    private UnityEvent _inventoryChanged;

    public void GenerateInventory()
    {
        for (int i = 0; i < InventoryManager.Instance.GetInventorySize(); i++)
        {
            var slot = Instantiate(_inventorySlotPlayer, _inventoryPanelPlayer.transform);
            _playerInventorySlots.Add(slot.GetComponent<ItemSlot>());
        }
    }

    public void RefreshInventory()
    {
        List<ItemDataToken> playerInventory = InventoryManager.Instance.GetInvetoryItems();
        List<ItemDataToken> globalInventory = InventoryManager.Instance.GetGlobalItems();

        foreach (ItemDataToken item in playerInventory)
        {
            Debug.Log(item.GetBaseItem.itemName);
        }

        foreach (ItemDataToken item in globalInventory)
        {
        
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
        InventoryManager.InventoryChanged += RefreshInventory;
        GameManager.OnGameStateChanged += GameStateChanged;
    }
}

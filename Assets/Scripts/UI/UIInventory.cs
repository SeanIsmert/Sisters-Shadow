using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;
using System;

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
    [SerializeField] private GameObject _inventoryCanvasPreview;
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

    [SerializeField, HideInInspector] private List<ItemSlot> _playerInventorySlots;
    [SerializeField, HideInInspector] private List<ItemSlot> _globalInventorySlots;
    private Action<UnityEngine.InputSystem.InputAction.CallbackContext> _cancelAction;
    private UnityEvent _inventoryChanged;

    public void GenerateInventory()
    {
        for (int i = 0; i < InventoryManager.Instance.GetInventorySize(); i++)
        {
            GameObject slot = Instantiate(_inventorySlotPlayer, _inventoryPanelPlayer.transform);
            _playerInventorySlots.Add(slot.GetComponent<ItemSlot>());
        }
        for (int i = 0; i < 15; i++)
        {
            GameObject slot = Instantiate(_inventorySlotsGlobal, _inventoryPanelsGlobal.transform);
            _globalInventorySlots.Add(slot.GetComponent<ItemSlot>());
        }
    }

    public void RefreshInventory()
    {
        // Get the player's inventory and the global inventory
        List<ItemToken> playerInventory = InventoryManager.Instance.GetInventoryItems();
        List<ItemToken> globalInventory = InventoryManager.Instance.GetGlobalItems();

        // Update player inventory slots
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (i < _playerInventorySlots.Count)
            {
                _playerInventorySlots[i].InList(playerInventory[i]); // Use ItemToken
            }
        }

        // Optional: Clear any remaining slots if the inventory shrinks
        for (int i = playerInventory.Count; i < _playerInventorySlots.Count; i++)
        {
            _playerInventorySlots[i].ClearSlot(); // Clear the slot if no item is assigned
        }

        // Repeat for global inventory if you have global slots to display
        for (int i = 0; i < globalInventory.Count; i++)
        {
            if (i < _globalInventorySlots.Count)
            {
                _globalInventorySlots[i].InList(globalInventory[i]); // Use ItemToken
            }
        }

        // Optional: Clear remaining global slots
        for (int i = globalInventory.Count; i < _globalInventorySlots.Count; i++)
        {
            _globalInventorySlots[i].ClearSlot();
        }
    }

    public void UseItem(ItemToken item)
    {
        
    }

    public void DiscardItem(ItemToken item)
    {
        InventoryManager.Instance.RemoveItem(item);
    }

    private void ToggleUI(GameObject canvas, bool active)
    {
        canvas.SetActive(active);
    }

    private void GameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.InventoryPlayer:
                PlayerInputManager.input.UI.Cancel.started += ctx => GameManager.Instance.UpdateGameState(GameState.Gameplay);
                ToggleUI(_inventoryCanvasPlayer, true);
                ToggleUI(_inventoryCanvasPreview, true);
                ToggleUI(_inventoryCanvassGlobal, false);
                break;
            case GameState.InventoryGlobal:
                PlayerInputManager.input.UI.Cancel.started += ctx => GameManager.Instance.UpdateGameState(GameState.Gameplay);
                ToggleUI(_inventoryCanvasPlayer, true);
                ToggleUI(_inventoryCanvasPreview, false);
                ToggleUI(_inventoryCanvassGlobal, true);
                break;
            default:
                PlayerInputManager.input.UI.Cancel.started -= ctx => GameManager.Instance.UpdateGameState(GameState.Gameplay);
                ToggleUI(_inventoryCanvasPlayer, false);
                ToggleUI(_inventoryCanvasPreview, false);
                ToggleUI(_inventoryCanvassGlobal, false);
                break;
        }
    }

    private void OnEnable()
    {
        _cancelAction = ctx => GameManager.Instance.UpdateGameState(GameState.Gameplay);
        InventoryManager.InventoryChanged += RefreshInventory;
        GameManager.OnGameStateChanged += GameStateChanged;
    }
}

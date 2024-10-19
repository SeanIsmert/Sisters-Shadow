using UnityEngine.UI;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private RawImage _itemImage;

    private TextMeshProUGUI _itemDescriptionPreview;
    private TextMeshProUGUI _itemNamePreview;
    private TextMeshProUGUI _itemAmountPreview;
    private RawImage _itemImagePreview;

    private ItemToken _currentItemToken;

    private void Awake()
    {
        _itemDescriptionPreview = UIInventory.Instance.GetItemDescription();
        _itemNamePreview = UIInventory.Instance.GetItemName();
        _itemAmountPreview = UIInventory.Instance.GetItemAmount();
        _itemImagePreview = UIInventory.Instance.GetItemImage();
    }

    public void SwapItem()
    {
        if (_currentItemToken == null || GameManager.Instance.state == GameState.InventoryPlayer)
            return;

        InventoryManager.Instance.MoveItem(_currentItemToken);
    }

    private bool CheckSlot(ItemBase item)
    {
        if (item == null || _itemName == null || _itemImage == null)
        {
            _itemImage.texture = null;
            _itemName.text = "Empty";

            Debug.Log("Prefab may not be set up correctly, check inventory slot prefab");
            return false;
        }

        return true;
    }

    public void ClearSlot()
    {
        _itemImage.texture = null; // Clear the icon
        _itemName.text = "Empty";  // Clear the name
    }

    public void InList(ItemToken itemToken)
    {
        _currentItemToken = itemToken;
        ItemBase baseItem = itemToken.GetBaseItem;

        if (CheckSlot(baseItem))
        {
            _itemImage.texture = baseItem.icon.texture;
            _itemName.text = baseItem.itemName;
        }
    }

    // Update the preview panel with detailed info
    public void InPreview()
    {
        if (_currentItemToken == null)
            return;

        ItemBase baseItem = _currentItemToken.GetBaseItem;

        _itemDescriptionPreview.text = baseItem.description;
        _itemNamePreview.text = baseItem.itemName;
        _itemImagePreview.texture = baseItem.icon.texture;
        if (baseItem.stackable)
        {
            _itemAmountPreview.text = _currentItemToken.GetItemAmount.ToString();
        }
    }
}

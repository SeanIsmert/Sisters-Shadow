using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemNameElement;      // The UI element for displaying the item's name.
    [SerializeField] private RawImage _itemImageElement;            // The UI element for displaying the item's icon.
    [SerializeField] private Sprite _defaultImage;                  // The image this slot will default to when there is no item.

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
        if (item == null || _itemNameElement == null || _itemImageElement == null)
        {
            _itemImageElement.texture = _defaultImage.texture;
            _itemNameElement.text = "Empty";

            Debug.Log("Prefab may not be set up correctly, check inventory slot prefab");
            return false;
        }

        return true;
    }

    public void ClearSlot()
    {
        _itemImageElement.texture = _defaultImage.texture; // Clear the icon
        _itemNameElement.text = "Empty";  // Clear the name
        _currentItemToken = null;
    }

    public void InList(ItemToken itemToken)
    {
        _currentItemToken = itemToken;
        ItemBase baseItem = itemToken.GetBaseItem;

        if (CheckSlot(baseItem))
        {
            _itemImageElement.texture = baseItem.icon.texture;
            _itemNameElement.text = baseItem.itemName;
        }
    }

    // Update the preview panel with detailed info
    public void InPreview()
    {
        if (_currentItemToken == null)
            return;     

        _itemDescriptionPreview.text = _currentItemToken.GetItemDescription;
        _itemNamePreview.text = _currentItemToken.GetItemName;
        _itemImagePreview.texture = _currentItemToken.GetItemImage.texture;
        _itemAmountPreview.text = _currentItemToken.GetItemAmount.ToString();
        if (!_currentItemToken.GetBaseItem.stackable)
        {
            _itemAmountPreview.gameObject.SetActive(false);
        }
        else
            _itemAmountPreview.gameObject.SetActive(true);

    }
}

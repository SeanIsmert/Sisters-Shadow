using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemNameElement;      // The UI element for displaying the item's name.
    [SerializeField] private Image _itemImageElement;            // The UI element for displaying the item's icon.
    [SerializeField] private Sprite _defaultImage;                  // The image this slot will default to when there is no item.

    private TextMeshProUGUI _itemDescriptionPreview;
    private TextMeshProUGUI _itemNamePreview;
    private TextMeshProUGUI _itemAmountPreview;
    private Image _itemImagePreview;

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
        Debug.Log("Swapping item!");

        if (_currentItemToken == null || GameManager.Instance.state == GameState.InventoryPlayer)
            return;

        InventoryManager.Instance.MoveItem(_currentItemToken);
    }

    private bool CheckSlot(ItemBase item)
    {
        if (item == null || _itemNameElement == null || _itemImageElement == null)
        {
            _itemImageElement.sprite = _defaultImage;
            _itemNameElement.text = "Empty";

            Debug.Log("Prefab may not be set up correctly, check inventory slot prefab");
            return false;
        }

        return true;
    }

    public void ClearSlot()
    {
        _itemImageElement.sprite = _defaultImage; // Clear the icon
        _itemNameElement.text = "Empty";  // Clear the name
        _currentItemToken = null;
    }

    public void InList(ItemToken itemToken)
    {
        _currentItemToken = itemToken;
        ItemBase baseItem = itemToken.GetBaseItem;

        if (CheckSlot(baseItem))
        {
            _itemImageElement.sprite = baseItem.icon;
            _itemNameElement.text = baseItem.itemName;
        }
    }

    // Update the preview panel with detailed info
    public void InPreview()
    {
        Debug.Log("Preview update!");

        if (_currentItemToken == null)
            return;     

        _itemDescriptionPreview.text = _currentItemToken.GetItemDescription;
        _itemNamePreview.text = _currentItemToken.GetItemName;
        _itemImagePreview.sprite = _currentItemToken.GetItemImage;
        _itemAmountPreview.text = _currentItemToken.GetItemAmount.ToString();
        if (!_currentItemToken.GetBaseItem.stackable)
        {
            _itemAmountPreview.gameObject.SetActive(false);
        }
        else
            _itemAmountPreview.gameObject.SetActive(true);

    }
}

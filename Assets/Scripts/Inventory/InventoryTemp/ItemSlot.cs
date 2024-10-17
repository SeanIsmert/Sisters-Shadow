using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private RawImage _itemImage;

    private TextMeshProUGUI _itemDescriptionPreview;
    private TextMeshProUGUI _itemNamePreview;
    private TextMeshProUGUI _itemAmountPreview;
    private RawImage _itemImagePreview;

    private void Start()
    {
        _itemDescriptionPreview = UIInventory.Instance.GetItemDescription();
        _itemNamePreview = UIInventory.Instance.GetItemName();
        _itemAmountPreview = UIInventory.Instance.GetItemAmount();
        _itemImagePreview = UIInventory.Instance.GetItemImage();
    }

    private bool CheckSlot(Item item)
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

    public void InList(Item item)
    {
        _itemImage.texture = item.icon.texture;
        _itemName.text = item.name;
    }

    public void InPreview(ItemDataToken item)
    {
        _itemDescriptionPreview.text = item.GetBaseItem.description;
        _itemNamePreview.text = item.GetBaseItem.itemName;
        _itemAmountPreview.text = item.Amount.ToString();
        _itemImagePreview.texture = item.GetBaseItem.icon.texture;
    }
}

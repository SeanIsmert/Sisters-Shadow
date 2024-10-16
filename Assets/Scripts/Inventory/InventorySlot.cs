using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private RawImage _image;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemAmount;

    public void onLoad(InventoryItem item, uint amount)
    {
        if(item == null || _itemName == null || _image == null || _itemAmount == null)
        {
            _itemName.text = "Empty";
            _itemAmount.text = null;
            _image.texture = null;

            Debug.Log("Prefab may not be set up correctly, check inventory slot prefab");
            return;
        }

        if (item.itemName != null)
        {
            _itemName.text = item.name;
        }
        if (item.multiple)
        {
            _itemAmount.text = amount.ToString();
        }
        else
        {
            _itemAmount.text = string.Empty;
        }
        _image.texture = item.icon.texture;
    }
}

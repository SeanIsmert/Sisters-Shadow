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

    public void onLoad(InventoryItem item)
    {
        if(item == null || _itemName == null || _image == null || _itemAmount == null)
        {
            Debug.Log("Prefab not set up correctly, check inventory slot prefab");
            return;
        }

        if (item.itemName != null)
        {
            _itemName.text = item.name;
        }
        if (item.multiple)
        {
            _itemAmount.text = item.amount.ToString();
        }
        else
        {
            _itemAmount.text = string.Empty;
        }
        _image.texture = item.icon.texture;
    }
}

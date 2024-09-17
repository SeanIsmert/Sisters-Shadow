using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public RawImage img;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemAmount;

    public void onLoad(InventoryItem item)
    {
        if (item.itemName != null)
        {
            itemName.text = item.name;
        }
        if (item.multiple)
        {
            itemAmount.text = item.amount.ToString();
        }
        else
        {
            itemAmount.text = string.Empty;
        }
        img.texture = item.icon;
    }
}

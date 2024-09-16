using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryItem", order = 1)]

public class InventoryItem : ScriptableObject
{
    public string itemName;
    public bool multiple = false;
    public uint amount = 0;
    public Texture2D icon;
}

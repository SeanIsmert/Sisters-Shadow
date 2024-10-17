using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 2)]
public class Item : ScriptableObject
{
    [Header("Initialize")]
    public string itemName;
    [TextArea(4, 8)] public string description;
    public Sprite icon;
    [Space]

    [Header("Settings")]
    public bool consumable;
    public bool stackable;
    public bool keyItem;
    public uint amount;
    public uint maxStack;

    //private ItemToken();
}

#if UNITY_EDITOR
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public Item _curItem;              // Reference to the scriptable object.

    private void OnEnable()
    {
        _curItem = (Item)target;       // Set reference.
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if (_curItem == null || _curItem.icon == null)
            return null;

        Texture2D cache = new(width, height);
        EditorUtility.CopySerialized(_curItem.icon.texture, cache);
        return cache;
    }
}
#endif

[System.Serializable]
public class ItemData
{
    public Item Item { get; private set; }
    public uint Amount { get; set; }

    public ItemData(Item item, uint amount)
    {
        Item = item;
        Amount = amount;
    }
}
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryItem", order = 1)]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    [TextArea(4, 8)] public string description;
    public bool multiple = false;
    public uint amount = 0;
    public Sprite icon;
}

#if UNITY_EDITOR
[CustomEditor(typeof(InventoryItem))]
public class InventoryItemEditor : Editor
{
    public InventoryItem _curItem;              // Reference to the scriptable object.

    private void OnEnable()
    {
        _curItem = (InventoryItem)target;       // Set reference.
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if(_curItem == null || _curItem.icon == null)
            return null;

        Texture2D cache = new(width, height);
        EditorUtility.CopySerialized(_curItem.icon.texture, cache);
        return cache;
    }
}
#endif
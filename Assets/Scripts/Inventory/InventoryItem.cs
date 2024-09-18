using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryItem", order = 1)]

public class InventoryItem : ScriptableObject
{
    public string itemName;
    [TextArea(4, 8)] public string description;
    public bool multiple = false;
    public uint amount = 0;
    public Texture2D icon;
}

// Custom Editor.
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
        if (_curItem == null || _curItem.icon == null)                      // If object or icon are null, do nothing.
            return null;

        Texture2D previewIcon = null;

        while (previewIcon == null)
            previewIcon = AssetPreview.GetAssetPreview(_curItem.icon);      // Set preview reference to item icon.

        Texture2D cache = new(width, height);
        EditorUtility.CopySerialized(previewIcon, cache);                   // Apply width/height.
        return cache;                                                       // Return preview.
    }
}
#endif
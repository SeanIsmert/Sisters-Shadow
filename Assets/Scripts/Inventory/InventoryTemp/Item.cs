using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 2)]
public class Item : DatabaseElement
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
    //public Vector2Int amountRange;
    public uint maxStack;

    //private ItemToken();

    public virtual ItemDataToken GenerateToken()
    {
        return new ItemDataToken(this);
    }
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
public class ItemDataToken
{
    private int _index;
    private uint _amount;

    public Item GetBaseItem { get { return DatabaseContainer.Instance.itemDatabase.elements[_index] as Item; } }
    public uint Amount { get { return _amount; } }

    public string Description { get { return GetBaseItem.description; } }

    public void AddAmount(uint value)
    {
        _amount += value;

        _amount = (uint)Mathf.Clamp(_amount, 0, GetBaseItem.maxStack);
    }

    public ItemDataToken(Item item)
    {
        _index = item.GetIndex;
        _amount = 1;
    }

    public ItemDataToken(Item item, uint amount)
    {
        _index = item.GetIndex;
        _amount = amount;
    }
}
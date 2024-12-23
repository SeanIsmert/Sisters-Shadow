using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 2)]
public class ItemBase : DatabaseElement
{
    [Header("Initialize")]
    public string itemName;
    [TextArea(4, 8)] public string description;
    public Sprite icon;
    [Space]

    public bool consumable;
    public bool stackable;
    public bool keyItem;
    public bool equipable;

    public bool weapon;
    public bool randomRange;
    public uint maxStack;
    public int amount;
    public Vector2Int amountRange;
    public bool singleUse;
    public string keyID;
    public string itemID;

    public virtual ItemToken GenerateToken()
    {
        if (keyItem)
            return new ItemToken(this, keyID);
        else if (stackable)
        {
            if (randomRange)
                amount = (int)Random.Range(amountRange.x, amountRange.y);
            
            return new ItemToken(this, amount);
        }
        else if (equipable)
            return new ItemToken(this);
        else
            return new ItemToken(this);
    }
}

[System.Serializable]
public class ItemToken
{
#region Variables
    private int _index;
    private int _amount;
    private string _key;
    private bool _equiped = false;
    // ------------------------------------------------------------------------------------------------------------------- // Getters
    public ItemBase GetBaseItem { get { return DatabaseContainer.Instance.itemDatabase.elements[_index] as ItemBase; } }
    public string GetItemDescription { get { return GetBaseItem.description; } }
    public string GetItemName { get { return GetBaseItem.name; } }
    public Sprite GetItemImage { get { return GetBaseItem.icon; } }
    public bool GetEquiped { get { return _equiped; } }
    public string GetKeyID { get { return _key; } }
    public int GetItemAmount { get { return _amount; } }
    // ------------------------------------------------------------------------------------------------------------------- // Setters
    public int SetItemAmount { set { _amount = value; } }
    public bool Setequiped { set { _equiped = value; } }
    public string SetKeyID { set { _key = value; } }
#endregion

    public void ChangeValue(int value)
    {
        _amount += value;

        _amount = (int)Mathf.Clamp(_amount, 0, GetBaseItem.maxStack);
    }

#region Token Generation Overrides
    public ItemToken(ItemBase item)
    {
        _index = item.GetIndex;
        _amount = 1;
        _key = "";
    }

    public ItemToken(ItemBase item, int amount)
    {
        _index = item.GetIndex;
        _amount = amount;
        _key = "";
    }
    public ItemToken(ItemBase item, string key)
    {
        _index = item.GetIndex;
        _amount = 1;
        _key = key;
    }
#endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(ItemBase))]
public class ItemEditor : Editor
{
    private ItemBase _curItem;
    private SerializedProperty _itemName, _description, _icon, _consumable, _stackable, _keyItem, _equipable;
    private SerializedProperty _weapon, _randomRange, _maxStack, _amount, _amountRange, _keyID, _singleUse, _itemID;

    private void OnEnable()
    {
        _curItem = (ItemBase)target;  // Set reference to the scriptable object

        // Cache references to serialized properties
        _itemName = serializedObject.FindProperty("itemName");
        _description = serializedObject.FindProperty("description");
        _icon = serializedObject.FindProperty("icon");
        _consumable = serializedObject.FindProperty("consumable");
        _stackable = serializedObject.FindProperty("stackable");
        _keyItem = serializedObject.FindProperty("keyItem");
        _equipable = serializedObject.FindProperty("equipable");
        _weapon = serializedObject.FindProperty("weapon");
        _randomRange = serializedObject.FindProperty("randomRange");
        _maxStack = serializedObject.FindProperty("maxStack");
        _amount = serializedObject.FindProperty("amount");
        _amountRange = serializedObject.FindProperty("amountRange");
        _keyID = serializedObject.FindProperty("keyID");
        _singleUse = serializedObject.FindProperty("singleUse");
        _itemID = serializedObject.FindProperty("itemID");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // General fields
        EditorGUILayout.PropertyField(_itemName);
        EditorGUILayout.PropertyField(_description);
        EditorGUILayout.PropertyField(_itemID);
        EditorGUILayout.PropertyField(_icon);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        if (!_equipable.boolValue && !_keyItem.boolValue)
            EditorGUILayout.PropertyField(_consumable);

        if (!_equipable.boolValue && !_keyItem.boolValue)
            EditorGUILayout.PropertyField(_stackable);

        if (!_consumable.boolValue && !_stackable.boolValue && !_equipable.boolValue)
            EditorGUILayout.PropertyField(_keyItem);

        if (!_consumable.boolValue && !_stackable.boolValue && !_keyItem.boolValue)
            EditorGUILayout.PropertyField(_equipable);

        //EditorGUILayout.Space();
        EditorGUILayout.LabelField("Subsettings", EditorStyles.boldLabel);

        // Show itemID if either consumable or equipable
        if (_equipable.boolValue)
        {
            EditorGUILayout.PropertyField(_weapon);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        // Show keyID if keyItem is true
        if (_keyItem.boolValue)
        {
            EditorGUILayout.PropertyField(_keyID);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_singleUse);
        }

        // Show stack-related fields if stackable is checked
        if (_stackable.boolValue)
        {
            EditorGUILayout.PropertyField(_maxStack);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_randomRange);

            if (_randomRange.boolValue)
            {
                EditorGUILayout.PropertyField(_amountRange);
            }
            else
            {
                EditorGUILayout.PropertyField(_amount);
            }
        }

        serializedObject.ApplyModifiedProperties();
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
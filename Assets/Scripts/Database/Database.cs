using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Database", menuName = "Inventory/Database", order = 1)]
public class Database : ScriptableObject
{
    public DatabaseElement[] elements;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Database))]
public class DatabaseEditor: Editor
{
    private Database _curDatabase;

    private void OnEnable()
    {
        _curDatabase = target as Database;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Set Indexes"))
        {
            for (int i = 0; i < _curDatabase.elements.Length; i++)
            {
                _curDatabase.elements[i].SetIndex(i);
                EditorUtility.SetDirty(_curDatabase.elements[i]);
            }
        }
    }
}
#endif
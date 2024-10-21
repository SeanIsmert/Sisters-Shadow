using GenericUtilities;
using UnityEngine;

[CreateAssetMenu(fileName = "DatabaseContainer", menuName = "Inventory/DatabaseContainer", order = 0)]
public class DatabaseContainer : ScriptableObjectSingleton<DatabaseContainer>
{
    public Database itemDatabase;
}

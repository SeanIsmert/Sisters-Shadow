using GenericUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Database Container")]
public class DatabaseContainer : ScriptableObjectSingleton<DatabaseContainer>
{
    public Database itemDatabase;
}

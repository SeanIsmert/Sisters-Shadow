using UnityEngine;

/// <summary>
/// This class contains the necessary information for the player to save and load info.
/// It is made into a singleton and made accessabile for other classes to refrence.
/// Written by: Sean
/// Modified by: 
/// </summary>
public class PlayerDataManager : MonoBehaviour, IDataPersistence
{
    public int health;
    public int bullets;

    public static PlayerDataManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
        transform.rotation = data.playerRotation;

        health = data.health;
        bullets = data.bullets;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
        data.playerRotation = this.transform.rotation;

        data.health = health;
        data.bullets = bullets;
    }
}

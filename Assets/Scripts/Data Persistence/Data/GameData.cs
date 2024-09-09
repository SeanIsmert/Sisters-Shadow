using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Information to store
    #region Variables
    public SerializableDictionary<string, bool> itemsCollected;
    //public SerializableDictionary<string, DoorState> doorsState;
    public SerializableDictionary<string, AIState> AIStates;


    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public List<string> keys;
    public int health;
    public int bullets;
    #endregion

    //Values defined in this constructor will be the default values without data
    #region Initialize
    public GameData() 
    {
        itemsCollected = new SerializableDictionary<string, bool>();
        //doorsState = new SerializableDictionary<string, DoorState>();
        AIStates = new SerializableDictionary<string, AIState>();
        keys = new List<string>();
        this.playerPosition = Vector3.zero;
        this.playerRotation = Quaternion.identity;
        this.health = 2;
        this.bullets = 4;
    }
    #endregion
}

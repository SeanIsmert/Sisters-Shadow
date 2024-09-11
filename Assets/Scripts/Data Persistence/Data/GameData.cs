using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that stores the base default values for all saved data.
/// is called or overwritten based on save and load data.
/// Written by: Sean
/// Modified by:
/// </summary>
[System.Serializable]
public class GameData
{
    //Information to store
    #region Variables
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
        AIStates = new SerializableDictionary<string, AIState>();
        keys = new List<string>();
        this.playerPosition = Vector3.zero;
        this.playerRotation = Quaternion.identity;
        this.health = 2;
        this.bullets = 4;
    }
    #endregion
}
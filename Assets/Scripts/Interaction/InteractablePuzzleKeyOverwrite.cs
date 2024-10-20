using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePuzzleKeyOverwrite : MonoBehaviour, IInteract
{
    [SerializeField] private string _keytoGrab;
    [SerializeField] private string _newKeyID;
    public Vector3 Position { get { return transform.position; } }

    public void Interaction()
    {
        var token = CheckInventory();
        if (token != null)
            token.SetKeyID = _newKeyID;
    }

    private ItemToken CheckInventory()
    {
        foreach (var token in InventoryManager.Instance.GetInventoryItems()) // Iterate through each item in the player's inventory
        {
            if (!token.GetBaseItem.keyItem) // skip items that arn't key items
                continue;

            if (token.GetKeyID == _keytoGrab) // Match key to key
            {
                return token; // Return the token if a match is found
            }
        }

        return null; // Return null otherwise
    }
}

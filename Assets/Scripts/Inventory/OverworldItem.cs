using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class for overworld items that the player can pick up.
/// To be attached to a 3D model with a trigger of some sort.
/// Requires an inventoryItem scriptable object.
/// Written by: Jack
/// </summary>
public class OverworldItem : MonoBehaviour, IInteract
{
    public InventoryItem onCollect;
    public Vector3 Position { get { return transform.position; } }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Position);
    }


    public void Interaction()
    {
        Debug.Log("interacted with item!");
        if (InventoryManager.instance.AddItem(onCollect))
        {
            InteractableManager.instance.RemoveTarget(this);
            Debug.Log("picked up " + onCollect.itemName);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("inventory full!");
        }
    }
    
    
}

using UnityEngine;
/// <summary>
/// Class for overworld items that the player can pick up.
/// To be attached to a 3D model with a trigger of some sort.
/// Requires an inventoryItem scriptable object.
/// Written by: Jack
/// </summary>
public class OverworldItem : MonoBehaviour, IInteract
{
    [SerializeField] private InventoryItem onCollect;
    public Vector3 Position { get { return transform.position; } }

    public void Interaction()
    {
        Debug.Log("interacted with item!");
        if (PlayerInventory.instance.AddItem(onCollect))
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

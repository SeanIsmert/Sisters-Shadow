using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteract
{
    [SerializeField] private ItemBase _item;
    public Vector3 Position { get { return transform.position; } }

    public void Interaction()
    {
        if (InventoryManager.Instance.AddItem(_item))
        {
            InteractableManager.Instance.RemoveTarget(this);
            Debug.Log("picked up " + _item.itemName);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("inventory full!");
        }
    }
    
    
}

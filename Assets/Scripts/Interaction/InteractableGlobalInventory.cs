using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGlobalInventory : MonoBehaviour, IInteract
{
    public Vector3 Position { get { return transform.position; } }

    public void Interaction()
    {
        GameManager.Instance.UpdateGameState(GameState.InventoryGlobal);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldItem : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public InventoryItem onCollect;
    bool interactable = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!interactable)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (inventoryManager.AddItem(onCollect))
            {
                Debug.Log("picked up " +  onCollect.itemName);
            }
            else
            {
                Debug.Log("inventory full!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactable = false;
        }
    }
}

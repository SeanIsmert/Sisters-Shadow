using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject panel;
    public GameObject inventorySlotPrefab;
    public InventoryItem[] inventory;
    public List<InventorySlot> inventorySlots;
    public int maxSize;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxSize; i++) 
        { 
            var slot = Instantiate(inventorySlotPrefab, panel.transform);
            inventorySlots.Add(slot.GetComponent<InventorySlot>());
        }

        for (int i = 0;i < inventory.Length; i++)
        {
            inventorySlots[i].onLoad(inventory[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class UIInventory : MonoSinglton<UIInventory>
{
    [Header("Inventory UI")]
    [SerializeField] private GameObject _inventoryCanvas;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventorySlot;
    // ---------------------------------------------------------------------------------- //
    public GameObject GetInventoryCanvas() { return _inventoryCanvas; }
    public GameObject GetInventoryPanel() { return _inventoryPanel; }
    public GameObject GetInventorySlot() { return _inventorySlot; }

    public void OpenUI()
    {
        _inventoryCanvas.SetActive(true);
    }

    public void CloseUI()
    {
        _inventoryCanvas.SetActive(false);
    }
}

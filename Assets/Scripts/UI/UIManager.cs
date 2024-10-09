using UnityEngine.UI;
using UnityEngine;
using TMPro;

/// <summary>
/// Manager script intended to handle any UI elements displayed during regular gameplay.
/// This includes things like health, indicators,ammo, Dialogue, menus, etc.
/// @author Kay.
/// @modified by Sean
/// </summary>
public class UIManager : MonoBehaviour
{
#region Variables
#region Menu
    [Header("Menu UI")]
    [SerializeField] private GameObject _menuCanvas;
    // ---------------------------------------------------------------------------------- //

    [Space]
#endregion

#region Save
    [Header("Save UI")]
    [SerializeField] private GameObject _saveCanvas;
    // ---------------------------------------------------------------------------------- //

    [Space]
#endregion

#region Lose
    [Header("Lose UI")]
    [SerializeField] private GameObject _loseCanvas;
    // ---------------------------------------------------------------------------------- //

    [Space]
#endregion

#region Win
    [Header("Win UI")]
    [SerializeField] private GameObject _winCanvas;
    // ---------------------------------------------------------------------------------- //

    [Space]
#endregion

#region Info
    [Header("Info UI")]
    [SerializeField] private TextMeshProUGUI _infoTextField;
    [SerializeField] private Transform _infoImageTransform;
    [SerializeField] private GameObject _infoCanvas;
    // ---------------------------------------------------------------------------------- //
    public TextMeshProUGUI getInfoTextField() { return _infoTextField; }
    public Transform getInfoImageTransform() { return _infoImageTransform; }
    public GameObject getInfoCanvas() { return _infoCanvas; }
    [Space]
#endregion

#region Inspect
    [Header("Inspect UI")]
    [SerializeField] private TextMeshProUGUI _inspectTextField;
    [SerializeField] private GameObject _inspectCanvas;
    // ---------------------------------------------------------------------------------- //
    public TextMeshProUGUI getInspectTextField() { return _inspectTextField; }
    public GameObject getInspectCanvas() { return _inspectCanvas; }
    [Space]
#endregion

#region Dialogue
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI _dialogueTextField;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] private Transform _responsePanle;
    [SerializeField] private Button _responseButton;
    // ---------------------------------------------------------------------------------- //
    public TextMeshProUGUI getDialogueTextField() { return _dialogueTextField; }
    public GameObject getDialogueCanvas() { return _dialogueCanvas; }
    public Transform getResponsePanle() { return _responsePanle; }
    public Button getResponseButton() { return _responseButton; }
    [Space]
#endregion

#region Inventory
    [Header("Inventory UI")]
    [SerializeField] private GameObject _inventoryCanvas;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventorySlot;
    // ---------------------------------------------------------------------------------- //
    public GameObject getInventoryCanvas() { return _inventoryCanvas; }
    public GameObject getInventoryPanel() { return _inventoryPanel; }
    public GameObject getInventorySlot() { return _inventorySlot; }

    [Space]
#endregion

#region Player
    [Header("Player UI")]
    [SerializeField] private Image _healthCircle;
#endregion

    public static UIManager instance;
#endregion

#region Initialize
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _healthCircle.color = new Color(1f, 1f, 1f, 0f);
    }
    #endregion

#region CodeBase
    public void UpdateHealthUI(float health)
    {
        float imageAlpha = (255f - (health * 255f)) / (255f - 0f);      // Reverse normalization wizardry.

        _healthCircle.color = new Color(1f, 1f, 1f, imageAlpha);        // Apply result to image alpha value.
    }
#endregion
}

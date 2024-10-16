using UnityEngine;
using TMPro;

public class UIInspect : UIManager
{
    [Header("Inspect UI")]
    [SerializeField] private TextMeshProUGUI _inspectTextField;
    [SerializeField] private GameObject _inspectCanvas;
    // ---------------------------------------------------------------------------------- //
    public TextMeshProUGUI GetInspectTextField() { return _inspectTextField; }
    public GameObject GetInspectCanvas() { return _inspectCanvas; }
}

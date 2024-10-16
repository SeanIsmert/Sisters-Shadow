using UnityEngine;
using TMPro;

public class UIInfo : UIManager
{
    [Header("Info UI")]
    [SerializeField] private TextMeshProUGUI _infoTextField;
    [SerializeField] private Transform _infoImageTransform;
    [SerializeField] private GameObject _infoCanvas;
    // ---------------------------------------------------------------------------------- //
    public TextMeshProUGUI GetInfoTextField() { return _infoTextField; }
    public Transform GetInfoImageTransform() { return _infoImageTransform; }
    public GameObject GetInfoCanvas() { return _infoCanvas; }
}

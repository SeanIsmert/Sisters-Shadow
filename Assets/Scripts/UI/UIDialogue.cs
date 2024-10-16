using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIDialogue : MonoSinglton<UIDialogue>
{
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI _dialogueTextField;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] private Transform _responsePanle;
    [SerializeField] private Button _responseButton;
    // ---------------------------------------------------------------------------------- //
    public TextMeshProUGUI GetDialogueTextField() { return _dialogueTextField; }
    public GameObject GetDialogueCanvas() { return _dialogueCanvas; }
    public Transform GetResponsePanle() { return _responsePanle; }
    public Button GetResponseButton() { return _responseButton; }

    public void OpenUI()
    {
        _dialogueCanvas.SetActive(true);
    }

    public void CloseUI()
    {
        _dialogueCanvas.SetActive(false);
    }
}

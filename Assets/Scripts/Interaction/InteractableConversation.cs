using UnityEngine.UI;
using UnityEngine;
using XNode;
using TMPro;
using System.Collections;
using UnityEditor.Rendering;
using static System.Net.Mime.MediaTypeNames;

public class InteractableConversation : MonoBehaviour, IInteract
{
    [Tooltip("The Scriptable Object that holds the XNode Dialogue graph")]
    [SerializeField] private DialogueGraph _conversationGraph;
    [Space]

    [Tooltip("The string that all NPC Dialogue will be spoken on.")]
    [SerializeField] private TextMeshProUGUI _spokenLine;
    [Tooltip("The Scriptable Object that holds the XNode Dialogue graph")]
    [SerializeField] private Transform _responsePanle;
    [Tooltip("The Scriptable Object that holds the XNode Dialogue graph")]
    [SerializeField] private GameObject _buttonPrefab;

    //for now
    [SerializeField] private GameObject _dialogueObject;

    private Coroutine _textTyping;

    public Vector3 Position { get { return transform.position; } }


    public void Interaction()
    {
        GameManager.instance.UpdateGameState(GameState.Dialogue);
        _dialogueObject.SetActive(true);

        foreach (Node node in _conversationGraph.nodes)
        {
            if (node is EntryNode)
            {
                _conversationGraph.current = node.GetPort("exit").Connection.node as CoreNodeBase;
                break;
            }
        }

        ParseNode();
    }

    //what to do with the node information
    private void ParseNode()
    {
        if (_conversationGraph.current == null || _spokenLine == null)
            return;

        switch (_conversationGraph.current.GetNodeType) 
        {
            case "NPC":
                IDialogue dialogue = (IDialogue)_conversationGraph.current;
                _textTyping = StartCoroutine(TextTyping(dialogue.TextField));
                break;
            case "Response":
                ClearButtons();
                NextNode("exit");
                break;
            case "ActiveEvent":
                (_conversationGraph.current as SetActive)?.ExecuteEvent();
                NextNode("exit");
                break;
            case "Exit":
                ClearButtons();
                ExitConversation();
                break;
        }
        
    }

    public void NextNode(string fieldName)
    {
        foreach (NodePort port in _conversationGraph.current.Ports)
        {
            if (port.fieldName == fieldName)
            {
                _conversationGraph.current = port.Connection.node as CoreNodeBase;
                break;
            }
        }
        ParseNode();
    }

    public void ExitConversation()
    {
        GameManager.instance.UpdateGameState(GameState.Gameplay);

        _dialogueObject.SetActive(false);

    }

    private void SpawnResponseButtons(string text)
    {
        PlayerInputManager.input.UI.Submit.started -= ctx => { StopCoroutine(_textTyping); SetText(_spokenLine, text); SpawnResponseButtons(text); };

        foreach (NodePort port in _conversationGraph.current.Ports)
        {
            if (port.Connection == null || port.IsInput)
                continue;

            if (port.Connection.node is IDialogue)
            {
                IDialogue response = port.Connection.node as IDialogue;

                Button button = Instantiate(_buttonPrefab, _responsePanle).GetComponent<Button>();

                button.onClick.AddListener(() => NextNode(port.fieldName));
                SetText(button.GetComponentInChildren<TextMeshProUGUI>(), response.TextField);
            }
            else if (port.Connection.node is ExitNode)
                NextNode(port.fieldName);
        }
    }

    private void SetText(TMP_Text t, string d)
    {
        t.text = d;
    }

    private void ClearButtons()
    {
        Transform[] children = _responsePanle.GetComponentsInChildren<Transform>();

        for (int i = children.Length - 1; i > 0; i--)
        {
            if (children[i] != _responsePanle)
            {
                Destroy(children[i].gameObject);
            }
        }
    }

    private IEnumerator TextTyping(string text)
    {
        PlayerInputManager.input.UI.Submit.started += ctx => { StopCoroutine(_textTyping); SetText(_spokenLine, text); SpawnResponseButtons(text); };

        string currentText = null;
        for (int i = 0; i < text.Length; i++)
        {
            currentText += text[i];
            _spokenLine.text = currentText;
            yield return new WaitForSeconds(.1f);
        }

        SpawnResponseButtons(text);
        _textTyping = null;
        yield return null;
    }

    private void OnEnable()
    {
        PlayerInputManager.input.UI.Cancel.performed += ctx => { ClearButtons(); ExitConversation(); };
    }

    private void OnDisable()
    {
        PlayerInputManager.input.UI.Cancel.performed -= ctx => { ClearButtons(); ExitConversation(); };
    }
}

using UnityEngine.UI;
using UnityEngine;
using XNode;
using TMPro;

public class InteractableConversation : MonoBehaviour, IInteract
{
    [SerializeField] private DialogueGraph _conversationGraph;
    [SerializeField] private TextMeshProUGUI _spokenLine;
    [SerializeField] private Transform _responsePanle;
    [SerializeField] private GameObject _buttonPrefab;

    //for now
    [SerializeField] private GameObject _dialogueObject;

    public Vector3 Position { get { return transform.position; } }


    public void Interaction()
    {
        GameManager.instance.UpdateGameState(GameState.Dialogue);
        _dialogueObject.SetActive(true);

        //On start, we need to know the first node (luckily we have an EntryNode)
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
                _spokenLine.text = (_conversationGraph.current as NPCDialogue)?.dialogueSpoken;
                SpawnResponseButtons();
                break;
            case "Response":
                ClearButtons();
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

    private void SpawnResponseButtons()
    {
        foreach (NodePort port in _conversationGraph.current.Ports)
        {
            if (port.Connection == null || port.IsInput)
                continue;

            if (port.Connection.node is ResponseDialogue)
            {
                ResponseDialogue rd = port.Connection.node as ResponseDialogue;

                Button b = Instantiate(_buttonPrefab, _responsePanle).GetComponent<Button>();

                b.onClick.AddListener(() => NextNode(port.fieldName));
                b.GetComponentInChildren<TextMeshProUGUI>().text = rd.dialogueSpoken.ToString();
            }
        }
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

    private void OnEnable()
    {
        PlayerInputManager.instance.input.UI.Cancel.performed += ctx => { ClearButtons(); ExitConversation(); };
    }

    private void OnDisable()
    {
        PlayerInputManager.instance.input.UI.Cancel.performed -= ctx => { ClearButtons(); ExitConversation(); };
    }
}

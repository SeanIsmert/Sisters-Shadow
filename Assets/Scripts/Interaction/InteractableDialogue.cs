using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using XNode;
using TMPro;

public class InteractableDialogue : MonoBehaviour, IInteract
{
#region Variables
    [Header("DialogueTree")]
    [Tooltip("The Scriptable Object that holds the XNode Dialogue graph")]
    [SerializeField] private DialogueGraph _dialogueGraph;
    [Space]

    [Header("Events")]
    [SerializeField] private GameObject[] objectsToChange;
    [Space]

    private TextMeshProUGUI _dialogueTextField;
    private GameObject _dialogueCanvas;
    private Transform _responsePanle;
    private Button _responseButton;
    private Coroutine _animateText;

    private string _currentText;
    public Vector3 Position { get { return transform.position; } }
    #endregion

#region Initialize
    private void Start()
    {
        
    }

    public void Interaction()
    {
        PlayerInputManager.input.UI.Cancel.performed += ctx => Exit();
        GameManager.instance.UpdateGameState(GameState.Dialogue);
        _dialogueCanvas.SetActive(true);

        foreach (Node node in _dialogueGraph.nodes)
        {
            if (node is EntryNode)
            {
                _dialogueGraph.current = node.GetPort("exit").Connection.node as CoreNodeBase;
                break;
            }
        }

        ParseNode();
    }

    public void Exit()
    {
        PlayerInputManager.input.UI.Cancel.performed -= ctx => Exit();
        GameManager.instance.UpdateGameState(GameState.Gameplay);
        _dialogueCanvas.SetActive(false);
    }
#endregion

#region CodeBase
    private void ParseNode()
    {
        if (_dialogueGraph.current == null)
            return;

        switch (_dialogueGraph.current.GetNodeType)
        {
            case "Dialogue": // Your NPC Speaks his line of text
                IDialogue dialogue = (IDialogue)_dialogueGraph.current;
                _animateText = StartCoroutine(AnimatedText(dialogue.TextField, dialogue.AnimateSpeed));
                break;
            case "Response": // The Player Responds using buttons
                NextNode("exit");
                break;
            case "ActiveEvent": // Trigger an Event for setting game objects actives
                (_dialogueGraph.current as SetActive)?.ExecuteEvent(objectsToChange);
                NextNode("exit");
                break;
            case "HealthCheck": // Run the condition that checks health
                NextNode("exit");
                break;
            case "Exit": // The Exit node that dictates we have reached the end of the tree
                WipeResponse();
                Exit();
                break;
        }
    }

    private void NextNode(string fieldName)
    {
    
    }

#region Logic
    private void SetText(TMP_Text textField, string text)
    {
        textField.text = text;
    }

    /// <summary>
    /// Spawns our response buttons so the player can navigate the dialogue graph.
    /// Includes a check for exitNodes when leaving
    /// </summary>
    private void Response()
    {
        foreach (NodePort port in _dialogueGraph.current.Ports)
        {
            if (port.Connection == null || port.IsInput) // Check to see if our has no connection or IsInput, if so skip this node
                continue;

            if (port.Connection.node is IDialogue) // If our node has IDialogue and stores text information or if they lead to the exit node
            {
                IDialogue response = port.Connection.node as IDialogue; // Grab the information from the node

                Button button = Instantiate(_responseButton, _responsePanle).GetComponent<Button>(); // Instantiate our button
                button.onClick.AddListener(() => { NextNode(port.fieldName); WipeResponse(); }); // Subscribe the button to NextNode() and pass the port name

                SetText(button.GetComponentInChildren<TextMeshProUGUI>(), response.TextField); // In the buttons text area set our IDialogue TextField
            }
            else if (port.Connection.node is ExitNode) // Check if our button leads to an exit 
            {
                NextNode(port.fieldName); // Send it off with the fieldName
            }
        }
    }

    /// <summary>
    /// The method responsible for clearing buttons away
    /// </summary>
    private void WipeResponse()
    {
        Transform[] responses = _responsePanle.GetComponentsInChildren<Transform>(); // Grab all our response buttons

        for (int i = responses.Length - 1; i > 0; i--) // Iterate through them using a inverse for loop
        {
            if (responses[i] != _responsePanle) // I really don't know why this logic is the way it is, but it works
                Destroy(responses[i].gameObject);
        }
    }
    #endregion
    #endregion

    #region Animation
    private IEnumerator AnimatedText(string text, float scrollSpeed)
    {
        PlayerInputManager.input.UI.Submit.started += ctx => Subscription(); // Subscribe to allow skipping of Animation
        string currentText = null;

        for (int i = 0; i < text.Length; i++) // For loop which "Animates" our text on screen
        {
            currentText += text[i];
            _dialogueTextField.text = currentText;
            yield return new WaitForSeconds(scrollSpeed);
        }

        PlayerInputManager.input.UI.Submit.started -= ctx => Subscription(); // Unsubscribe to mitigate odd behavior
        _animateText = null;

        Response();

        yield return null;
    }

    private void Subscription()
    {
        if (_animateText != null)
        {
            StopCoroutine(_animateText); _animateText = null;
            //SetText(_spokenLine, _currentText);
            //SpawnResponseButtons(text);
        }
    }
#endregion
}

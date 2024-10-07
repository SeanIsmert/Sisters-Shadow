using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
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
    [Tooltip("The Game Objects In scene that will change when hitting the set active node")]
    [SerializeField] private GameObject[] objectsToChange;
    [Space]

    [Header("Initalize")]
    [Tooltip("The Game Objects that holds the canvas for NPC dialogue")]
    [SerializeField] private GameObject _dialogueCanvas;
    [Tooltip("The Prefab button for all responses")]
    [SerializeField] private Button _responseButton;

    private Action<InputAction.CallbackContext> _exitAction;
    private TextMeshProUGUI _dialogueTextField;
    private Transform _responsePanle;
    private Coroutine _animateText;

    private string _currentText;
    public Vector3 Position { get { return transform.position; } }
    #endregion

#region Initialize
    private void Start()
    {
        _dialogueTextField = _dialogueCanvas.GetComponentInChildren<TextMeshProUGUI>();
        _responsePanle = _dialogueCanvas.GetComponentInChildren<VerticalLayoutGroup>().transform;
    }

    /// <summary>
    /// Upon triggering the conversation start here
    /// </summary>
    public void Interaction()
    {
        _exitAction = ctx => Exit(); PlayerInputManager.input.UI.Cancel.performed += _exitAction; // Subscribe to be able to leave dialogue
        GameManager.instance.UpdateGameState(GameState.Dialogue); // Set your game state to ensure no moving and button usability
        _dialogueCanvas.SetActive(true); // Turn on the canvas so you can see the dialogue

        foreach (Node node in _dialogueGraph.nodes) // Search for the entry node to begin traversing the tree
        {
            if (node is EntryNode)
            {
                _dialogueGraph.current = node.GetPort("exit").Connection.node as CoreNodeBase;
                break;
            }
        }

        ParseNode(); // Start talking
    }

    /// <summary>
    /// The method for turning off and leaving dialogue
    /// </summary>
    public void Exit()
    {
        Subscription(new InputAction.CallbackContext());
        WipeResponse();

        PlayerInputManager.input.UI.Cancel.performed -= _exitAction; // Unsubscribe from your quit dialogue buttons
        GameManager.instance.UpdateGameState(GameState.Gameplay); // Send the player back to Gameplay
        _dialogueCanvas.SetActive(false); // Turn off the dialogue visuals
    }
#endregion

#region CodeBase
    private void ParseNode()
    {
        if (_dialogueGraph.current == null) // Simple check to see if the current node is null, for some reason...
            return;

        switch (_dialogueGraph.current.GetNodeType)
        {
            case "Dialogue": // Your NPC Speaks his line of text
                IDialogue dialogue = (IDialogue)_dialogueGraph.current;
                _animateText = StartCoroutine(AnimatedText(dialogue.TextField, dialogue.AnimateSpeed));
                break;
            case "Response": // The Player Responds using buttons
                WipeResponse();
                NextNode("exit");
                break;
            case "ActiveEvent": // Trigger an Event for setting game objects actives
                (_dialogueGraph.current as SetActive)?.ExecuteEvent(objectsToChange);
                NextNode("exit");
                break;
            case "HealthCheck": // Run the condition that checks health
                string fieldName = (_dialogueGraph.current as HealthCheck)?.PortOnCondtion();
                NextNode(fieldName);
                break;
            case "Exit": // The Exit node that dictates we have reached the end of the tree
                WipeResponse();
                Exit();
                break;
        }
    }

    /// <summary>
    /// Reads the ports fieldName and makes sure you are going along the correct path.
    /// </summary>
    private void NextNode(string fieldName)
    {
        foreach (NodePort port in _dialogueGraph.current.Ports) // Checks every port
        {
            if (port.fieldName == fieldName) // matches the port names
            {
                _dialogueGraph.current = port.Connection.node as CoreNodeBase; // moves the current node to the next node
                break;
            }
        }

        ParseNode();
    }
#region Logic
    /// <summary>
    /// Simply sets the text component that was passed in with a string passed in.
    /// </summary>
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

            if (port.Connection.node is NPCDialogue)
            {
                NextNode(port.fieldName);
            }
            else if (port.Connection.node is IDialogue) // If our node has IDialogue and stores text information or if they lead to the exit node
            {
                IDialogue response = port.Connection.node as IDialogue; // Grab the information from the node

                Button button = Instantiate(_responseButton, _responsePanle).GetComponent<Button>(); // Instantiate our button
                button.onClick.AddListener(() => NextNode(port.fieldName)); // Subscribe the button to NextNode() and pass the port name

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
    /// <summary>
    /// This enumerator "Animates" text so that it looks cool.
    /// Has the ability to pass in both text and speed for dynamic text interactions.
    /// </summary>
    private IEnumerator AnimatedText(string text, float scrollSpeed)
    {


        PlayerInputManager.input.UI.Submit.started += Subscription; // Subscribe to allow skipping of Animation
        string currentText = null;

        for (int i = 0; i < text.Length; i++) // For loop which "Animates" our text on screen
        {
            currentText += text[i];
            _dialogueTextField.text = currentText;
            yield return new WaitForSeconds(scrollSpeed);
        }

        PlayerInputManager.input.UI.Submit.started -= Subscription; // Unsubscribe to mitigate odd behavior
        _animateText = null;

        Response(); // spawn our response options

        yield return null;
    }

    /// <summary>
    /// How the animated text subscribes and unsubscribes to It's input.
    /// I was unable to handle it in the Enumerator due to lambda expression limitations.
    /// Instead this will have to do, no complaining.
    /// </summary>
    private void Subscription(InputAction.CallbackContext ctx)
    {
        if (_animateText != null)
        {
            IDialogue dialogue = (IDialogue)_dialogueGraph.current; // Store our current nodes final text read

            StopCoroutine(_animateText); _animateText = null; //Stop the current logic
            SetText(_dialogueTextField, dialogue.TextField); // display our current nodes final text read
            Response(); // spawn the response buttons to continue navigation through dialogue
        }
    }
#endregion
}

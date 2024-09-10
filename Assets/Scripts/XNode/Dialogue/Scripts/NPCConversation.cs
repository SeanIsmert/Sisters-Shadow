using UnityEngine.UI;
using UnityEngine;
using XNode;
using TMPro;

public class NPCConversation : MonoBehaviour
{
    public DialogueGraph dialogue;
    public TextMeshProUGUI spokenLine;
    public Transform responsePanle;
    public GameObject buttonPrefab;

    private void Start()
    {
        //On start, we need to know the first node (luckily we have an EntryNode)
        foreach (Node item in dialogue.nodes)
        {
            if (item is EntryNode)
            { 
                //Issue? we only store current as a DialogueNodeBase in out DialogueGraph script
                //our entry node is a coreNode, but luckily our Entry node leads to only one DialogueNodeBase anyway
                //lets access that
                dialogue.current = item.GetPort("exit").Connection.node as DialogueNodeBase;
                break;
            }
        }

        ParseNode();
    }

    //what to do with the node information
    private void ParseNode()
    {
        if (dialogue.current == null || spokenLine == null)
            return;

        //Now I need to know the kind of node im dealing with
        //remember our getDialogueType we made in our DialogueNodeBase
        switch (dialogue.current.GetDialogueType) 
        {
            case "NPC":
                spokenLine.text = dialogue.current.dialogueSpoken;
                SpawnResponseButtons();
                break;
            case "Response":
                ClearButtons();
                NextNode("exit");
                break;
        }
    }

    public void NextNode(string fieldName)
    {
        foreach (NodePort port in dialogue.current.Ports)
        {
            if (port.fieldName == fieldName)
            {
                dialogue.current = port.Connection.node as DialogueNodeBase;
                break;
            }
        }
        ParseNode();
    }

    private void SpawnResponseButtons()
    {
        foreach (NodePort port in dialogue.current.Ports)
        {
            if (port.Connection == null || port.IsInput)
                continue;

            if (port.Connection.node is ResponseDialogue)
            {
                ResponseDialogue rd = port.Connection.node as ResponseDialogue;

                Button b = Instantiate(buttonPrefab, responsePanle).GetComponent<Button>();

                b.onClick.AddListener(() => NextNode(port.fieldName));
                b.GetComponentInChildren<TextMeshProUGUI>().text = rd.dialogueSpoken.ToString();
            }
        }
    }

    private void ClearButtons()
    {
        Transform[] children = responsePanle.GetComponentsInChildren<Transform>();

        for (int i = children.Length - 1; i > 0; i--)
        {
            if (children[i] != responsePanle)
            {
                Destroy(children[i].gameObject);
            }
        }
    }
}

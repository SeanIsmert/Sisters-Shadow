using System.Collections;
using System.Collections.Generic;
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
        foreach (Node item in dialogue.nodes)
        {
            if (item is EntryNode)
            { 
                dialogue.current = item.GetPort("exit").Connection.node as DialogueNodeBase;
                break;
            }
        }

        ParseNode();
    }

    private void ParseNode()
    {
        if (dialogue.current == null || spokenLine == null)
            return;

        switch (dialogue.current.GetDialogueType) 
        {
            case "NPC":
                spokenLine.text = dialogue.current.dialogueSpoken;
                //spawn
                break;
            case "Response":
                //clear buttons and move to next node
                break;
        }
    }
}

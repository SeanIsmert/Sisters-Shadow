using UnityEngine;
using XNode;

public abstract class DialogueNodeBase : Node 
{
    [TextArea]
    public string dialogueSpoken;

    public abstract string GetDialogueType { get; }
}
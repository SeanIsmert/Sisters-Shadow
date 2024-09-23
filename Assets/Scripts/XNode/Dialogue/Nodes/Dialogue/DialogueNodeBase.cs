using UnityEngine;
using XNode;

public abstract class DialogueNodeBase : Node, IDialogue
{
    [TextArea]
    public string dialogueSpoken;

    public abstract string GetDialogueType { get; }

    public string TextField { get { return dialogueSpoken; } }
}
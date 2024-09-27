using UnityEngine;
using XNode;

public abstract class DialogueNodeBase : CoreNodeBase, IDialogue
{
    [TextArea]
    public string dialogueSpoken;

    public string TextField { get { return dialogueSpoken; } }
}
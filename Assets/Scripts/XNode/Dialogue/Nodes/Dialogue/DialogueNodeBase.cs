using UnityEngine;
using XNode;

public abstract class DialogueNodeBase : CoreNodeBase, IDialogue
{
    [TextArea]
    public string dialogueSpoken;
    [Range(0.0001f,2f)]
    public float animateSpeed;

    public string TextField { get { return dialogueSpoken; } }
    public float AnimateSpeed { get { return animateSpeed; } }
}
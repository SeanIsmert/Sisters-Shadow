using UnityEngine;

public class NPCDialogue : CoreNodeBase, IDialogue
{
    [TextArea]
    public string dialogueSpoken;

    [Input(typeConstraint = TypeConstraint.Strict)] 
    public bool entry;
    [Output(dynamicPortList = true, connectionType = ConnectionType.Override)] 
    public int exit;

    public override string GetNodeType { get { return "NPC"; } }

    public string TextField { get { return dialogueSpoken; } }
}
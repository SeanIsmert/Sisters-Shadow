using UnityEngine;

public class ResponseDialogue : CoreNodeBase, IDialogue
{
    [TextArea]
    public string dialogueSpoken;

    [Input(typeConstraint = TypeConstraint.Strict)] 
    public int entry;
    [Output(connectionType = ConnectionType.Override)] 
    public bool exit;

    public override string GetNodeType { get { return "Response"; } }

    public string TextField { get { return dialogueSpoken; } }
}
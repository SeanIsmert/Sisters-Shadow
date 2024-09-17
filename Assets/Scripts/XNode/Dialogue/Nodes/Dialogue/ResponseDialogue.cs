using UnityEngine;

public class ResponseDialogue : CoreNodeBase 
{
    [TextArea]
    public string dialogueSpoken;

    [Input(typeConstraint = TypeConstraint.Strict)] 
    public int entry;
    [Output(connectionType = ConnectionType.Override)] 
    public bool exit;

    public override string GetNodeType { get { return "Response"; } }
}
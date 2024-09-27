using UnityEngine;

public class ResponseDialogue : DialogueNodeBase, IDialogue
{
    [Input(typeConstraint = TypeConstraint.Strict)] 
    public int enter;
    [Output(connectionType = ConnectionType.Override)] 
    public bool exit;

    public override string GetNodeType { get { return "Response"; } }
}
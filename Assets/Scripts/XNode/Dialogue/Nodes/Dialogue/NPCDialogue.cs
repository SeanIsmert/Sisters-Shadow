using UnityEngine;

public class NPCDialogue : DialogueNodeBase, IDialogue
{
    [Input] 
    public bool enter;
    [Output(dynamicPortList = true, connectionType = ConnectionType.Override)] 
    public int exit;

    public override string GetNodeType { get { return "Dialogue"; } }
}
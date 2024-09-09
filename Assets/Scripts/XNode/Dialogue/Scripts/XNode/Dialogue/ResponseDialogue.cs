using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ResponseDialogue : DialogueNodeBase 
{
    [Input(typeConstraint = TypeConstraint.Strict)] 
    public int entry;
    [Output(connectionType = ConnectionType.Override)] 
    public bool exit;

    public override string GetDialogueType { get { return "Response"; } }

}
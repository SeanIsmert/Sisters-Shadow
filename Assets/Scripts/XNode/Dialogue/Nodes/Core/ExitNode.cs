using UnityEngine;

public class ExitNode : CoreNodeBase 
{
    [Input] public bool enter;

    public override string GetNodeType { get { return "Exit"; } }
}
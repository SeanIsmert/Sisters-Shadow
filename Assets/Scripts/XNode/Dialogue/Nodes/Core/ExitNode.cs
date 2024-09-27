using UnityEngine;

public class ExitNode : CoreNodeBase 
{
    [Input] public int enter;

    public override string GetNodeType { get { return "Exit"; } }
}
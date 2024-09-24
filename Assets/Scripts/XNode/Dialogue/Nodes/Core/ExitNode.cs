using UnityEngine;

public class ExitNode : CoreNodeBase 
{
    [Input] public int entry;

    public override string GetNodeType { get { return "Exit"; } }
}
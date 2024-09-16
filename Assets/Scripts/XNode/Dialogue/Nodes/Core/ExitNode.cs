using UnityEngine;

public class ExitNode : CoreNodeBase 
{
    [Input] public int entry;

    public override string GetCoreType { get { return "Exit"; } }
}
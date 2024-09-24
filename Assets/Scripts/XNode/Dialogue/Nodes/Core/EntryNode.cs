public class EntryNode : CoreNodeBase 
{
    [Output(connectionType = ConnectionType.Override)] 
    public bool exit;

    public override string GetNodeType { get { return "Entry"; } }
}
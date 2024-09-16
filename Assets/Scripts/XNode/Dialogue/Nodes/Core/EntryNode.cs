public class EntryNode : CoreNodeBase 
{
    [Output(connectionType = ConnectionType.Override)] public bool exit;

    public override string GetCoreType { get { return "Entry"; } }
}
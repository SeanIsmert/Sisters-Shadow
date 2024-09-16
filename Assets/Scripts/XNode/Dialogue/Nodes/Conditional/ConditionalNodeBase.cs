using XNode;

public abstract class ConditionalNodeBase : Node 
{
    [Input(typeConstraint = TypeConstraint.Strict)]
    public int entry;

    [Output(connectionType = ConnectionType.Override)]
    public bool ifTrue;  // Proceed if the condition is true

    [Output(connectionType = ConnectionType.Override)]
    public bool ifFalse; // Proceed if the condition is false

    // Override this in derived classes to implement the condition
    public abstract bool Condition();

    //public override string GetDialogueType { get { return "Conditional"; } }
}
using XNode;

public abstract class ConditionalNodeBase : CoreNodeBase 
{
    [Input(typeConstraint = TypeConstraint.Strict)]
    public bool enter;

    [Output(connectionType = ConnectionType.Override)]
    public bool isTrue;  // String port name that OnPortCondition returns if Condition

    [Output(connectionType = ConnectionType.Override)]
    public bool isFalse; // String port name that OnPortCondition returns if !Condition

    //Override this in derived classes to ensure correct output path is choosen
    public abstract string PortOnCondtion();

    //Override this in derived classes to implement the condition
    public abstract bool Condition();
}
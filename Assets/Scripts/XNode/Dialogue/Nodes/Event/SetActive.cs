using UnityEngine;

public class SetActive : EventNodeBase
{
    [Input(typeConstraint = TypeConstraint.Strict)]
    public bool enter;
    [Output(connectionType = ConnectionType.Override)]
    public bool exit;


    public GameObject gameObject;
    [Tooltip("setting the game objects state to true will make it appear (SetActive-True) and in vice versa")]
    public bool state;

    public void ExecuteEvent()
    {
        if (gameObject != null)
            gameObject.SetActive(state);
    }

    public override string GetNodeType { get { return "ActiveEvent"; } }
}
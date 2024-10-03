using UnityEngine;

public class SetActive : EventNodeBase
{
    [Input]
    public bool enter;
    [Output(connectionType = ConnectionType.Override)]
    public bool exit;

    [Tooltip("Set the GameObject's state to true to make it appear, or false to hide it")]
    public bool state = false;


    /// <summary>
    /// I FREAKING HATE THIS GARBAGE
    /// </summary>
    public override void ExecuteEvent(GameObject[] objects)
    {
        if (objects.Length > 0)
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(state);
            }
        }
    }

    public override string GetNodeType { get { return "ActiveEvent"; } }
}
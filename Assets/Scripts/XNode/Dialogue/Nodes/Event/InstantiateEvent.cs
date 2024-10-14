using UnityEngine;

public class InstantiateEvent : EventNodeBase
{
    [Input]
    public bool enter;
    [Output(connectionType = ConnectionType.Override)]
    public bool exit;

    [Tooltip("Prefab to instantiate")]
    public GameObject gameObject;

    public override void ExecuteEvent(GameObject[] objects)
    {
        if (objects.Length > 0)
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(gameObject);
            }
        }
    }

    public override string GetNodeType { get { return "ActiveEvent"; } }
}
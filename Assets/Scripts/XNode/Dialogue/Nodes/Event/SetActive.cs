using UnityEngine;

public class SetActive : EventNodeBase
{
    [Input(typeConstraint = TypeConstraint.Strict)]
    public bool enter;
    [Output(connectionType = ConnectionType.Override)]
    public bool exit;

    [Tooltip("The string of the game object in scene, make sure it is unique.")]
    public string gameObjectName;
    [Tooltip("Set the GameObject's state to true to make it appear, or false to hide it")]
    public bool state = false;

    private GameObject _gameObject;


    private void OnEnable()
    {
        // Find the GameObject in the scene based on the name
        _gameObject = GameObject.Find(gameObjectName);
        // Optionally, you could use tags: targetObject = GameObject.FindWithTag(targetObjectName);
        _gameObject?.SetActive(!state);
    }

    /// <summary>
    /// I FREAKING HATE THIS GARBAGE
    /// </summary>
    public void ExecuteEvent()
    {
        if (_gameObject != null)
            _gameObject.SetActive(state);
        else
            Debug.Log("Object is null");
    }

    public override string GetNodeType { get { return "ActiveEvent"; } }
}
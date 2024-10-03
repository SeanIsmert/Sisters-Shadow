using UnityEngine;

public abstract class EventNodeBase : CoreNodeBase
{
    public abstract void ExecuteEvent(GameObject[] objects);
}
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NewSensor : MonoBehaviour
{
    private NewAgent _parentAgent;
    private SphereCollider _sensor;

    public SphereCollider GetSensorCollider
    {
        get
        {
            if(_sensor == null)
            {
                _sensor = GetComponent<SphereCollider>();
            }

            return _sensor;
        }
    }

    private void Awake()
    {
        GetSensorCollider.isTrigger = true;

        if (GetComponentInParent<NewAgent>() != null)
        {
            _parentAgent = GetComponentInParent<NewAgent>();
        }
        else
            Debug.LogError("Sensor requires a parent object with a NewAgent component!");
    }

    private void OnTriggerEnter(Collider other)
    {
        _parentAgent?.OnSensorEvent(TriggerEventType.Enter, other);
    }

    private void OnTriggerStay(Collider other)
    {
        _parentAgent?.OnSensorEvent(TriggerEventType.Stay, other);
    }

    private void OnTriggerExit(Collider other)
    {
        _parentAgent?.OnSensorEvent(TriggerEventType.Exit, other);
    }
}

public enum TriggerEventType
{
    Enter,
    Stay,
    Exit
}

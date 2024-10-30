using UnityEngine;

public class NewAgent : MonoBehaviour
{
    [SerializeField] private float _fov;

    private NewSensor _sensor;
    private Collider _target;

    public TempStates _curState;

    // Start is called before the first frame update
    void Start()
    {
        _sensor = GetComponentInChildren<NewSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get the true position of the sensor
    /// When an object is a child of another object and its parent's scale is changed
    /// this will change the "true" scale of the child even if the scale does not change in the inspector
    /// This will always return the most accurate version of the position
    /// </summary>
    public Vector3 GetSensorPosition
    {
        get
        {
            if (_sensor == null)
            {
                return Vector3.zero;
            }

            Vector3 pos = _sensor.transform.position;
            pos.x += _sensor.GetSensorCollider.center.x * _sensor.transform.lossyScale.x;
            pos.y += _sensor.GetSensorCollider.center.y * _sensor.transform.lossyScale.y;
            pos.z += _sensor.GetSensorCollider.center.z * _sensor.transform.lossyScale.z;

            return pos;
        }
    }

    /// <summary>
    /// Get the true radius of the sensor
    /// When an object is a child of another object and its parent's scale is changed
    /// this will change the "true" scale of the child even if the scale does not change in the inspector
    /// This will always return the most accurate version of the radius
    /// </summary>
    public float GetSensorRadius
    {
        get
        {
            if (_sensor == null)
            {
                return 0f;
            }

            float sensorRadius = _sensor.GetSensorCollider.radius;
            float radius = Mathf.Max(sensorRadius * _sensor.transform.lossyScale.x, sensorRadius * _sensor.transform.lossyScale.y);
            radius = Mathf.Max(radius, sensorRadius * _sensor.transform.lossyScale.z);

            return radius;
        }
    }

    public void OnSensorEvent(TriggerEventType triggerEvent, Collider other)
    {
        switch(triggerEvent)
        {
            case TriggerEventType.Enter:
                _target = other;
                UpdateManager.FastUpdate += SearchActions;
                break;

            case TriggerEventType.Stay:
                break;

            case TriggerEventType.Exit:
                _target = null;
                UpdateManager.FastUpdate -= SearchActions;
                break;
        }
    }

    private void SearchActions()
    {
        _curState = TempStates.Searching;

        if (IsColliderVisible(_target))
            _curState = TempStates.Chase;
    }

    private bool IsColliderVisible(Collider other)
    {
        Vector3 direction = other.transform.position - GetSensorPosition;
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle < _fov * 0.5f)
        {
            if (Physics.Raycast(GetSensorPosition, direction, out RaycastHit hit, GetSensorRadius))
            {
                if (hit.collider == other)
                    return true;
            }
        }

        return false;
    }

    public enum TempStates
    {
        Idle,
        Patrol,
        Searching,
        Chase,
        Attack,
        Dead
    }

    private void OnDrawGizmos()
    {
        if (_sensor == null)
            return;

        UnityEditor.Handles.color = Color.cyan;
        Vector3 rotatedForward = Quaternion.Euler(0f, -_fov * 0.5f, 0f) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(GetSensorPosition, Vector3.up, rotatedForward, _fov, GetSensorRadius);
    }
}

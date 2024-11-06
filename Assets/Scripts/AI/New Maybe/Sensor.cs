using UnityEngine;

namespace AIController
{
    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour
    {
        private Agent _parentAgent;      // Reference to the parent's Agent component.
        private SphereCollider _sensor;     // Reference to this object's sphere collider.

        /// <summary>
        /// Returns this sensor's collider.
        /// </summary>
        public SphereCollider GetSensorCollider
        {
            get
            {
                if (_sensor == null)
                {
                    _sensor = GetComponent<SphereCollider>();           // Set collider reference if not already set.
                }

                return _sensor;
            }
        }

        private void Awake()
        {
            GetSensorCollider.isTrigger = true;                         // Make sure collider is trigger.

            if (GetComponentInParent<Agent>() != null)
            {
                _parentAgent = GetComponentInParent<Agent>();        // Set Agent reference.
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
}
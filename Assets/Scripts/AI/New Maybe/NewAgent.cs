using AIController;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace AIController
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NewAgent : MonoBehaviour
    {
        private Dictionary<NewStateType, NewStateBase> _states = new Dictionary<NewStateType, NewStateBase>();      //Stores all states, and a key to quickly find them.
        [Header("States")]
        [Tooltip("This agent's currently active state type.")]
        public NewStateType _curStateType;
        private NewStateBase _curState;

        [Header("Values")]
        [Tooltip("This agent's field of view.")]
        [SerializeField] private float _fov;

        public NavMeshAgent GetNavAgent { get { return _navAgent; } }

        private NewSensor _sensor;
        private Collider _target;
        private NavMeshAgent _navAgent;

        public Collider GetTarget { get { return _target; } }

        // Start is called before the first frame update
        void Start()
        {
            _sensor = GetComponentInChildren<NewSensor>();      // Find the child sensor component.
            _navAgent = GetComponent<NavMeshAgent>();           // Find NavMeshAgent.

            NewStateBase[] foundStates = GetComponents<NewStateBase>();     // Collect all states on this object.

            foreach (NewStateBase state in foundStates)                     // Look through the states found.
            {
                if (_states.ContainsKey(state.GetStateType) == false)        // Make sure we don't have duplicate states.
                {
                    _states.Add(state.GetStateType, state);                 // Add the current state to dictionary.
                    state.InitState(this);                                  // Initialize the current state.
                }
            }

            if (_states.ContainsKey(NewStateType.Patrol))
                ChangeState(NewStateType.Patrol);
        }

        private void OnEnable()
        {
            UpdateManager.FastUpdate += StateUpdate;        // Perform current state actions each FastUpdate tick.
        }

        private void OnDisable()
        {
            UpdateManager.FastUpdate -= StateUpdate;        // Stop performing current state actions.
        }

        /// <summary>
        /// Listens to FastUpdate and performs state actions each tick.
        /// </summary>
        /// <param name="tickSpeed"></param>
        private void StateUpdate(float tickSpeed)
        {
            ChangeState(_curState.OnStateUpdate());
        }

        /// <summary>
        /// Takes in a State Type and transitions into the new state.
        /// </summary>
        /// <param name="newState"></param>
        private void ChangeState(NewStateType newState)
        {
            if (!_states.ContainsKey(newState))        // Ensure we have the new state registered.
                return;

            if (_curState == null || _curState.GetStateType != newState)
            {
                if (_curState != null)              // Exit the current state.
                    _curState.OnStateExit();

                _curStateType = newState;           // Set the new state type.
                _curState = _states[newState];      // Set the new state to current.
                _curState.OnStateEnter();           // Perform the state's enter actions.
            }
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

        /// <summary>
        /// Called by the sensor whenever a trigger event occurs, and takes in the sensor event type.
        /// </summary>
        /// <param name="triggerEvent"></param>
        /// <param name="other"></param>
        public void OnSensorEvent(TriggerEventType triggerEvent, Collider other)
        {
            switch (triggerEvent)
            {
                case TriggerEventType.Enter:
                    _target = other;                                // Set target.
                    break;

                case TriggerEventType.Stay:
                    break;

                case TriggerEventType.Exit:
                    _target = null;                                 // Remove target.
                    break;
            }
        }

        /// <summary>
        /// Checks agent field of view and line of sight to determine if the player is visible.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsColliderVisible(Collider other)
        {
            if (other != null)      // Ensure the target is not null.
            {
                Vector3 direction = other.transform.position - GetSensorPosition;       // Get relative vector.
                float angle = Vector3.Angle(transform.forward, direction);              // Get angle.

                if (angle < _fov * 0.5f)        // Check agent vision.
                {
                    if (Physics.Raycast(GetSensorPosition, direction, out RaycastHit hit, GetSensorRadius))     // Check agent line of sight.
                    {
                        if (hit.collider == other)      // Check that the hit object is the target.
                            return true;                // If all checks pass, the target collider IS visible.
                    }
                }
            }

            return false;       // If any checks fail, the target collider IS NOT visible.
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
}

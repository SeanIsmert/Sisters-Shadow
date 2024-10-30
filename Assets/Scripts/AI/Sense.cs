using UnityEngine;

namespace AIController
{
    /// <summary>
    /// Class responsible for the "senses" of the AI.
    /// Allows for line of sight, and "hearing"
    /// Needs a big refactor!!!
    /// Written by: Sean
    /// Modified by:
    /// </summary>
    #region Require
    [RequireComponent(typeof(SphereCollider))]
    #endregion
    public class Sense : MonoBehaviour
    {
        #region Variables
        [Header("Vision Settings")]
        [SerializeField] private float _sightRange = 1f;
        [SerializeField] public float _fov = 110f;
        [Space]

        [Header("Hearing Settings")]
        [SerializeField] private float _hearingDistance;


        [HideInInspector] public bool isVisible;
        [HideInInspector] public bool isInRange;
        [HideInInspector] public bool isHeard;
        [HideInInspector] public bool isAlert;

        private SphereCollider _collider;
        private Agent _agent;
        private Sense _sense;
        #endregion

        #region Initialize
        private void Start()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            _agent = GetComponent<Agent>();
            _sense = this;
        }
        #endregion

        #region CodeBase
        /// <summary>
        /// Get the attached sphere collider
        /// If we have not set up the sphere collider yet, do so now
        /// </summary>
        public SphereCollider GetSphereCollider
        {
            get
            {
                if (_collider == null)
                {
                    _collider = GetComponent<SphereCollider>();
                }

                return _collider;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = true;
                // UpdateManager.FastUpdate += SearchForPlayer;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (IsColliderVisible(other))
                    isVisible = true;
                else
                    isVisible = false;
                if (IsHeard(other))
                    isHeard = true;
                else
                    isHeard = false;
            }

            if (isAlert && other.CompareTag("AI"))
            {
                IsAlerting(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isVisible = false;
                isInRange = false;
            }
        }

        private void SearchForPlayer()
        {

        }

        /// <summary>
        /// Get the true position of the sensor
        /// When an object is a child of another object and its parent's scale is changed
        /// this will change the "true" scale of the child even if the scale does not change in the inspector
        /// This will always return the most accurate version of the position
        /// </summary>
        private Vector3 GetSensorPosition
        {
            get
            {
                if (_sense == null)
                {
                    return Vector3.zero;
                }

                Vector3 pos = _sense.transform.position;
                pos.x += _sense.GetSphereCollider.center.x * _sense.transform.lossyScale.x;
                pos.y += _sense.GetSphereCollider.center.y * _sense.transform.lossyScale.y;
                pos.z += _sense.GetSphereCollider.center.z * _sense.transform.lossyScale.z;

                return pos;
            }
        }

        /// <summary>
        /// Get the true radius of the sensor
        /// When an object is a child of another object and its parent's scale is changed
        /// this will change the "true" scale of the child even if the scale does not change in the inspector
        /// This will always return the most accurate version of the radius
        /// </summary>
        private float GetSensorRadius
        {
            get
            {
                if (_sense == null)
                {
                    return 0f;

                }

                float sensorRadius = _sense.GetSphereCollider.radius;
                float radius = Mathf.Max(sensorRadius * _sense.transform.lossyScale.x, sensorRadius * _sense.transform.lossyScale.y);
                radius = Mathf.Max(radius, sensorRadius * _sense.transform.lossyScale.z);

                return radius;
            }
        }

        /// <summary>
        /// Determines if the object of question is within the field of view
        /// If so, then determine if it is behind a wall or something similar
        /// If the object is within the field of view is not blocked from line of sight
        /// The agent can see them, and therefore return true
        /// </summary>
        private bool IsColliderVisible(Collider other)
        {
            Vector3 direction = other.transform.position - GetSensorPosition;
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle > _fov * 0.5f)
            {
                return false;
            }

            RaycastHit hit;

            if (Physics.Raycast(GetSensorPosition, direction.normalized, out hit, GetSensorRadius * _sightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the object of question is within the sphere of sound
        /// If so, then determine if it is behind a wall or something similar
        /// If the object is within the sphere and not blocked
        /// The agent can hear them, and therefore return true
        /// </summary>
        private bool IsHeard(Collider other)
        {
            Vector3 direction = other.transform.position - GetSensorPosition;
            float sqrDistanceToTarget = direction.sqrMagnitude;
            float angle = Vector3.Angle(transform.forward, direction);

            if (sqrDistanceToTarget > _hearingDistance * _hearingDistance)
            {
                return false;
            }

            return true;
        }

        private bool IsAlerting(Collider other)
        {
            Sense otherAI = other.gameObject.GetComponent<Sense>();
            if (otherAI != null && otherAI.isAlert == false)
            {
                otherAI.isAlert = true;
                return true;
            }
            return false;
        }
        #endregion

        #region Editor
        #if UNITY_EDITOR

        // Draws an in engine representation of the agent's line of sight
        // Make sure the gizmo colour has an alpha above 0 otherwise it cannot be seen
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = new Color(1,1,1,.25f);
            Vector3 rotatedForward = Quaternion.Euler(0f, -_fov * 0.5f, 0f) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(GetSensorPosition, Vector3.up, rotatedForward, _fov, GetSensorRadius * _sightRange);
        }
        #endif
        #endregion 
    }
}

using UnityEngine;

namespace AIController
{
    public class NewPatrolState : NewStateBase
    {
        [Header("Behavior")]
        [Tooltip("Determines whether the agent will follow a predetermined path, or wander within a given area.")]
        [SerializeField] private PatrolType _patrolType;

        [Header("Path Values")]
        [Tooltip("The PatrolNetwork script that this agent will reference for waypoint information.")]
        [SerializeField] private PatrolNetwork _patrolPath;
        private int _patrolIndex;

        [Header("Wander Values")]
        [Tooltip("The Collider that this agent will use to find random wander points.")]
        [SerializeField] private Collider _wanderArea;

        public override NewStateType GetStateType => NewStateType.Patrol;

        public override NewStateType OnStateUpdate()
        {
            if (_agent.GetNavAgent.remainingDistance <= _agent.GetNavAgent.stoppingDistance)        // If the agent has reached its destination, set a new path.
            {
                _agent.GetNavAgent.SetDestination(MovementDestination());
            }

            if (_agent.IsColliderVisible(_agent.GetTarget))     // If the player is in range and visible, chase!
                return NewStateType.Chase;

            return GetStateType;
        }

        public override Vector3 MovementDestination()
        {
            switch(_patrolType)
            {
                case PatrolType.Path:

                    if (_patrolPath == null || _patrolPath.waypoints.Count == 0)        // If no patrol path is set or waypoints do not exist, send a warning and return.
                    {
                        Debug.LogError(gameObject + "'s patrol path field is null or has no waypoints!");
                        return transform.position;
                    }

                    _patrolIndex++;

                    if (_patrolIndex >= _patrolPath.waypoints.Count)            // If index is greater than count, loop back to zero.
                        _patrolIndex = 0;

                    return _patrolPath.waypoints[_patrolIndex].position;        // Return the position of the waypoint at the current index.

                case PatrolType.Wander:

                    if(_wanderArea == null)                             // If no wander area is set, send a warning and return.
                    {
                        Debug.LogError(gameObject + "'s wander area field is null!");
                        return transform.position;
                    }

                    Bounds bounds = _wanderArea.bounds;                 // Collect the bounds of the wander area.

                    Vector3 target = new(                               // Use bounds to generate a new random waypoint.
                        Random.Range(bounds.min.x, bounds.max.x),
                        Random.Range(bounds.min.y, bounds.max.y),
                        Random.Range(bounds.min.z, bounds.max.z)
                        );

                    return target;
            }

            return transform.position;
        }

        private enum PatrolType
        {
            Path,
            Wander,
        }
    }
}
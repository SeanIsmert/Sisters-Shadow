using UnityEngine;

namespace AIController
{
    public class PatrolState : StateBase
    {
        [Header("Behavior")]
        [Tooltip("Determines whether the agent will follow a predetermined path, or wander within a given area.")]
        [SerializeField] private PatrolType _patrolType;
        [Tooltip("The amount of time in seconds this agent will wait before moving to a new patrol destination.")]
        [SerializeField] private float _waitTime;
        private float _waitTimer;

        [Header("Path Values")]
        [Tooltip("The PatrolNetwork script that this agent will reference for waypoint information.")]
        [SerializeField] private PatrolNetwork _patrolPath;
        private int _patrolIndex;

        [Header("Wander Values")]
        [Tooltip("The Collider that this agent will use to find random wander points.")]
        [SerializeField] private Collider _wanderArea;

        public override StateType GetStateType => StateType.Patrol;

        public override void OnStateEnter()
        {
            _waitTimer = _waitTime;

            switch (_patrolType)
            {
                case PatrolType.Path:               // In Path state, initialize by starting from the closest waypoint.

                    int closestIndex = 0;           // Stores the index of the closest waypoint.
                    float closestDistance = 0;      // Stores the shortest distance found.

                    for (int i = 0; i < _patrolPath.waypoints.Count; i++)                                               // Check through the list of waypoints.
                    {
                        float distance = Vector3.Distance(transform.position, _patrolPath.waypoints[i].position);       // Calculate distance between agent and waypoint.

                        if (i == 0)                         // On first loop, initialize and continue.
                        {
                            closestIndex = i;
                            closestDistance = distance;
                            continue;
                        }

                        if (distance < closestDistance)     // If we find a closer waypoint, store it.
                        {
                            closestDistance = distance;
                            closestIndex = i;
                        }
                    }

                    _patrolIndex = closestIndex;                                    // Set the next patrol target as the closest waypoint.
                    _agent.GetNavAgent.SetDestination(MovementDestination());       // Start movement.
                    break;

                case PatrolType.Wander:
                    break;
            }
        }

        public override StateType OnStateUpdate(float tickSpeed)
        {
            if (_agent.IsColliderVisible(_agent.GetTarget))     // If the player is in range and visible, chase!
                return StateType.Chase;

            if (_agent.GetNavAgent.remainingDistance <= _agent.GetNavAgent.stoppingDistance)        // Check if the agent has reached its destination.
            {
                if (_waitTimer > 0f)                // Check if the wait timer is still counting.
                {
                    _waitTimer -= tickSpeed;        // Count down wait timer.
                }
                else
                {
                    switch (_patrolType)
                    {
                        case PatrolType.Path:                                                       // In Path state, do waypoint index stuff.

                            _patrolIndex++;

                            if (_patrolIndex >= _patrolPath.waypoints.Count)                        // If index is greater than count, loop back to zero.
                                _patrolIndex = 0;

                            _agent.GetNavAgent.SetDestination(MovementDestination());
                            break;

                        case PatrolType.Wander:                                                     // In Wander state, just get a new destination.

                            _agent.GetNavAgent.SetDestination(MovementDestination());
                            break;
                    }

                    _waitTimer = _waitTime;         // Reset timer.
                }
            }

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
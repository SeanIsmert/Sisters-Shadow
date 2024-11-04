using UnityEngine;

namespace AIController
{
    public class PatrolState : StateBase
    {
        [SerializeField] private PatrolNetwork _patrolPath;
        private int _patrolIndex = 0;

        public override StateType GetStateType => StateType.Patrol;

        public override StateType OnStateUpdate()
        {
            if(!_agent.GetNavAgent.pathPending || _agent.GetNavAgent.remainingDistance <= _agent.GetNavAgent.stoppingDistance)
                _agent.GetNavAgent.SetDestination(MovementDestination());

            return GetStateType;
        }

        public override Vector3 MovementDestination()
        {
            if(_patrolPath != null)
            {
                _patrolIndex++;

                if (_patrolIndex > _patrolPath.waypoints.Count)
                    _patrolIndex = 0;

                return _patrolPath.waypoints[_patrolIndex].transform.position;
            }
            else
                return _agent.transform.position;
        }
    }
}
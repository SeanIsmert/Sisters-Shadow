using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIController
{
    public class NewChaseState : NewStateBase
    {
        public override NewStateType GetStateType => NewStateType.Chase;

        public override NewStateType OnStateUpdate()
        {
            if (_agent.IsColliderVisible(_agent.GetTarget))     // If the player is visible, go to their position.
            {
                _agent.GetNavAgent.SetDestination(MovementDestination());

                if(Vector3.Distance(transform.position, _agent.GetTarget.transform.position) <= _agent.GetNavAgent.stoppingDistance + 1f)     // If the player is visible and within range, go to attack.
                {
                    return NewStateType.Attack;
                }
            }
            else if (_agent.GetNavAgent.remainingDistance <= _agent.GetNavAgent.stoppingDistance)       // If the player is not visible and we reached their last known location, return to patrolling.
            {
                return NewStateType.Patrol;
            }

            return GetStateType;
        }

        public override Vector3 MovementDestination()
        {
            return _agent.GetTarget.transform.position;
        }
    }
}
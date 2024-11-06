using UnityEngine;

namespace AIController
{
    public class DeathState : StateBase
    {
        public override StateType GetStateType => StateType.Dead;

        public override void OnStateEnter()
        {
            _agent.GetNavAgent.isStopped = true;
        }

        public override StateType OnStateUpdate(float tickSpeed)
        {
            return GetStateType;
        }

        public override Vector3 MovementDestination()
        {
            throw new System.NotImplementedException();
        }
    }
}
using UnityEngine;

namespace AIController
{
    public class AttackState : StateBase
    {
        [SerializeField] private int _damageAmount;

        public override StateType GetStateType => StateType.Attack;

        public override StateType OnStateUpdate(float tickSpeed)
        {
            _agent.GetTarget.gameObject.GetComponent<EntityHealth>()?.ValueChange(_damageAmount);
            return StateType.Chase;
        }

        public override Vector3 MovementDestination()
        {
            throw new System.NotImplementedException();
        }
    }
}
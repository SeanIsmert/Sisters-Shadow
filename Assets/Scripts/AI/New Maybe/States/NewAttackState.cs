using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIController
{
    public class NewAttackState : NewStateBase
    {
        [SerializeField] private int _damageAmount;

        public override NewStateType GetStateType => NewStateType.Attack;

        public override NewStateType OnStateUpdate()
        {
            _agent.GetTarget.gameObject.GetComponent<EntityHealth>()?.ValueChange(_damageAmount);
            return NewStateType.Chase;
        }

        public override Vector3 MovementDestination()
        {
            throw new System.NotImplementedException();
        }
    }
}
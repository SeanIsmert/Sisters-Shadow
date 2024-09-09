using UnityEngine;

namespace AIController
{
    /// <summary>
    /// The Base Version of a state
    /// Stores all methods and data shared by every state
    /// </summary>
    public abstract class StateBase : MonoBehaviour
    {
        #region Variables
        protected Agent _agent;
        protected Sense _sense;
        protected Animator _animator;

        protected float _currentSpeed;
        protected float _targetSpeed;

        public abstract StateType GetStateType { get; }
        #endregion

        #region Initialize
        //Set up the state and give it an owner
        public void InitState(Agent agent)
        {
            _agent = agent;
            _animator = GetComponent<Animator>();
            _sense = agent.GetComponent<Sense>();
        }
        #endregion

        #region CodeBase
        /// <summary>
        /// Is called whenever we enter this state
        /// </summary>
        public virtual void OnStateEnter() { }

        /// <summary>
        /// Is called whenever we exit this state
        /// </summary>
        public virtual void OnStateExit() { }

        /// <summary>
        /// Is called every frame while in this state || 
        /// Returns a state type to go to the next state
        /// </summary>
        public abstract StateType OnStateUpdate();
        #endregion
    }

    #region States
    //All of the states in the game
    public enum StateType
    {
        Idle, Chase, Patrol, Pursue, Attack, Dead
    }
    #endregion
}
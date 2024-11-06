using UnityEngine;

namespace AIController
{
    /// <summary>
    /// The Base Version of a state that Stores all methods and data shared by every state.
    /// Every state inherits these characteristics.
    /// Might be fixed?! but I don't know how to make AI without update :p
    /// Written by: Sean
    /// Modified by:
    /// </summary>
    public abstract class NewStateBase : MonoBehaviour
    {
        #region Variables
        protected NewAgent _agent;
        protected Sense _sense;
        protected Animator _animator;

        protected float _currentSpeed;
        protected float _targetSpeed;

        public abstract NewStateType GetStateType { get; }
        #endregion

        #region Initialize
        //Set up the state and give it an owner
        public void InitState(NewAgent agent)
        {
            _agent = agent;
            _animator = GetComponent<Animator>();
            //_sense = agent.GetComponent<Sense>();
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
        public abstract NewStateType OnStateUpdate();

        public abstract Vector3 MovementDestination();
        #endregion
    }

    #region States
    //All of the states in the game
    public enum NewStateType
    {
        Idle, Chase, Patrol, Pursue, Attack, Dead
    }
    #endregion
}
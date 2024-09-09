using System.Collections.Generic;
using UnityEngine.AI;
using AIController;
using UnityEngine;

namespace AIController
{
    #region Require
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    #endregion
    public class Agent : MonoBehaviour, IDamageable, IDataPersistence
    {
        #region GUID
        [SerializeField, HideInInspector] private string guid;
        [ContextMenu("Generate Guid")]
        private void GenerateGuid()
        {
            guid = System.Guid.NewGuid().ToString();
        }
        #endregion

        #region Varriables
        private Dictionary<StateType, StateBase> _states = new Dictionary<StateType, StateBase>();      //Stores all states, and a key to quickly find them

        public NavMeshAgent getNavAgent { get { return _navAgent; } }
        protected NavMeshAgent _navAgent;

        public Animator animator { get { return _animator; } }
        protected Animator _animator;

        [Header("Current State")]
        public StateType _curStateType;
        private StateBase _curState;

        private Sense _sense;
        [HideInInspector]public float _slerpSpeed = 1f;

        [Header("BadCode")]
        public Transform _target;
        [SerializeField] private int _health = 5;
        [SerializeField] private AudioClip _death;
        [SerializeField] private AudioClip _hit;
        #endregion

        #region CodeBase
        #region Initialize
        private void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _sense = GetComponent<Sense>();

            StateBase[] foundStates = GetComponents<StateBase>();

            //Loop through all states found
            for (int i = 0; i < foundStates.Length; i++)
            {
                //If we do not yet have a state of X type
                if (_states.ContainsKey(foundStates[i].GetStateType) == false)
                {
                    //Add it into our states
                    _states.Add(foundStates[i].GetStateType, foundStates[i]);
                    //Make sure the state is set up with the agent as its owner
                    foundStates[i].InitState(this);
                }
            }
            ChangeState(_curStateType);
        }
        #endregion

        #region Updates
        public void Update()
        {
            if (_curState == null)
                return;

            if (_health <= 0)
            {
                _curStateType = StateType.Dead;
                _curState = _states[StateType.Dead];
                ChangeState(_curState.OnStateUpdate());
            }
            else
            {
                //Update that state, and go to the state it tells us to
                ChangeState(_curState.OnStateUpdate());
            }
        }

        private void OnAnimatorMove()
        {
            if ((_curStateType != StateType.Dead && Time.deltaTime > 0))
            {
                    _navAgent.updatePosition = true;
                    _navAgent.updateRotation = false;
                    _navAgent.velocity = _animator.deltaPosition / Time.deltaTime;

                    Quaternion newRot = Quaternion.LookRotation(_navAgent.desiredVelocity);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRot, _slerpSpeed * Time.deltaTime);
            }
            else
            {
                _navAgent.updatePosition = false;
                _navAgent.updateRotation = false;
            }
        }
        #endregion

        #region States
        private void ChangeState(StateType newState)
        {
            //If we do not own a state of X Type
            if (_states.ContainsKey(newState) == false)
            {
                return;
            }

            //If the current state is empty or does not equal the type we want to go to
            if (_curState == null || _curState.GetStateType != newState)
            {
                //Check if the current state is empty
                if (_curState != null)
                {
                    //if it is not empty call on exit
                    _curState.OnStateExit();
                }
                //Set the current state type to the new state type (not needed just visual)
                _curStateType = newState;
                //Set current state to new state
                _curState = _states[newState];
                //Tell new state to enter
                _curState.OnStateEnter();
            }
        }
        #endregion

        #region Damage
        public void ValueChange(int amount)
        {
            _health += amount;

            _sense.isVisible = true;

            if (_health > 0)
                AudioManager.instance.PlaySFX2D(_hit, .2f);
            else
                AudioManager.instance.PlaySFX2D(_death, 0f);
        }
        #endregion

        #region SaveLoad
        public void LoadData(GameData data)
        {
            if (data.AIStates.TryGetValue(guid, out AIState savedState))
            {
                transform.rotation = savedState.rotation;
                transform.position = savedState.position;
                _curStateType = savedState.state;
                _health = savedState.health;

            }
        }

        public void SaveData(GameData data)
        {
            var aiState = new AIState(transform.rotation, transform.position, _curStateType, _health);
            data.AIStates[guid] = aiState;
        }
        #endregion
        #endregion
    }
    #region States
    public enum TriggerEventType
    {
        Enter, Stay, Exit
    }
    #endregion
}

#region SaveLoadInfo
[System.Serializable]
public class AIState
{
    public Quaternion rotation;
    public Vector3 position;
    public StateType state;
    public int health;

    public AIState(Quaternion rotation, Vector3 position, StateType state, int health)
    {
        this.rotation = rotation;
        this.position = position;
        this.state = state;
        this.health = health;
    }
}
#endregion
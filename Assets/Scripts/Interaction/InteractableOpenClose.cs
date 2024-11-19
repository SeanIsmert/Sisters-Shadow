using UnityEngine.Events;
using System.Collections;
using UnityEngine;
using System;

public class InteractableOpenClose : MonoBehaviour, IInteract//, IDataPersistence
{
    [Header("OpenCloseType")]
    [SerializeField] private InteractableType _openType;
    [Space]

    [Header("Settings")]
    [Tooltip("Will convert the animation from math to use the animator attached")]
    [SerializeField] private bool _useAnimator;
    [Tooltip("Whether or not the object is locked")]
    [SerializeField] private bool _locked;
    [SerializeField] private bool _spawnObjects;
    [Space]

    [Header("SubSettings")]
    [Tooltip("The speed at which the object moves from position to position")]
    [SerializeField] private AnimationCurve _transitionSpeed;
    [Tooltip("The array of objects to spawn in and set active")]
    [SerializeField] private GameObject[] _gameObjects;
    [Tooltip("Will flip the direction the object opens on")]
    [SerializeField] private bool _flipAxis;
    [Tooltip("If the door can be unlocked by damaging it")]
    [SerializeField] private bool _damagable;
    [Tooltip("The string value needed to unlock the object")]
    [SerializeField] private string _keyID;
    [Space]

    [Header("Events")]
    [SerializeField] private UnityEvent _onOpenActions;
    [SerializeField] private UnityEvent _onTryActions;
    [SerializeField] private UnityEvent _onCloseActions;
    private Animator _animator;

    private bool _transitioning, open;
    private string _guid;

    public Vector3 Position { get { return transform.position; } }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interaction()
    {
        if (_locked)
        {
            _onTryActions?.Invoke();
        }

        if (_useAnimator)
        {
            switch (_openType)
            {
                case InteractableType.Door:
                    break;
                case InteractableType.Drawer:
                    break;
                case InteractableType.Shutter:
                    break;
                case InteractableType.Slider:
                    break;
            }

            bool open = false;//_animator.GetBool();
            bool transitioning = false;//_animator.GetBool();

            if (!transitioning)
            {
                if (!open)
                {
                    if (!_locked)
                    {
                        //Open();
                        //SpawnObject();
                    }
                    else
                    {
                        //LockCheck();
                    }
                }
                else
                {
                    //Close();
                }
            }
        }
        else
        {
            if (_useAnimator)
            {
                switch (_openType)
                {
                    case InteractableType.Door:
                        break;
                    case InteractableType.Drawer:
                        break;
                    case InteractableType.Shutter:
                        break;
                    case InteractableType.Slider:
                        break;
                }
            }
        }
    }

    public void SpawnObjects(bool spawn)
    {
        if (!_spawnObjects)
            return;

        foreach (var obj in _gameObjects)
        {
            obj.SetActive(spawn);
        }
    }

    private IEnumerator OpenClose(bool open)
    {
        yield return new WaitForSeconds(_transitionSpeed.Evaluate(5));//.time);
    }

    /*
    public void LoadData(GameData data)
    {
        throw new System.NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        throw new System.NotImplementedException();
    }
    */

    private enum InteractableType { Door, Shutter, Slider, Drawer }
}

using UnityEngine.Events;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractableOpenClose : MonoBehaviour, IInteract//, IDataPersistence
{
    [SerializeField] private InteractableType _type;

    [Tooltip("Will convert the animation from math to use the animator attached")]
    [SerializeField] private bool _useAnimator;
    [Tooltip("Whether or not the object is locked")]
    [SerializeField] private bool _locked;
    [Tooltip("Array of objects to spawn when the method is called")]
    [SerializeField] private bool _spawnObjects;

    [Header("SubSettings")]
    [Tooltip("The speed and time which the object moves from position to position")]
    [SerializeField] private AnimationCurve _animationSpeed;
    [Tooltip("Either the distance the object travels, or the degrees it turns")]
    [SerializeField] private float _distance;
    [Tooltip("The array of objects to spawn in and set active")]
    [SerializeField] private GameObject[] _gameObjects;
    [Tooltip("Will flip the direction the object opens on")]
    [SerializeField] private bool _flipAxis;
    [Tooltip("If the door can be unlocked by damaging it")]
    [SerializeField] private bool _damagable;
    [Tooltip("Makes it to where items spawned only spawn once")]
    [SerializeField] private bool _itemsSpawnOnce;
    [Tooltip("The string value needed to unlock the object")]
    [SerializeField] private string _keyID;
    [Space]

    [Header("Events")]
    [SerializeField] private UnityEvent _onOpenActions;
    [SerializeField] private UnityEvent _onTryActions;
    [SerializeField] private UnityEvent _onCloseActions;
    private Animator _animator;

    private bool _transitioning, _open;
    private string _guid;

    public Vector3 Position { get { return transform.position; } }

#region Initialize
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interaction()
    {
        Debug.Log($"Player interacted");
        if (!CheckLock() && _locked)
        {
            _onTryActions?.Invoke();
            return;
        }

    #region Animator
        if (_useAnimator)
        {
            Debug.Log($"In Animator");

            _transitioning = _animator.GetBool("IsTransitioning");
            _open = _animator.GetBool("IsOpen");

            switch (_type)
            {
                case InteractableType.Hinge:
                    _animator.SetInteger("Type", 0);
                    break;
                case InteractableType.Drawer:
                    _animator.SetInteger("Type", 1);
                    break;
                case InteractableType.Shutter:
                    _animator.SetInteger("Type", 2);
                    break;
                case InteractableType.Slider:
                    _animator.SetInteger("Type", 3);
                    break;
            }

            if (_transitioning)
                return;
            if (!_open)
            {
                _animator.SetBool("IsOpen", true);
                _onOpenActions?.Invoke();
            }
            else
            {
                _animator.SetBool("IsOpen", false);
                _onOpenActions?.Invoke();
            }
        }
    #endregion
    #region Math
        else
        {
            Debug.Log($"In Math");
            if (_transitioning)
                return;
            if (_open)
                StartCoroutine(OpenClose(false));
            else
                StartCoroutine(OpenClose(true));
        }
    #endregion
    }
#endregion

#region CodeBase
    public void SpawnObjects(bool spawn)
    {
        if (!_spawnObjects)
            return;

        foreach (var obj in _gameObjects)
        {
            obj.SetActive(spawn);
        }
    }
    public void SetLock(bool locked)
    {
        _locked = locked;
    }
    private bool CheckLock()
    {
        foreach (var token in InventoryManager.Instance.GetInventoryItems()) // Iterate through each item in the player's inventory
        {
            if (!token.GetBaseItem.keyItem) // skip items that arn't stackable items
                continue;

            if (token.GetKeyID == _keyID)
            {
                _locked = false;
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Animation
    private IEnumerator OpenClose(bool open)
    {
        Debug.Log($"Enumerator started: Opening = {open}");
        _transitioning = true;
        float duration = _animationSpeed.keys[_animationSpeed.length - 1].time; // Total duration
        float elapsed = 0f;

        // Calculate direction multiplier
        float directionMultiplier = open ? 1 : -1;
        if (_flipAxis) directionMultiplier *= -1;

        // Define movement/rotation targets dynamically
        Vector3 positionOffset = Vector3.zero;
        Quaternion rotationOffset = Quaternion.identity;

        switch (_type)
        {
            case InteractableType.Hinge:
                float doorAngle = _distance * directionMultiplier;
                rotationOffset = Quaternion.Euler(0, doorAngle, 0);
                break;

            case InteractableType.Drawer:
                positionOffset = transform.forward * _distance * directionMultiplier;
                break;

            case InteractableType.Shutter:
                positionOffset = Vector3.up * _distance * directionMultiplier;
                break;

            case InteractableType.Slider:
                positionOffset = transform.right * _distance * directionMultiplier;
                break;
        }

        // Capture the starting state
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        // Interpolate movement/rotation
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = _animationSpeed.Evaluate(elapsed / duration);

            transform.localPosition = Vector3.Lerp(startPosition, startPosition + positionOffset, t);
            transform.localRotation = Quaternion.Lerp(startRotation, startRotation * rotationOffset, t);

            yield return null;
        }

        // Finalize state
        transform.localPosition = startPosition + positionOffset;
        transform.localRotation = startRotation * rotationOffset;

        // Toggle state
        _open = open;
        _transitioning = false;

        // Invoke appropriate actions
        if (open)
            _onOpenActions?.Invoke();
        else
            _onCloseActions?.Invoke();

        Debug.Log($"Enumerator ended: Open = {_open}");
    }
    #endregion

    #region SaveLoad
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
    #endregion
    private enum InteractableType { Hinge, Drawer, Shutter, Slider }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InteractableOpenClose))]
public class InteractableOpenCloseEditor : Editor
{
    // Serialized properties
    private SerializedProperty _openType, _useAnimator, _locked, _spawnObjects;
    private SerializedProperty _animationSpeed, _distance, _flipAxis;
    private SerializedProperty _damagable, _keyID, _itemsSpawnOnce, _gameObjects;
    private SerializedProperty _onOpenActions, _onTryActions, _onCloseActions;

    // Dropdown states
    private bool _showOpenCloseType = true;
    private bool _showSettings = true;
    private bool _showSubSettings = true;
    private bool _showEvents = true;

    private void OnEnable()
    {
        // Initialize serialized properties
        _openType = serializedObject.FindProperty("_type");
        _useAnimator = serializedObject.FindProperty("_useAnimator");
        _locked = serializedObject.FindProperty("_locked");
        _spawnObjects = serializedObject.FindProperty("_spawnObjects");
        _animationSpeed = serializedObject.FindProperty("_animationSpeed");
        _distance = serializedObject.FindProperty("_distance");
        _flipAxis = serializedObject.FindProperty("_flipAxis");
        _damagable = serializedObject.FindProperty("_damagable");
        _keyID = serializedObject.FindProperty("_keyID");
        _itemsSpawnOnce = serializedObject.FindProperty("_itemsSpawnOnce");
        _gameObjects = serializedObject.FindProperty("_gameObjects");
        _onOpenActions = serializedObject.FindProperty("_onOpenActions");
        _onTryActions = serializedObject.FindProperty("_onTryActions");
        _onCloseActions = serializedObject.FindProperty("_onCloseActions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // OpenCloseType Dropdown
        _showOpenCloseType = EditorGUILayout.Foldout(_showOpenCloseType, "OpenCloseType");
        if (_showOpenCloseType)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_openType);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Space();

        // Settings Dropdown
        _showSettings = EditorGUILayout.Foldout(_showSettings, "Settings");
        if (_showSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_useAnimator, new GUIContent("Use Animator"));
            EditorGUILayout.PropertyField(_locked, new GUIContent("Locked"));
            EditorGUILayout.PropertyField(_spawnObjects, new GUIContent("Spawn Objects"));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Space();

        // SubSettings Dropdown
        _showSubSettings = EditorGUILayout.Foldout(_showSubSettings, "SubSettings");
        if (_showSubSettings)
        {
            EditorGUI.indentLevel++;

            // Show additional fields based on _useAnimator and _locked
            if (!_useAnimator.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_animationSpeed, new GUIContent("Animation Speed"));
                EditorGUILayout.PropertyField(_distance, new GUIContent("Distance"));
                EditorGUILayout.PropertyField(_flipAxis, new GUIContent("Flip Axis"));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            if (_locked.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_damagable, new GUIContent("Damagable"));
                if (!_damagable.boolValue)
                {
                    EditorGUILayout.PropertyField(_keyID, new GUIContent("Key ID"));
                }
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
            }

            if (_spawnObjects.boolValue)
            {
                EditorGUI.indentLevel++;
                // Show ItemsSpawnOnce if there are GameObjects in the array
                if (_gameObjects.arraySize > 0)
                {
                    EditorGUILayout.PropertyField(_itemsSpawnOnce, new GUIContent("Items Spawn Once"));
                }
                EditorGUILayout.PropertyField(_gameObjects, new GUIContent("GameObjects"), true);
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Space();

        // Events Dropdown
        _showEvents = EditorGUILayout.Foldout(_showEvents, "Events");
        if (_showEvents)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_onOpenActions, new GUIContent("On Open Actions"));
            EditorGUILayout.PropertyField(_onTryActions, new GUIContent("On Try Actions"));
            EditorGUILayout.PropertyField(_onCloseActions, new GUIContent("On Close Actions"));
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
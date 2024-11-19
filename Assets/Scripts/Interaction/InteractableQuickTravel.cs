using UnityEngine;

public class InteractableQuickTravel : MonoBehaviour, IInteract
{
    [Header("Settings")]
    [SerializeField] private Transform _waypoint;

    private Transform _player;

    public Vector3 Position { get { return transform.position; } }

    private void Start()
    {
        _player = _player == null ? GameObject.FindGameObjectWithTag("Player").transform : _player;
    }

    public void Interaction()
    {
        _player.position = _waypoint.position;
        InteractableManager.Instance.RemoveTarget(this);
    }
}
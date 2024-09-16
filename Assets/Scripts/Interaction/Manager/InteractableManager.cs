using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that collects information on interactable events.
/// Uses a simple dot product to engage interactions based on players rotation.
/// Utilizes a interface to filter and invokes Interaction on said event.
/// Written by: Sean
/// Modified by: 
/// </summary>
public class InteractableManager : MonoBehaviour
{
    [SerializeField] private IInteract _currentTarget;
    [SerializeField] private List<IInteract> _targets = new List<IInteract>();

    public static InteractableManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteract target = other.GetComponent<IInteract>();

        if (target != null && !_targets.Contains(target))
        {
            _targets.Add(target);
            Debug.Log("Added: " + other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteract target = other.GetComponent<IInteract>();

        if (target != null)
        {
            _targets.Remove(target);
            Debug.Log("Removed: " + other);
        }
    }

    // Method to handle interactions
    public void HandleInteraction()
    {
        Debug.Log("handle interaction");
        if (_targets.Count == 0)
        {
            _currentTarget = null;
            return;
        }

        float bestDot = -2;

        for (int i = 0; i < _targets.Count; i++)
        {
            Vector3 dir = _targets[i].Position - transform.position;
            dir.Normalize();
            float dot = Vector3.Dot(transform.forward, dir);

            if (dot <= 0.25f)
            {
                continue;
            }

            if (dot > bestDot)
            {
                _currentTarget = _targets[i];
                bestDot = dot;
            }
        }

        if (GameManager.instance.state == GameState.Gameplay)
            _currentTarget?.Interaction();
    }

    public void RemoveTarget(IInteract target)
    {
        _targets.Remove(target);
        Debug.Log("Poped: " + target);
        if (_currentTarget == target)
        {
            _currentTarget = null;
        }
    }
}
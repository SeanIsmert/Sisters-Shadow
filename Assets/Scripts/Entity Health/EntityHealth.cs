using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Umbrella script meant to be used by any object that needs to have health and/or take damage.
/// @author Kay.
/// </summary>
public class EntityHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _curHealth;                           // This object's current health value.
    [SerializeField] private float _maxHealth;                         // This object's maximum health value.

    public int GetEntityHealth {  get { return _curHealth; } }
    public int SetEntityHealth { set { _curHealth = value; } }

    [SerializeField] private UnityEvent<float> _onHealthChange;        // Event called when this object takes damage.
    [SerializeField] private UnityEvent _onDeathActions;               // Event called when this object's health reaches zero.

    private void Awake()
    {
        //_curHealth = (int)_maxHealth;        // Set max health on awake.
    }

    public void ValueChange(int amount)
    {
        _curHealth = (int)Mathf.Clamp(_curHealth -= amount, 0, _maxHealth);     // Subtract damage amount from current health.

        _onHealthChange?.Invoke(_curHealth/_maxHealth);      // Send normalized value through the onHealthChange event.

        if(_curHealth <= 0)
            _onDeathActions?.Invoke();                       // Call death event when health reaches zero.
    }
}

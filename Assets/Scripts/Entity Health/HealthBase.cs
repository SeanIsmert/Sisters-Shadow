using UnityEngine;
using UnityEngine.Events;

public class HealthBase : MonoBehaviour, IDamageable
{
    [SerializeField] private int _curHealth;        // This object's current health value.
    [SerializeField] private float _maxHealth;        // This object's maximum health value.

    [SerializeField] private UnityEvent<float> _onHealthChange;        // Event called when this object takes damage.
    [SerializeField] private UnityEvent _onDeathActions;               // Event called when this object's health reaches zero.

    private void Awake()
    {
        _curHealth = (int)_maxHealth;        // Set max health on awake.
    }

    public void ValueChange(int amount)
    {
        _curHealth -= amount;           // Subtract damage amount from current health.

        _onHealthChange?.Invoke(_curHealth/_maxHealth);      // Call the onHealthChange event.
        Debug.Log(_curHealth / _maxHealth);

        if(_curHealth <= 0)
            _onDeathActions?.Invoke();      // Call death event when health reaches zero.
    }
}

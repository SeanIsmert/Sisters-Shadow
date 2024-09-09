using UnityEngine.Events;
using UnityEngine;

public class StateTrigger : MonoBehaviour
{
    #region Variables
    [Tooltip("Used to change the Game state on enter.")]
    [SerializeField] private GameState _newState;
    [Tooltip("Used to call code elsewhere or intialize it.")]
    [SerializeField] private UnityEvent _onStateChanged;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.UpdateGameState(_newState);
            _onStateChanged?.Invoke();
        }
    }
}
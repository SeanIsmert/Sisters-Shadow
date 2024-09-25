using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthCircle;

    // Start is called before the first frame update
    void Start()
    {
        _healthCircle.color = new Color(_healthCircle.color.r, _healthCircle.color.g, _healthCircle.color.b, 0f);
    }

    public void UpdateHealthUI(float health)
    {
        float healthAlpha = _healthCircle.color.a;
        healthAlpha += health;
    }
}

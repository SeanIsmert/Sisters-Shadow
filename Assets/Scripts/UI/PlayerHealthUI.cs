using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthCircle;

    // Start is called before the first frame update
    void Start()
    {
        _healthCircle.color = new Color(1f, 1f, 1f, 0f);
    }

    public void UpdateHealthUI(float health)
    {
        float imageAlpha = (255f - (health * 255f)) / (255f - 0f);      // Reverse normalization wizardry.

        _healthCircle.color = new Color(1f, 1f, 1f, imageAlpha);        // Apply result to image alpha value.
    }
}

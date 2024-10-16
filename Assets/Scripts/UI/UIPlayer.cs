using UnityEngine.UI;
using UnityEngine;

public class UIPlayer : MonoSinglton<UIPlayer>
{
    [Header("Player UI")]
    [SerializeField] private Image _healthCircle;

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

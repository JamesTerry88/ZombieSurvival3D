using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth player;
    public Slider slider;

    void Start()
    {
        if (!slider) slider = GetComponent<Slider>();
        if (!player) player = FindObjectOfType<PlayerHealth>();
        if (player && slider)
        {
            slider.minValue = 0;
            slider.maxValue = player.maxHealth;
            slider.value   = player.currentHealth;
        }
    }

    void Update()
    {
        if (player && slider)
        {
            slider.maxValue = player.maxHealth;    // in case you change max later
            slider.value    = player.currentHealth;
        }
    }
}

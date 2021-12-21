using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Image healthBarBackground;
    private void Start()
    {
        PlayerHealth.onDamageTaken += UpdateHealthbar;
        PlayerHealth.onHeal += UpdateHealthbar;
        PlayerHealth.onRespawn += UpdateHealthbar;
        PlayerHealth.onRespawn += ToggleHealthBar;
        PlayerHealth.onDeath += ToggleHealthBar;
    }

    public void UpdateHealthbar(PlayerHealth playerHealth)
    {
        healthBar.fillAmount = playerHealth.health / playerHealth.maxHealth;
    }

    public void ToggleHealthBar(PlayerHealth playerHealth)
    {
        healthBar.enabled = !playerHealth.isCurrentlyDead;
        healthBarBackground.enabled = !playerHealth.isCurrentlyDead;
    }

    private void OnDestroy()
    {
        PlayerHealth.onDamageTaken -= UpdateHealthbar;
        PlayerHealth.onHeal -= UpdateHealthbar;
        PlayerHealth.onRespawn -= UpdateHealthbar;
        PlayerHealth.onRespawn -= ToggleHealthBar;
        PlayerHealth.onDeath -= ToggleHealthBar;
    }
}

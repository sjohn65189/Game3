using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public float Health, MaxHealth, Width, Height;
    public Color LowSeverityColor;
    public Color MediumSeverityColor;
    public Color HighSeverityColor;

    [SerializeField]
    private RectTransform healthBar;
    private Image healthBarImage;

    private void Awake()
    {
        healthBarImage = healthBar.GetComponent<Image>();
        healthBarImage.color = LowSeverityColor;
    }

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);

        // Adjust color transparency based on health
        float healthPercentage = Health / MaxHealth;
        //healthBarImage.color = Color.Lerp(HighSeverityColor, LowSeverityColor, healthPercentage);
    }
}


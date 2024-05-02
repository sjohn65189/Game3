using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
	public float Health, MaxHealth, Width, Height;

	[SerializeField]
	private RectTransform healthBar;
	private Image healthBarImage;
	private Material healthBarMaterial;

	private void Awake()
	{
		healthBarImage = healthBar.GetComponent<Image>();
		healthBarMaterial = healthBarImage.material;
		healthBarMaterial.SetFloat("_Health", 1);
	}

	public void SetMaxHealth(float maxHealth)
	{
		MaxHealth = maxHealth;
	}

	public void SetHealth(float health)
	{
		healthBarMaterial.SetFloat("_Health", health/100);
/*
		Health = health;
		float newWidth = (Health / MaxHealth) * Width;

		healthBar.sizeDelta = new Vector2(newWidth, Height);

		// Adjust color transparency based on health
		float healthPercentage = Health / MaxHealth;
		healthBarMaterial.SetFloat("_Health", healthPercentage);
*/
	}
}


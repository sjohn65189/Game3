using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float Health, MaxHealth;

    [SerializeField]
    private HealthBarUI healthBar;
    private Image healthBarImage;
    private Color targetColor;
    private bool isColorChanging = false;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(MaxHealth);
        healthBarImage = healthBar.GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void SetHealth(float healthChange) {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
    }
    
    IEnumerator ChangeHealthBarColor(Color targetColor)
    {
        isColorChanging = true;
        Color startColor = healthBarImage.color;
        float elapsedTime = 0f;
        float duration = 1f; // Adjust the duration of color change as needed

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthBarImage.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        healthBarImage.color = targetColor; // Ensure final color is set accurately
        isColorChanging = false;
    }
}

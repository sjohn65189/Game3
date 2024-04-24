using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float Health, MaxHealth;
    public Bee bee;

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
        StartCoroutine(DecreaseHealthOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (bee.severity == Bee.SeverityLevel.Low)
        {
            targetColor = healthBar.LowSeverityColor;
        }
        else if (bee.severity == Bee.SeverityLevel.Medium)
        {
            targetColor = healthBar.MediumSeverityColor;
        }
        else if (bee.severity == Bee.SeverityLevel.High)
        {
            targetColor = healthBar.HighSeverityColor;
        }

        if (healthBarImage.color != targetColor)
        {
            if (!isColorChanging)
            {
                StartCoroutine(ChangeHealthBarColor(targetColor));
            }
        }

        if (Input.GetKeyDown("m")) {
            SetHealth(-2.5f);
        }

        if (Input.GetKeyDown("n")) {
            SetHealth(2.5f);
        }
        
    }

    public void SetHealth(float healthChange) {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
    }

    private IEnumerator DecreaseHealthOverTime()
    {
        while (true)
        {
            float decreaseAmount = 0f;

            // Adjust decrease amount based on bee severity
            if (bee.severity == Bee.SeverityLevel.Low)
            {
                decreaseAmount = 1f;
            }
            else if (bee.severity == Bee.SeverityLevel.Medium)
            {
                decreaseAmount = 2f;
            }
            else if (bee.severity == Bee.SeverityLevel.High)
            {
                decreaseAmount = 3f;
            }

            SetHealth(-decreaseAmount);

            // Wait for 1 second before decreasing health again
            yield return new WaitForSeconds(1f);
        }
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

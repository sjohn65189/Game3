using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;

    private static List<string> collectedItems = new List<string>(); // Static list to keep track of collected items across all instances

    public string itemName; //Name of Item
    public Image[] uiSprites; // Array of UI sprites to display collected items

    public AudioClip pickupSound; // Add this field to hold the pickup sound
    public AudioSource audioSource; // Add this field to reference the AudioSource component

    public GameObject FloatingTextPrefab; // This hold the floating text prefab in unity prefabs folder

    public enum ItemPower
    {
        None,
        PlayerSpeedup,
        YetiSlowDown,
    }

    public ItemPower itemPower;

    private PlayerController playerController;
    public Yeti yeti;

    private float powerUpDuration = 10f; //duration of powerup
    public bool isPowerUpActive = false; //flag to track if active
    private float powerUpTimer = 0f; //timer

    public GameObject pickUpMessage;
    void Start()
    {
        // Find the PlayerController and Yeti components
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found");
        }

        input = new PlayerInputActions();
        input.Enable();

        //Reset items list when the game is reset
        ResetCollectedItems();
        pickUpMessage.SetActive(false);
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        if (isPowerUpActive)
        {
            powerUpTimer += Time.deltaTime;
            if (powerUpTimer >= powerUpDuration)
            {
                DeactivatePowerUp();
            }
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (input.PlayerActions.PickUpItem.IsPressed())
            {
                CollectItem();
                // Play the pickup sound
                if (pickupSound != null && audioSource != null)
                {
                    audioSource.volume = 0.5f;
                    audioSource.clip = pickupSound;
                    audioSource.Play();
                }

                // Add to score for picking up relic
                ScoreManagerExpanded.instance.GetRelic();

                // Add to the artifacts grabbed count
                ScoreManagerExpanded.instance.ArtifactGrabbed();

                // Show floating text
                if (FloatingTextPrefab)
                {
                    ShowFloatingText();
                }
            }
        }
    }

    void CollectItem()
    {
        //check if item has already been collected
        if (!collectedItems.Contains(itemName))
        {
            // Add the item to the list of collected items
            collectedItems.Add(itemName);

            // Disable the collected item's renderer and collider
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            ActivateItemPower();
        }
    }

    void ActivateItemPower()
    {
        switch (itemPower)
        {
            case ItemPower.PlayerSpeedup:
                //increase player speed to 7
                playerController.moveSpeed = 7f;
                Debug.Log("Increased speed!");
                StartPowerUpTimer();
                break;
            case ItemPower.YetiSlowDown:
                //slow yeti speed down
                yeti.AdjustSpeed(3.0f);
                Debug.Log("Yeti speed decreased!");
                StartPowerUpTimer();
                break;
            default:
                break;

        }
    }

    void StartPowerUpTimer()
    {
        Debug.Log("Player has powerup");
        isPowerUpActive = true;
        powerUpTimer = 0f;
    }

    void DeactivatePowerUp()
    {
        Debug.Log("Player no longer has powerup");
        isPowerUpActive = false;
        powerUpTimer = 0f;
        if (itemPower == ItemPower.PlayerSpeedup)
        {
            playerController.moveSpeed = 5f;
        }
        if (itemPower == ItemPower.YetiSlowDown)
        {
            yeti.AdjustSpeed(5.5f);
        }
    }
    void ResetCollectedItems()
    {
        //clear collected items list when game is restarted
        collectedItems.Clear();
    }

    void ShowFloatingText()
    {
        Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			pickUpMessage.SetActive(true);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			pickUpMessage.SetActive(false);
		}
	}
}

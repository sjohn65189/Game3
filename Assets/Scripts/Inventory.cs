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
	void Start() 
	{
		input = new PlayerInputActions();
		input.Enable();

		//Reset items list when the game is reset
		ResetCollectedItems();
	}
	
	private void OnDisable()
	{
		input.Disable();
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
			}
		}
	}

	void CollectItem() {
		//check if item has already been collected
		if (!collectedItems.Contains(itemName))
		{
			// Add the item to the list of collected items
			collectedItems.Add(itemName);

			// Disable the collected item's renderer and collider
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;

			// Update the UI to display the collected items
			UpdateCollectedItemsUI();
		}
	}

	void UpdateCollectedItemsUI()
	{
		// Print the names of collected items to the console for demonstration
		foreach (var item in collectedItems)
		{
			Debug.Log("Collected Item: " + item);
		}
		// Loop through all collected items and update UI sprites accordingly
		for (int i = 0; i < collectedItems.Count && i < uiSprites.Length; i++)
		{
			// Assuming your items have a sprite associated with their name
			Sprite itemSprite = Resources.Load<Sprite>("ArtifactSprites/" + collectedItems[i]); // Adjust the path as needed

			// Assign the collected item sprite to the corresponding UI sprite
			uiSprites[i].sprite = itemSprite;
		}
	}

	void ResetCollectedItems() 
	{
		//clear collected items list when game is restarted
		collectedItems.Clear();
	}
}

using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BridgeSystem : MonoBehaviour
{
	[HideInInspector] public PlayerInputActions input;
	public Vector3Int targetTilePosition; //position the plank will land on the bridge
	public Tilemap groundTilemap;
	public Tilemap groundOverlayTilemap;
	public Tilemap colliderTilemap;
	public float rotationAngle; // The angle by which you want to rotate the item
	public AudioClip pickupSound; // Add this field to hold the pickup sound
	public AudioSource audioSource; // Add this field to reference the AudioSource component

	public int requiredPlanksBridge1 = 3;
	public int requiredPlanksBridge2 = 7;



	private TextMeshProUGUI plankCounter1;
	private TextMeshProUGUI plankCounter2;
	private TextMeshProUGUI plankCounter3;
	private bool hasCollided = false;
	private bool counted = false;
	private NavMeshSurface navMeshSurface; //reference to the NavMeshSurface component
	private Rigidbody2D plankRigidbody; // Reference to the Rigidbody2D component
	private int SFXEnabled;
	private GameObject placedPlanks;

	public GameObject pickUpMessage;
	private void Start()
	{
		placedPlanks = GameObject.FindGameObjectWithTag("PlacedPlanks");
		SFXEnabled = PlayerPrefs.GetInt("SFXEnabled", 1);
		input = new PlayerInputActions();
		input.Enable();
		
		// Get counter objects
		plankCounter1 = GameObject.FindGameObjectWithTag("PlankCounter1").GetComponent<TextMeshProUGUI>();
		plankCounter2 = GameObject.FindGameObjectWithTag("PlankCounter2").GetComponent<TextMeshProUGUI>();
		plankCounter3 = GameObject.FindGameObjectWithTag("PlankCounter3").GetComponent<TextMeshProUGUI>();
		
		// Get the Rigidbody2D component attached to this GameObject
		plankRigidbody = GetComponent<Rigidbody2D>();

		//find navMeshSurface in scene
		navMeshSurface = FindObjectOfType<NavMeshSurface>();

		pickUpMessage.SetActive(false);
		
		var targetPlank = placedPlanks.transform.Find(gameObject.name);
		targetPlank.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	private void OnDisable()
	{
		input.Disable();
	}
	
	private void Update()
	{
		// Check if the "G" key is pressed and the collision flag is set
		if (input.PlayerActions.PickUpItem.IsPressed() && hasCollided && !counted)
		{
			// Play the pickup sound
			if (pickupSound != null && audioSource != null && SFXEnabled == 1)
			{
				audioSource.volume = 0.5f;
				audioSource.clip = pickupSound;
				audioSource.Play();
			}
			
			// Move to target tile and add to corresponding counter
			MoveToTargetTile();
			hasCollided=false;
			counted = true;

			switch (gameObject.tag) 
			{
				case "area1":
					int currentCount1 = int.Parse(plankCounter1.text);
					plankCounter1.text = (currentCount1 += 1).ToString();
					break;
				case "area2":
					int currentCount2 = int.Parse(plankCounter2.text);
					plankCounter2.text = (currentCount2 += 1).ToString();
					break;
				case "area3":
					int currentCount3 = int.Parse(plankCounter3.text);
					plankCounter3.text = (currentCount3 += 1).ToString();
					break;
			}
			


			// Rebuild the NavMesh after moving the item
			if (navMeshSurface != null)
			{
				navMeshSurface.BuildNavMesh();
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !hasCollided)
		{
			pickUpMessage.SetActive(true);
			hasCollided=true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			pickUpMessage.SetActive(false);
			hasCollided = false;
		}
	}

	private void MoveToTargetTile()
	{
		// Disable sprite
		GetComponent<SpriteRenderer>().sprite = null;
		
		// Enable target plank sprite
		EnableTargetPlank();
		
		// Move the item to the target tile position
		Vector3 targetWorldPosition = groundTilemap.GetCellCenterWorld(targetTilePosition);
		transform.position = targetWorldPosition;

		// Rotate the item along z axis
		transform.eulerAngles = new Vector3(0f, 0f, rotationAngle);

		// Disable collider at tile positions (target tile + 1 tile above and below)
		for (int i = -1 ; i<2 ; i++) 
		{
			colliderTilemap.SetTile(new Vector3Int(targetTilePosition.x, targetTilePosition.y-i, targetTilePosition.z), null);
		}
	}
	
	// Enable the sprite of the target plank
	private void EnableTargetPlank()
	{
		var targetPlank = placedPlanks.transform.Find(gameObject.name);
		targetPlank.GetComponent<SpriteRenderer>().enabled = true;
	}
	
	// Methods to check if each bridge is complete
	public bool IsBridge1Complete()
	{
		int currentCount1 = int.Parse(plankCounter1.text);
		return currentCount1 == requiredPlanksBridge1;
	}

	public bool IsBridge2Complete()
	{
		int currentCount2 = int.Parse(plankCounter2.text);
		int currentCount3 = int.Parse(plankCounter3.text);
		return (currentCount2 + currentCount3) == requiredPlanksBridge2;
	}

}

using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using TMPro;

public class BridgeSystem : MonoBehaviour
{
	public Vector3Int targetTilePosition; //position the plank will land on the bridge
	public Tilemap groundTilemap;
	public Tilemap groundOverlayTilemap;
	public Tilemap colliderTilemap;
	public float rotationAngle; // The angle by which you want to rotate the item
	public AudioClip pickupSound; // Add this field to hold the pickup sound
	public AudioSource audioSource; // Add this field to reference the AudioSource component


	private TextMeshProUGUI plankCounter1;
	private TextMeshProUGUI plankCounter2;
	private bool hasCollided = false;
	private bool counted = false;
	private NavMeshSurface navMeshSurface; //reference to the NavMeshSurface component
	private Rigidbody2D plankRigidbody; // Reference to the Rigidbody2D component

	private void Start()
	{
		// Get counter objects
		plankCounter1 = GameObject.FindGameObjectWithTag("PlankCounter1").GetComponent<TextMeshProUGUI>();
		plankCounter2 = GameObject.FindGameObjectWithTag("PlankCounter2").GetComponent<TextMeshProUGUI>();
		
		// Get the Rigidbody2D component attached to this GameObject
		plankRigidbody = GetComponent<Rigidbody2D>();

		//find navMeshSurface in scene
		navMeshSurface = FindObjectOfType<NavMeshSurface>();
	}
	private void Update()
	{
		// Check if the "G" key is pressed and the collision flag is set
		if (Input.GetKeyDown(KeyCode.G) && hasCollided && !counted)
		{
			// Play the pickup sound
			if (pickupSound != null && audioSource != null)
			{
				audioSource.clip = pickupSound;
				audioSource.Play();
			}
			
			// Move to target tile and add to corresponding counter
			MoveToTargetTile();
			hasCollided=false;
			counted = true;
			
			switch (gameObject.tag) 
			{
				case "bridge1":
					int currentCount1 = int.Parse(plankCounter1.text);
					plankCounter1.text = (currentCount1 += 1).ToString();
					break;
				case "bridge2":
					int currentCount2 = int.Parse(plankCounter2.text);
					plankCounter2.text = (currentCount2 += 1).ToString();
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
			hasCollided=true;
			
		}
	}

	private void MoveToTargetTile()
	{
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
}

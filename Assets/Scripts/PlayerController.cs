using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
	[HideInInspector] public PlayerInputActions input;
	public Tilemap groundTilemap;
	public Tilemap groundOverlayTilemap;
	
	public NavMeshSurface navSurface;

	public float thresholdDistance = 1f; // Distance to yeti to decrease health
	public float moveSpeed = 5f;
	public Transform movePoint;

	public AudioClip snowStepping;
	public AudioSource audioSource;
	
	public PlayerHealth playerHealth;
	public GameManager gameManager;
	public Yeti yeti;
	
	public CameraShake cameraShaker;
	private float maxDistance = 7.0f;

	public LayerMask whatStopsMovement;
	[HideInInspector] public List<Vector3> wanderPoints; // This list of points is what the yeti follows when chasing the player

	[HideInInspector]public Tile playerCurrentTile;
	private Sprite flatSnowSprite;
	public GameObject yetiNearby;
	YetiClose yetiN;
	
	private int SFXEnabled;
	
	void Start()
	{
		input = new PlayerInputActions();
		input.Enable();
		SFXEnabled = PlayerPrefs.GetInt("SFXEnabled", 1);
		playerHealth = GetComponent<PlayerHealth>();
		navSurface.hideEditorLogs = true;
		movePoint.parent = null;
		flatSnowSprite =  Resources.Load<Sprite>("GroundSprites/darkbluegreen.png");

		// Ensure the AudioSource component is assigned
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
			if (audioSource == null)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
			}
		}

		yetiNearby = GameObject.Find("YetiNearbyMaterial");
		yetiN = yetiNearby.GetComponent<YetiClose>();
	}

	void Awake()
	{
		wanderPoints = new List<Vector3>();
		

		playerCurrentTile = groundTilemap.GetTile<Tile>(Vector3Int.FloorToInt(movePoint.position));
		
		StartCoroutine(DropWanderPoints());
	}

	private void OnDisable()
	{
		input.Disable();
	}

	void Update()
	{
		// Calculate the distance between object1 and object2
		float distance = Vector3.Distance(transform.position, yeti.transform.position);
		
		// If camera close to yeti, we shake based on distance
		if (distance > maxDistance && yeti.gameObject.activeSelf) 
		{
			cameraShaker.intensity = 0f;
		}
		else if (yeti.gameObject.activeSelf)
		{
			cameraShaker.intensity = Mathf.Lerp(0f, 8.0f, 1.0f - (distance / maxDistance));
		}

		// Check if the distance is less than the threshold distance
		if (distance < thresholdDistance && yeti.gameObject.activeSelf) 
		{
			CatchPlayer();
		}
		
		// Move player towards movepoint
		transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
		
		bool didMove = false;

		if(Vector3.Distance(transform.position, movePoint.position) <= .05f)
		{
			// Check movement in order (up, down, left, right)
			if(input.PlayerActions.MoveUp.IsPressed())
			{
				// If no object in front of us, move
				if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f, 0f), .2f, whatStopsMovement))
				{
					movePoint.position += new Vector3(0f, 1f, 0f);
					didMove = true;
				}
			}
			else if(input.PlayerActions.MoveDown.IsPressed())
			{
				if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f, 0f), .2f, whatStopsMovement))
				{
					movePoint.position += new Vector3(0f, -1f, 0f);
					didMove = true;
				}
			}
			else if(input.PlayerActions.MoveLeft.IsPressed())
			{
				if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), .2f, whatStopsMovement))
				{
					movePoint.position += new Vector3(-1f, 0f, 0f);
					didMove = true;
				}
			}
			else if(input.PlayerActions.MoveRight.IsPressed())
			{
				if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), .2f, whatStopsMovement))
				{
					movePoint.position += new Vector3(1f, 0f, 0f);
					didMove = true;
				}
			}
		}

		//playerCurrentTile = groundTilemap.GetTile<Tile>(Vector3Int.FloorToInt(movePoint.position));
		if (didMove) 
		{
			if (SFXEnabled == 1) 
			{
				PlayFootstepSound();
			}
			
			if (groundOverlayTilemap.GetTile<Tile>(Vector3Int.FloorToInt(movePoint.position)) != null) 
			{
				groundOverlayTilemap.SetTile(Vector3Int.FloorToInt(movePoint.position), null);
				StartCoroutine(RenderNavMesh());
			}
		}
	}
	
	void PlayFootstepSound()
	{
		// Check if footstep sound and audio source are assigned
		if (snowStepping != null && audioSource != null)
		{
			// Assign footstep sound to audio source
			audioSource.clip = snowStepping;
			// Play the footstep sound
			audioSource.Play();
		}
	}
	IEnumerator RenderNavMesh()
	{
		// Change 2D bounds to be around player position
		navSurface.BuildNavMesh();
		yield return null;
	}

	IEnumerator DropWanderPoints()
	{
		while (true)
		{
			if (!wanderPoints.Contains(movePoint.transform.position)) 
			{
				// Append the current position to the wanderPoints list
				wanderPoints.Add(movePoint.transform.position);
			}

			// Wait for 5 seconds
			yield return new WaitForSeconds(4f);
		}
	}
	
	public void ClearWanderPoints() 
	{
		wanderPoints = new List<Vector3>();
	}
	
	public void CatchPlayer() 
	{
		if (SFXEnabled == 1) 
		{
			gameManager.PlayCatchSounds();
		}
		
		// If health above 0, decrease by 25 otherwise gameover
		if (playerHealth.Health > 0) 
		{
			cameraShaker.intensity = 0f;
			yetiN.material.SetFloat("_VignettePower", yetiN.maxFloatValue);
			playerHealth.SetHealth(-25);
			transform.position = new Vector3(0, 0, 0);
			movePoint.transform.position = new Vector3(0, 0, 0);
			gameManager.YetiStart(); // Reset yeti
		}
		if (playerHealth.Health <= 0) 
		{
			cameraShaker.intensity = 0f;
			gameManager.Gameover();
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision) 
	{
		// Win condition
		if (collision.gameObject.CompareTag("WinZone")) 
		{
			gameManager.Victory();
			
		}
		
		// Reset wander points when crossing between areas
		if (collision.gameObject.CompareTag("WanderReset")) 
		{
			yeti.WanderResetRoar();
			ClearWanderPoints();
		}
	}
}

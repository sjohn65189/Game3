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

	public float moveSpeed = 5f;
	public Transform movePoint;

	public AudioClip snowStepping;
	public AudioSource audioSource;
	
	public PlayerHealth playerHealth;
	public Menus menus;
	public Yeti yeti;

	public LayerMask whatStopsMovement;
	[HideInInspector] public List<Vector3> wanderPoints; // This list of points is what the yeti follows when chasing the player

	[HideInInspector]public Tile playerCurrentTile;
	private Sprite flatSnowSprite;
	
	void Start()
	{
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
	}

	void Awake()
	{
		wanderPoints = new List<Vector3>();
		input = new PlayerInputActions();
		input.Enable();

		playerCurrentTile = groundTilemap.GetTile<Tile>(Vector3Int.FloorToInt(movePoint.position));
		
		StartCoroutine(DropWanderPoints());
	}

	private void OnDisable()
	{
		input.Disable();
	}

	void Update()
	{
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
			PlayFootstepSound();
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
			yield return new WaitForSeconds(5f);
		}
	}
	
	public void ClearWanderPoints() 
	{
		wanderPoints = new List<Vector3>();
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// On collision with Yeti, lose health
		if (collision.gameObject.CompareTag("Yeti")) {
			// If health above 0, decrease by 25 otherwise gameover
			if (playerHealth.Health > 0) 
			{
				playerHealth.SetHealth(-25);
				transform.position = new Vector3(0, 0, 0);
				movePoint.transform.position = new Vector3(0, 0, 0);
				yeti.ResetPosition();
			}
			else 
			{
				menus.Gameover();
			}
		}
	}
}

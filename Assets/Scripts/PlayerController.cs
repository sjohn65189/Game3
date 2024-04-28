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

	public LayerMask whatStopsMovement;

	[HideInInspector]public Tile playerCurrentTile;
	private Sprite flatSnowSprite;
	
	void Start()
	{
		movePoint.parent = null;
		flatSnowSprite =  Resources.Load<Sprite>("GroundSprites/darkbluegreen.png");
	}

	void Awake()
	{
		input = new PlayerInputActions();
		input.Enable();

		playerCurrentTile = groundTilemap.GetTile<Tile>(Vector3Int.FloorToInt(movePoint.position));
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
			if(groundOverlayTilemap.GetTile<Tile>(Vector3Int.FloorToInt(movePoint.position)) != null) 
			{
				groundOverlayTilemap.SetTile(Vector3Int.FloorToInt(movePoint.position), null);
				navSurface.BuildNavMesh();
			}
		}
	}
}

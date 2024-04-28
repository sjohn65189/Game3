using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class Yeti : MonoBehaviour
{
	public GameObject player;
	public LayerMask snowLayer;
	
	public float moveSpeed = 6f;
	public Transform movePoint;
	
	NavMeshAgent agent;
	
	private PlayerController playerController;
	private Vector3 playerLastPosition; // Last position Yeti saw player
	private bool hasReachedLastPosition = false; // Determines if Yeti has reached last position
	
	// Debug variable
	private bool isDebug = true;

	// Start is called before the first frame update
	void Start()
	{
		playerLastPosition = player.transform.position;
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		
		playerController = player.GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		agent.SetDestination(playerLastPosition);
	}

	void FixedUpdate() 
	{
		// Check if Yeti can see player
		CheckForPlayer();
		
		// If Yeti has reached player's last position, begin wandering
		if (playerLastPosition == null || hasReachedLastPosition == true) 
		{
			
		}
	}
	
	void CheckForPlayer() 
	{
		// Cast ray towards player
		RaycastHit2D raycastHit2D = Physics2D.Linecast(transform.position, player.transform.position, snowLayer);
		
		// If debug, draw line
		if (isDebug) 
		{
			Debug.DrawLine(transform.position, player.transform.position, Color.red);
		}
		
		// Check for collision with snow
		if (raycastHit2D.collider != null)
		{
			// Collision with snow detected
			if (isDebug) 
			{
				Debug.Log("Collision with snow detected!");
			}
		}
		else
		{
			// No collision with snow, update player's last position
			playerLastPosition = playerController.movePoint.transform.position;
		}
	}
	
	// Yeti will wander around the carved path until it sees the player again
	void Wander() 
	{
		
	}
}

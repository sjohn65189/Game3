using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class Yeti : MonoBehaviour
{
	public GameObject player;
	public LayerMask snowLayer;
	public AudioClip yetiSound;
	public AudioSource audioSource;
	
	public AudioSource wanderResetRoar;

	public float yetiSoundDelay = 3.0f;
	private bool canPlaySound = true;
	
	NavMeshAgent agent;
	
	private PlayerController playerController;
	private Vector3 target; // Last position Yeti saw player
	private bool hasReachedLastPosition = false; // Determines if Yeti has reached last position
	private bool canSeePlayer = false; // Check if Yeti can see player
	private float distanceCheck = 0.5f; // Distance check
	
	// Debug variable
	private bool isDebug = false;

	public GameObject yetiNearby;
	YetiClose yetiN;
//	Material yetiNMat;

	// Start is called before the first frame update
	void Start()
	{
		target = player.transform.position;
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		
		playerController = player.GetComponent<PlayerController>();

		// Ensure AudioSource component is assigned
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		yetiNearby = GameObject.Find("YetiNearbyMaterial");
		yetiN = yetiNearby.GetComponent<YetiClose>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Vector2.Distance(transform.position, target) <= distanceCheck) 
		{
			hasReachedLastPosition = true;
		}
		
		if (hasReachedLastPosition == true && !canSeePlayer) 
		{
			target = GetWanderPoint();
		}
		
		hasReachedLastPosition = false;
		agent.SetDestination(target);

		// Play the Yeti sound with delay
		if (canSeePlayer && canPlaySound && yetiSound != null && audioSource != null)
		{
			StartCoroutine(PlaySoundWithDelay(yetiSoundDelay));
		}
	}

	void FixedUpdate() 
	{
		// Check if Yeti can see player
		CheckForPlayer();
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

		float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
		// Check for collision with snow
		if (raycastHit2D.collider != null)
		{
			canSeePlayer = false;
			float normalizedDistance = Mathf.Clamp01(distanceToPlayer / 5f);
			float yetiAway = Mathf.Lerp(yetiN.minFloatValue, yetiN.maxFloatValue, normalizedDistance);
			yetiN.material.SetFloat("_VignettePower", yetiAway);
			//Debug.Log("Yeti Away:" + yetiAway);
			agent.speed = 5.5f;
			// Collision with snow detected
			if (isDebug) 
			{
				Debug.Log("Collision with snow detected!");
			}
		}
		else
		{
			// No collision with snow, update player's last position
			canSeePlayer = true;
			agent.speed = 7.5f;
			float normalizedDistance = Mathf.Clamp01(distanceToPlayer / 5f);
			float yetiClose = Mathf.Lerp(yetiN.minFloatValue, yetiN.maxFloatValue, normalizedDistance);
			Debug.Log("Yeti Close:" + yetiClose);
			yetiN.material.SetFloat("_VignettePower", yetiClose);
			target = playerController.movePoint.transform.position;

		}
	}
	
	// Yeti will wander around the carved path until it sees the player again
	Vector3 GetWanderPoint() 
	{
		// Get random point from wander points
		List<Vector3> wanderPoints = playerController.wanderPoints;
		
		// Instantiate wander point to player movePoint (this will be used if list is empty to prevent Yeti from standing still)
		Vector3 result = playerController.movePoint.transform.position;
		
		// Check that wanderPoints is not empty
		if (wanderPoints.Count > 0) 
		{
			int randIndex = Random.Range(0, wanderPoints.Count-1);
			
			// Get result and delete from list
			result = wanderPoints[randIndex];
			wanderPoints.Remove(result);
			playerController.wanderPoints = wanderPoints;
		}
		
		return result;
	}

	IEnumerator PlaySoundWithDelay(float delay)
	{
		canPlaySound = false;
		yield return new WaitForSeconds(delay);
		audioSource.clip = yetiSound;
		audioSource.Play();
		yield return new WaitForSeconds(audioSource.clip.length);
		canPlaySound = true;
	}
	
	// This will reset Yeti to starting position 
	public void ResetPosition() 
	{
		StartCoroutine(DelayChase());
	}
	
	public IEnumerator DelayChase()
	{
		gameObject.SetActive(false);
		transform.position = new Vector3(-1, 0, 0);
		yield return new WaitForSeconds(5f);
		gameObject.SetActive(true);
	}
	
	// Roar when player crosses a wander reset zone (yeti sound 3)
	public void WanderResetRoar() 
	{
		wanderResetRoar.Play();
	}
}

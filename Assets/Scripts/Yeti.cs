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
	public AudioSource stompingSound;

	public AudioSource wanderResetRoar;

	public float yetiSoundDelay = 3.0f;
	private bool canPlaySound = true;

	NavMeshAgent agent;

	private PlayerController playerController;
	private Vector3 target; // Last position Yeti saw player
	private bool hasReachedLastPosition = false; // Determines if Yeti has reached last position
	private bool canSeePlayer = false; // Check if Yeti can see player
	private float distanceCheck = 0.5f; // Distance check
	public Inventory inventory;

	// Debug variable
	private bool isDebug = false;
	private int SFXEnabled;

	public GameObject yetiNearby;
	YetiClose yetiN;
	
	// Simple animation stuff
	public float maxRotationAngle = 15f; // Maximum rotation angle
	public float maxRotationSpeed = 200f; // Maximum rotation speed in degrees per second at max character speed
	private float currentRotationAngle = 0f; // Current rotation angle
	private int rotationDirection = 1; // Current rotation direction (1 for right, -1 for left)

	// Start is called before the first frame update
	void Start()
	{
		if (inventory == null)
		{
			Debug.LogError("Inventory reference not set");
		}

		SFXEnabled = PlayerPrefs.GetInt("SFXEnabled", 1);
		if (SFXEnabled != 1)
		{
			stompingSound.mute = true;
		}

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
		// Check if Yeti can see player
		CheckForPlayer();

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
		if (canSeePlayer && canPlaySound && yetiSound != null && audioSource != null && SFXEnabled == 1)
		{
			StartCoroutine(PlaySoundWithDelay(yetiSoundDelay));
		}
		
		// Normalize the speed to get a rotation speed proportional to the character's speed
		float rotationSpeed = (agent.velocity.magnitude / agent.speed) * maxRotationSpeed;

		// Calculate the rotation step for this frame
		float rotationStep = rotationSpeed * Time.deltaTime * rotationDirection;

		// Update the current rotation angle
		currentRotationAngle += rotationStep;

		// Reverse direction if the max rotation angle is reached
		if (currentRotationAngle > maxRotationAngle)
		{
			currentRotationAngle = maxRotationAngle;
			rotationDirection = -1;
		}
		else if (currentRotationAngle < -maxRotationAngle)
		{
			currentRotationAngle = -maxRotationAngle;
			rotationDirection = 1;
		}

		// Apply the rotation to the transform
		transform.rotation = Quaternion.Euler(0, 0, currentRotationAngle);
	}
	
	public void AdjustSpeed(float newSpeed)
	{
		agent.speed = newSpeed;
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
		if (!inventory.isPowerUpActive || inventory.itemPower != Inventory.ItemPower.YetiSlowDown)
		{

			if (raycastHit2D.collider != null)
			{
				canSeePlayer = false;
				float normalizedDistance = Mathf.Clamp01(distanceToPlayer / 7f);
				float yetiAway = Mathf.Lerp(yetiN.minFloatValue, yetiN.maxFloatValue, normalizedDistance + 0.1f);
				yetiN.material.SetFloat("_VignettePower", yetiAway);
				Debug.Log("Yeti Away:" + yetiAway);
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
				float normalizedDistance = Mathf.Clamp01(distanceToPlayer / 7f);
				float yetiClose = Mathf.Lerp(yetiN.minFloatValue, yetiN.maxFloatValue, normalizedDistance - 0.1f);
				Debug.Log("Yeti Close:" + yetiClose);
				yetiN.material.SetFloat("_VignettePower", yetiClose);
				target = playerController.movePoint.transform.position;

			}
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
			int randIndex = Random.Range(0, wanderPoints.Count - 1);

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

	// Roar when player crosses a wander reset zone (yeti sound 3)
	public void WanderResetRoar()
	{
		// Set target back to player
		if (gameObject.activeSelf)
		{
			agent.SetDestination(player.transform.position);

			if (SFXEnabled == 1)
			{
				wanderResetRoar.Play();
			}
		}
	}
}

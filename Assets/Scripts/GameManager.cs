using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public PlayerInputActions input;
	public GameObject VictoryMenu;
	public GameObject GameOverMenu;
	public GameObject pauseMenu;
	public GameObject VictoryMainMenuButton;
	public GameObject GameOverMainMenuButton;
	
	public bool paused = false;
	public GameObject Yeti;
	public PlayerController Player;
	public PlayerController playerController;
	
	public Tilemap ColliderMap;
	public Tilemap NonPlacementMap;
	public Tile RockTile;
	
	// Planks & Artifacts
	public List<GameObject> planksArea1;
	public List<GameObject> planksArea2;
	public List<GameObject> planksArea3;
	public List<GameObject> artifacts;
	public LayerMask placementLayer;
	public LayerMask stopMovementLayer;
	
	public AudioSource Main_Music;
	public AudioSource Wind_Sound;

	void Start()
	{
		BuildArea1();
		BuildArea2();
		BuildArea3();
		
		
		input = new PlayerInputActions();
		input.Enable();

		// Check if music and sound effects are enabled. If no value is set, return 1
		int musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1);
		int SFXEnabled = PlayerPrefs.GetInt("SFXEnabled", 1);
		
		if (musicEnabled == 1) 
		{
			Main_Music.Play();
		}
		if (SFXEnabled == 1)
		{
			Wind_Sound.Play();
		}
	}
	
	void Awake() 
	{
		YetiStart(); // This delays the yeti chase
	}
	
	// return to the Main menu
	public void MainMenuButtonClicked()
	{
		// Load game
		SceneManager.LoadScene("MainMenu");
	}
	
	// restart the game
	public void RestartButtonClicked()
	{
		// Load game
		SceneManager.LoadScene("GameScene");
	}
	
	// Quit the game
	public void QuitButtonClicked()
	{
		// if UNITY_EDITOR
		//UnityEditor.EditorApplication.isPlaying = false;
		// endif 
		Application.Quit();
	}
	
	public void Gameover() 
	{
		//stop timer in background
		Timer.instance.StopTimer();

		GameOverMenu.SetActive(true);
		
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(GameOverMainMenuButton);
		
		Player.gameObject.SetActive(false);
	}
	public void ResumeButtonClicked()
	{
		EventSystem.current.SetSelectedGameObject(null);
		pauseMenu.SetActive(false);
		Yeti.SetActive(true);
		paused = false;
		Player.input.Enable();
		Timer.instance.StartTimer();
	}

	public void Victory() 
	{

		//stop timer in background
		Timer.instance.StopTimer();

		//use time multiplier for score
		ScoreManagerExpanded.instance.TimeMultiplier((int)Timer.instance.elapsedTime);

		VictoryMenu.SetActive(true);
		
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(VictoryMainMenuButton);
		
		Yeti.SetActive(false);
		Player.gameObject.SetActive(false);

	}
	
	// Reset Yeti and give 5 second delay
	public IEnumerator DelayChase()
	{
		Yeti.SetActive(false);
		Yeti.transform.position = new Vector3(-1, 0, 0);
		yield return new WaitForSeconds(5f);
		if (!paused)
		{
			Yeti.SetActive(true);
		}
	}
	
	public void YetiStart() 
	{
		StartCoroutine(DelayChase());
	}
	
	// Below contains the code for building each area
	#region Area Item Placement
	public void BuildArea1() 
	{
		Vector3Int area1MinBounds = new Vector3Int(3, 6, 0);
		Vector3Int area1MaxBounds = new Vector3Int(19, -6, 0);
		
		// Area where items cannot spawn
		Vector3Int deadZoneMinBounds = new Vector3Int(19, 1, 0);
		Vector3Int deadZoneMaxBounds = new Vector3Int(19, -1, 0);
		
		// Rock tiles to place
		int numberOfRockTiles = 14;
		int placedRockTiles = 0;

		// Loop until number of tiles have been placed
		while (placedRockTiles < numberOfRockTiles)
		{
			int randomX;
			int randomY;
			Vector3Int position;

			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area1MinBounds.x, area1MaxBounds.x + 1);
				randomY = Random.Range(area1MinBounds.y, area1MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while (position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y);

			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				ColliderMap.SetTile(position, RockTile);
				placedRockTiles++;
			}
		}
		
		// Sync the tilemap collider changes
		ColliderMap.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
		
		// Planks to place
		int numberOfPlanks = 3;
		int placedPlanks = 0;
		List<Vector3Int> plankPositions = new List<Vector3Int>();
		
		while (placedPlanks < numberOfPlanks)
		{
			int randomX;
			int randomY;
			Vector3Int position;
			List<Vector3Int> currentPlankPositions;
			bool canBePlaced = true;
			
			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area1MinBounds.x, area1MaxBounds.x + 1);
				randomY = Random.Range(area1MinBounds.y, area1MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while (position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y);
			
			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				planksArea1[placedPlanks].transform.position = position;
				
				// Save the positions that the plank will be covering
				if (planksArea1[placedPlanks].transform.rotation.z == 0f) 
				{
					currentPlankPositions = new List<Vector3Int>()
					{
						new Vector3Int(position.x-1, position.y),
						position,
						new Vector3Int(position.x+1, position.y)
					};
				}
				else
				{
					currentPlankPositions = new List<Vector3Int>()
					{
						new Vector3Int(position.x, position.y-1),
						position,
						new Vector3Int(position.x, position.y+1)
					};
				}
				
				// If a collider or nonplacement tile exists at any plank position, we do not place the plank
				foreach (Vector3Int plankPos in currentPlankPositions) 
				{
					if (ColliderMap.GetColliderType(plankPos) != Tile.ColliderType.None || plankPositions.Contains(plankPos) || NonPlacementMap.GetTile(plankPos) != null) 
					{
						canBePlaced = false;
					}
				}
				
				// If we place the plank, we add the position to current plank positions
				if (canBePlaced) 
				{
					plankPositions.AddRange(currentPlankPositions);
					placedPlanks++;
				}
			}
		}
		
		// Artifacts to place
		int numberOfArtifacts = 2;
		int placedArtifacts = 0;
		List<Vector3Int> artifactPositions = new List<Vector3Int>();
		
		while (placedArtifacts < numberOfArtifacts)
		{
			int randomX;
			int randomY;
			Vector3Int position;
			bool canBePlaced = true;
			
			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area1MinBounds.x, area1MaxBounds.x + 1);
				randomY = Random.Range(area1MinBounds.y, area1MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while (position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y);
			
			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				// We pick a random artifact to place
				int randIndex = Random.Range(0, artifacts.Count);
				artifacts[randIndex].transform.position = position;
				
				if (artifactPositions.Contains(position) || plankPositions.Contains(position) || NonPlacementMap.GetTile(position) != null) 
				{
					canBePlaced = false;
				}
				
				// If we place the artifact, we add the position to current artifact positions
				if (canBePlaced) 
				{
					artifacts.RemoveAt(randIndex);
					artifactPositions.Add(position);
					placedArtifacts++;
				}
			}
		}
	}
	
	public void BuildArea2() 
	{
		Vector3Int area2MinBounds = new Vector3Int(27, 16, 0);
		Vector3Int area2MaxBounds = new Vector3Int(46, -2, 0);
		
		// Area where items cannot spawn
		Vector3Int deadZoneMinBounds = new Vector3Int(27, 4, 0);
		Vector3Int deadZoneMaxBounds = new Vector3Int(29, 0, 0);
		
		Vector3Int deadZone2MinBounds = new Vector3Int(35, 16, 0);
		Vector3Int deadZone2MaxBounds = new Vector3Int(37, 16, 0);
		
		// Rock tiles to place
		int numberOfRockTiles = 20;
		int placedRockTiles = 0;

		// Loop until number of tiles have been placed
		while (placedRockTiles < numberOfRockTiles)
		{
			int randomX;
			int randomY;
			Vector3Int position;

			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area2MinBounds.x, area2MaxBounds.x + 1);
				randomY = Random.Range(area2MinBounds.y, area2MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while ((position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y) ||
				(position.x >= deadZone2MinBounds.x && position.x <= deadZone2MaxBounds.x &&
				position.y <= deadZone2MinBounds.y && position.y >= deadZone2MaxBounds.y));

			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				ColliderMap.SetTile(position, RockTile);
				placedRockTiles++;
			}
		}
		
		// Sync the tilemap collider changes
		ColliderMap.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
		
		// Planks to place
		int numberOfPlanks = 4;
		int placedPlanks = 0;
		List<Vector3Int> plankPositions = new List<Vector3Int>();
		
		while (placedPlanks < numberOfPlanks)
		{
			int randomX;
			int randomY;
			Vector3Int position;
			List<Vector3Int> currentPlankPositions;
			bool canBePlaced = true;
			
			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area2MinBounds.x, area2MaxBounds.x + 1);
				randomY = Random.Range(area2MinBounds.y, area2MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while ((position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y) ||
				(position.x >= deadZone2MinBounds.x && position.x <= deadZone2MaxBounds.x &&
				position.y <= deadZone2MinBounds.y && position.y >= deadZone2MaxBounds.y));
			
			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				planksArea2[placedPlanks].transform.position = position;
				
				// Save the positions that the plank will be covering
				if (planksArea2[placedPlanks].transform.rotation.z == 0f) 
				{
					currentPlankPositions = new List<Vector3Int>()
					{
						new Vector3Int(position.x-1, position.y),
						position,
						new Vector3Int(position.x+1, position.y)
					};
				}
				else
				{
					currentPlankPositions = new List<Vector3Int>()
					{
						new Vector3Int(position.x, position.y-1),
						position,
						new Vector3Int(position.x, position.y+1)
					};
				}
				
				// If a collider or nonplacement tile exists at any plank position, we do not place the plank
				foreach (Vector3Int plankPos in currentPlankPositions) 
				{
					if (ColliderMap.GetColliderType(plankPos) != Tile.ColliderType.None || plankPositions.Contains(plankPos) || NonPlacementMap.GetTile(plankPos) != null) 
					{
						canBePlaced = false;
					}
				}
				
				// If we place the plank, we add the position to current plank positions
				if (canBePlaced) 
				{
					plankPositions.AddRange(currentPlankPositions);
					placedPlanks++;
				}
			}
		}
		
		// Artifacts to place
		int numberOfArtifacts = 4;
		int placedArtifacts = 0;
		List<Vector3Int> artifactPositions = new List<Vector3Int>();
		
		while (placedArtifacts < numberOfArtifacts)
		{
			int randomX;
			int randomY;
			Vector3Int position;
			bool canBePlaced = true;
			
			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area2MinBounds.x, area2MaxBounds.x + 1);
				randomY = Random.Range(area2MinBounds.y, area2MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while ((position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y) ||
				(position.x >= deadZone2MinBounds.x && position.x <= deadZone2MaxBounds.x &&
				position.y <= deadZone2MinBounds.y && position.y >= deadZone2MaxBounds.y));
			
			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				// We pick a random artifact to place
				int randIndex = Random.Range(0, artifacts.Count);
				artifacts[randIndex].transform.position = position;
				
				if (artifactPositions.Contains(position) || plankPositions.Contains(position) || NonPlacementMap.GetTile(position) != null) 
				{
					canBePlaced = false;
				}
				
				// If we place the artifact, we add the position to current artifact positions
				if (canBePlaced) 
				{
					artifacts.RemoveAt(randIndex);
					artifactPositions.Add(position);
					placedArtifacts++;
				}
			}
		}
	}
	
	public void BuildArea3() 
	{
		Vector3Int area3MinBounds = new Vector3Int(29, 41, 0);
		Vector3Int area3MaxBounds = new Vector3Int(43, 27, 0);
		
		// Area where items cannot spawn
		Vector3Int deadZoneMinBounds = new Vector3Int(34, 27, 0);
		Vector3Int deadZoneMaxBounds = new Vector3Int(38, 26, 0);
		
		Vector3Int deadZone2MinBounds = new Vector3Int(29, 36, 0);
		Vector3Int deadZone2MaxBounds = new Vector3Int(29, 34, 0);
		
		// Rock tiles to place
		int numberOfRockTiles = 16;
		int placedRockTiles = 0;

		// Loop until number of tiles have been placed
		while (placedRockTiles < numberOfRockTiles)
		{
			int randomX;
			int randomY;
			Vector3Int position;

			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area3MinBounds.x, area3MaxBounds.x + 1);
				randomY = Random.Range(area3MinBounds.y, area3MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while ((position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y) ||
				(position.x >= deadZone2MinBounds.x && position.x <= deadZone2MaxBounds.x &&
				position.y <= deadZone2MinBounds.y && position.y >= deadZone2MaxBounds.y));

			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				ColliderMap.SetTile(position, RockTile);
				placedRockTiles++;
			}
		}
		
		// Sync the tilemap collider changes
		ColliderMap.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();
		
		// Planks to place
		int numberOfPlanks = 3;
		int placedPlanks = 0;
		List<Vector3Int> plankPositions = new List<Vector3Int>();
		
		while (placedPlanks < numberOfPlanks)
		{
			int randomX;
			int randomY;
			Vector3Int position;
			List<Vector3Int> currentPlankPositions;
			bool canBePlaced = true;
			
			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area3MinBounds.x, area3MaxBounds.x + 1);
				randomY = Random.Range(area3MinBounds.y, area3MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while ((position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y) ||
				(position.x >= deadZone2MinBounds.x && position.x <= deadZone2MaxBounds.x &&
				position.y <= deadZone2MinBounds.y && position.y >= deadZone2MaxBounds.y));
			
			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				planksArea3[placedPlanks].transform.position = position;
				
				// Save the positions that the plank will be covering
				if (planksArea3[placedPlanks].transform.rotation.z == 0f) 
				{
					currentPlankPositions = new List<Vector3Int>()
					{
						new Vector3Int(position.x-1, position.y),
						position,
						new Vector3Int(position.x+1, position.y)
					};
				}
				else
				{
					currentPlankPositions = new List<Vector3Int>()
					{
						new Vector3Int(position.x, position.y-1),
						position,
						new Vector3Int(position.x, position.y+1)
					};
				}
				
				// If a collider or nonplacement tile exists at any plank position, we do not place the plank
				foreach (Vector3Int plankPos in currentPlankPositions) 
				{
					if (ColliderMap.GetColliderType(plankPos) != Tile.ColliderType.None || plankPositions.Contains(plankPos) || NonPlacementMap.GetTile(plankPos) != null) 
					{
						canBePlaced = false;
					}
				}
				
				// If we place the plank, we add the position to current plank positions
				if (canBePlaced) 
				{
					plankPositions.AddRange(currentPlankPositions);
					placedPlanks++;
				}
			}
		}
		
		// Artifacts to place
		int numberOfArtifacts = 2;
		int placedArtifacts = 0;
		List<Vector3Int> artifactPositions = new List<Vector3Int>();
		
		while (placedArtifacts < numberOfArtifacts)
		{
			int randomX;
			int randomY;
			Vector3Int position;
			bool canBePlaced = true;
			
			// Keep generating random positions until it's not in the dead zone
			do
			{
				randomX = Random.Range(area3MinBounds.x, area3MaxBounds.x + 1);
				randomY = Random.Range(area3MinBounds.y, area3MaxBounds.y + 1);
				position = new Vector3Int(randomX, randomY, 0);
			}
			while ((position.x >= deadZoneMinBounds.x && position.x <= deadZoneMaxBounds.x &&
				position.y <= deadZoneMinBounds.y && position.y >= deadZoneMaxBounds.y) ||
				(position.x >= deadZone2MinBounds.x && position.x <= deadZone2MaxBounds.x &&
				position.y <= deadZone2MinBounds.y && position.y >= deadZone2MaxBounds.y));
			
			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				// We pick a random artifact to place
				int randIndex = Random.Range(0, artifacts.Count);
				artifacts[randIndex].transform.position = position;
				
				if (artifactPositions.Contains(position) || plankPositions.Contains(position) || NonPlacementMap.GetTile(position) != null) 
				{
					canBePlaced = false;
				}
				
				// If we place the artifact, we add the position to current artifact positions
				if (canBePlaced) 
				{
					artifacts.RemoveAt(randIndex);
					artifactPositions.Add(position);
					placedArtifacts++;
				}
			}
		}
	}
	
	#endregion
}

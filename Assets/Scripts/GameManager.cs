using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	[HideInInspector] public PlayerInputActions input;
	public GameObject VictoryMenu;
	public GameObject GameOverMenu;
	public GameObject pauseMenu;
	public bool paused = false;
	public GameObject Yeti;
	public PlayerController Player;
	public PlayerController playerController;
	
	public Tilemap ColliderMap;
	public Tile RockTile;
	
	public AudioSource Main_Music;
	public AudioSource Wind_Sound;

	private Yeti yetiScript;

	void Start()
	{
		BuildArea1();
		
		
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
	
	void Update()
	{
  
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
		Player.gameObject.SetActive(false);
	}
	public void ResumeButtonClicked()
	{
		pauseMenu.SetActive(false);
		Yeti.SetActive(true);
		paused = false;
		Player.input.Enable();
		Timer.instance.StartTimer();
	}

	public void Victory() 
	{
		//use time multiplier for score
		ScoreManagerExpanded.instance.TimeMultiplier((int)Timer.instance.elapsedTime);

		//stop timer in background
		Timer.instance.StopTimer();

		VictoryMenu.SetActive(true);
		Yeti.SetActive(false);
		Player.gameObject.SetActive(false);

	}
	
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
	public void BuildArea1() 
	{
		Vector3Int area1MinBounds = new Vector3Int(3, 6, 0);
		Vector3Int area1MaxBounds = new Vector3Int(19, -6, 0);
		
		int numberOfRockTiles = 14;
		int placedRockTiles = 0;

		while (placedRockTiles < numberOfRockTiles)
		{
			int randomX = Random.Range(area1MinBounds.x, area1MaxBounds.x + 1);
			int randomY = Random.Range(area1MinBounds.y, area1MaxBounds.y + 1);

			Vector3Int position = new Vector3Int(randomX, randomY, 0);
			
			// Check if there's already a collider tile at this position
			if (ColliderMap.GetColliderType(position) == Tile.ColliderType.None)
			{
				ColliderMap.SetTile(position, RockTile);
				placedRockTiles++;
			}
		}
		
	}
}

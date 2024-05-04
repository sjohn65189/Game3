using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject VictoryMenu;
	public GameObject GameOverMenu;
	public GameObject Yeti;
	public GameObject Player;
	public PlayerController playerController;
	
	public AudioSource Main_Music;
	public AudioSource Wind_Sound;
	
	void Start()
	{
		// Check if music and sound effects are enabled. If no value is set, return 1
		int musicEnabled = PlayerPrefs.GetInt("musicEnabled", 1);
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
		playerController.YetiStart(); // This delays the yeti chase
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
		UnityEditor.EditorApplication.isPlaying = false;
		// endif 
		Application.Quit();
	}
	
	public void Gameover() 
	{
		ScoreManager.instance.NewHigh();
		GameOverMenu.SetActive(true);
		Player.SetActive(false);
	}
	
	public void Victory() 
	{
		ScoreManager.instance.AddTimeToScore((int)Timer.instance.elapsedTime);
		ScoreManager.instance.NewHigh();
		VictoryMenu.SetActive(true);
		Yeti.SetActive(false);
	}
}

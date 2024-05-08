using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
	public GameObject StartMenu;
	public GameObject OptionsMenu;
	public TMP_Text MusicButton;
	public TMP_Text SoundButton;

	public AudioSource Main_Music;
	public AudioSource Wind_Sound;
	
	// Start is called before the first frame update
	void Start()
	{
		if (PlayerPrefs.GetInt("MusicEnabled", 1) == 1) 
		{
			MusicButton.text = "On";
			Main_Music.Play();
		}
		else 
		{
			MusicButton.text = "Off";
			Main_Music.Stop();
		}
		if (PlayerPrefs.GetInt("SFXEnabled", 1) == 1) 
		{
			SoundButton.text = "On";
			Wind_Sound.Play();
		}
		else 
		{
			SoundButton.text = "Off";
			Wind_Sound.Stop();
		}
		
		OptionsMenu.SetActive(false);
	}

	// Run the game
	public void StartButtonClicked()
	{
		// Load game
		SceneManager.LoadScene("GameScene");
    }
	
	// Open the options menu
	public void OptionsButtonClicked()
	{
		StartMenu.SetActive(false);
		OptionsMenu.SetActive(true);
	}
	
	public void BackButtonClicked() 
	{
		StartMenu.SetActive(true);
		OptionsMenu.SetActive(false);
	}
	
	// Quit the game
	public void QuitButtonClicked()
	{
		// if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		// endif 
		Application.Quit();
	}
	
	// enable/disable the game music
	public void GameMusicButtonClicked()
	{
		// Save setting to player prefs under "MusicEnabled"
		if(MusicButton.text == "Off")
		{
			MusicButton.text = "On";
			PlayerPrefs.SetInt("MusicEnabled", 1);
			Main_Music.Play();
		} 
		else 
		{
			MusicButton.text = "Off";
			PlayerPrefs.SetInt("MusicEnabled", 0);
			Main_Music.Stop();
		}
	}
	
	// enable/disable the game sounds
	public void GameSoundsButtonClicked()
	{
		// Save setting to player prefs under "SFXEnabled"
		if(SoundButton.text == "Off")
		{
			PlayerPrefs.SetInt("SFXEnabled", 1);
			SoundButton.text = "On";
			Wind_Sound.Play();
		} 
		else 
		{
			PlayerPrefs.SetInt("SFXEnabled", 0);
			SoundButton.text = "Off";
			Wind_Sound.Stop();
		}
	}
}

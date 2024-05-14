using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;

public class Menus : MonoBehaviour
{

    [HideInInspector] public PlayerInputActions input;

    public GameObject MainMenu;
	public GameObject OptionsMenu;
	public GameObject VictoryMenu;
	public GameObject GameOverMenu;
	public GameObject PauseMenu;
	public GameObject Game;
	public GameObject Yeti;
	public GameObject Player;
	public PlayerController playerController;
	public GameObject BridgePieces;
	public GameObject Camera;

	public GameObject Health;
	public TMP_Text MusicButton;
	public TMP_Text SoundButton;

	public AudioSource Main_Music;
    public AudioSource wind;
	public AudioSource pickUp;
	public AudioSource yeti;
	public AudioSource footsteps;

	private bool gameMusicEnabled = false;
    private bool gameSoundsEnabled = false;
	private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
	{
		OptionsMenu.SetActive(false);
		VictoryMenu.SetActive(false);
		GameOverMenu.SetActive(false);
		PauseMenu.SetActive(false);
		Game.SetActive(false);
		Main_Music.Stop();
		wind.Stop();
        pickUp.Stop();
        yeti.Stop();
        footsteps.Stop();

        input = new PlayerInputActions();
        input.Enable();
    }

	// run the game
	public void StartButtonClicked(){
		MainMenu.SetActive(false);
		Game.SetActive(true);
		Player.SetActive(true);
		Yeti.SetActive(true);
		BridgePieces.SetActive(true);
        //Camera.transform.SetParent(Player.transform);
        if (gameMusicEnabled)
        {
            Main_Music.Play();
            wind.Play();
        }
        if (gameSoundsEnabled)
        {
            yeti.Play();
            pickUp.Play();
            footsteps.Play();
        }
		if (input.PlayerActions.TogglePause.IsPressed()) 
		{
			PauseMenuIsToggled();

        }
    }
	//Resume button
	public void ResumeButtonClicked() 
	{
        Time.timeScale = 1f; //pause the game
        PauseMenu.SetActive(false);
        Yeti.SetActive(true);
        Player.SetActive(true);
        Timer.instance.StartTimer();
    }

	// open the options menu
	public void OptionsButtonClicked(){
		MainMenu.SetActive(false);
		OptionsMenu.SetActive(true);
	}
	
	// quit the game
	public void QuitButtonClicked(){
		// if UNITY_EDITOR
		//UnityEditor.EditorApplication.isPlaying = false;
		// endif 
		Application.Quit();
	}
	
	// return to the Main menu
	public void MainMenuButtonClicked(){
		if(OptionsMenu.activeSelf){
			OptionsMenu.SetActive(false);
			MainMenu.SetActive(true);
		} else if (VictoryMenu.activeSelf) {
			VictoryMenu.SetActive(false);
			MainMenu.SetActive(true);			
		} else if (GameOverMenu.activeSelf) {
			GameOverMenu.SetActive(false);
			MainMenu.SetActive(true);
		}
	}
	
	// restart the game
	public void RestartButtonClicked(){
		if(GameOverMenu.activeSelf){
			GameOverMenu.SetActive(false);
		} else if (VictoryMenu.activeSelf) {
			VictoryMenu.SetActive(false);
		}
		StartButtonClicked();
	}
	
	// enable/disable the game music
	public void GameMusicButtonClicked(){
		if(MusicButton.text == "Off"){
			MusicButton.text = "On";
			gameMusicEnabled = true;
		} else {
			MusicButton.text = "Off";
			gameMusicEnabled = false;
		}
	}
	
	// enable/disable the game sounds
	public void GameSoundsButtonClicked(){
		if(SoundButton.text == "Off"){
			SoundButton.text = "On";
            gameSoundsEnabled = true;
        } else {
			SoundButton.text = "Off";
            gameSoundsEnabled = false;
        }
	}

	public void PauseMenuIsToggled() 
	{
		isPaused = !isPaused;

		if (isPaused)
		{
			Time.timeScale = 0f; //pause the game
			PauseMenu.SetActive(true);
			Yeti.SetActive(false);
			Player.SetActive(false);
			Timer.instance.StopTimer();
		}
	}

}

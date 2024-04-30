using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menus : MonoBehaviour
{
	public GameObject MainMenu;
	public GameObject OptionsMenu;
	public GameObject VictoryMenu;
	public GameObject GameOverMenu;
	public GameObject Game; 
	public GameObject Player;
	public TMP_Text MusicButton;
	public TMP_Text SoundButton;
	
    // Start is called before the first frame update
    void Start()
    {
        OptionsMenu.SetActive(false);
		VictoryMenu.SetActive(false);
		GameOverMenu.SetActive(false);
		Game.SetActive(false);
    }

	// run the game
    public void StartButtonClicked(){
		MainMenu.SetActive(false);
		Game.SetActive(true);
		Player.SetActive(true);
	}
	
	// open the options menu
	public void OptionsButtonClicked(){
		MainMenu.SetActive(false);
		OptionsMenu.SetActive(true);
	}
	
	// quit the game
	public void QuitButtonClicked(){
		// if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
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
		} else {
			MusicButton.text = "Off";
		}
	}
	
	// enable/disable the game sounds
	public void GameSoundsButtonClicked(){
		if(SoundButton.text == "Off"){
			SoundButton.text = "On";
		} else {
			SoundButton.text = "Off";
		}
	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
	public GameObject MainMenu;
	public GameObject OptionsMenu;
	public GameObject VictoryMenu;
	public GameObject GameOverMenu;
	
    // Start is called before the first frame update
    void Start()
    {
        OptionsMenu.SetActive(false);
		VictoryMenu.SetActive(false);
		GameOverMenu.SetActive(false);
    }

	// run the game
    public void StartButtonClicked(){
		
	}
	
	// open the options menu
	public void OptionsButtonClicked(){
		MainMenu.SetActive(false);
		OptionsMenu.SetActive(true);
	}
	
	// quit the game
	public void QuitButtonClicked(){
		
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
		
	}
	
	// enable/disable the game music
	public void GameMusicButtonClicked(){
		
	}
	
	// enable/disable the game sounds
	public void GameSoundsButtonClicked(){
		
	}
	
}

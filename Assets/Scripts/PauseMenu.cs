using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[HideInInspector] public PlayerInputActions input;
	public GameObject pauseMenu;
	public GameObject Yeti;
	public PlayerController Player;
	public GameManager gameManager;
	public GameObject GameOver;
	public GameObject WinScreen;

	private Yeti yetiScript;
	// Start is called before the first frame update
	void Start()
	{
		pauseMenu.SetActive(false);

		input = new PlayerInputActions();
		input.Enable();
		
		// Get the Yeti script component
		yetiScript = Yeti.GetComponent<Yeti>();
	}

	// Update is called once per frame
	void Update()
	{
		if (input.PlayerActions.TogglePause.IsPressed() && GameOver.activeSelf == false && WinScreen.activeSelf == false)
		{
			PauseMenuIsToggled();
		}
	}
	
	public void PauseMenuIsToggled()
	{
		gameManager.paused = true;
		pauseMenu.SetActive(true);
		Yeti.SetActive(false);
		Player.input.Disable();
		Timer.instance.StopTimer();
	}
}

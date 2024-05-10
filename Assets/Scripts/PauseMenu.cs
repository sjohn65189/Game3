using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;
    public GameObject pauseMenu;
    public GameObject Yeti;
    public GameObject Player;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

        input = new PlayerInputActions();
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.PlayerActions.TogglePause.IsPressed())
        {
            PauseMenuIsToggled();
        }
    }
    public void PauseMenuIsToggled()
    {
        isPaused = true;
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Yeti.SetActive(false);
            Player.SetActive(false);
            Timer.instance.StopTimer();
        }
    }
}

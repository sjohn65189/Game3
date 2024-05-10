using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;
    public GameObject pauseMenu;
    public GameObject Yeti;
    public PlayerController Player;
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
        pauseMenu.SetActive(true);
        Yeti.SetActive(false);
        Player.input.Disable();
        Timer.instance.StopTimer();
    }
}

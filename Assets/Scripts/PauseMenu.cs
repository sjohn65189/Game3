using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;
    public GameObject pauseMenu;
    public GameObject Yeti;
    public PlayerController Player;

    private Yeti yetiScript;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

        input = new PlayerInputActions();
        input.Enable();

        // Find the GameObject with the Yeti script attached
        GameObject yetiGameObject = GameObject.Find("Yeti");
        if (yetiGameObject != null)
        {
            // Get the Yeti script component
            yetiScript = yetiGameObject.GetComponent<Yeti>();
        }
        else
        {
            Debug.LogError("Yeti GameObject not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Yeti.activeSelf)
        {
            Debug.Log("yeti set active");
        }
        if (input.PlayerActions.TogglePause.IsPressed())
        {
            PauseMenuIsToggled();
        }
    }
    public void PauseMenuIsToggled()
    {
        pauseMenu.SetActive(true);
        // Pause Yeti coroutine if it exists
        if (yetiScript != null)
        {
            yetiScript.StopDelayChaseCoroutine();
        }
        Yeti.SetActive(false);
        Player.input.Disable();
        Timer.instance.StopTimer();
    }
}

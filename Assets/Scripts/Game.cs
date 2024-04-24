using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public PlayerHealth health;

    void Start()
    {
        
    }

    void Update()
    {
        if (health != null)
        {
            if (health.Health <= 0f)
            {
                SceneManager.LoadScene("GameOver");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    // We will call this function when the user presses escape
    void Quit() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

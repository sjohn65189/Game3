using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public GameObject Menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicButtonClicked() 
    {
        Menus menu = Menu.GetComponent<Menus>();

        if (menu.gameMusicEnabled == false)
        {
            menu.gameMusicEnabled = true;
            menu.Main_Music.Play();

            if (menu.MusicButton.text == "Off")
            {
                menu.MusicButton.text = "On";
            }
        }
        else if (menu.gameMusicEnabled == true)
        {
            menu.gameMusicEnabled = false;
            menu.Main_Music.Stop();
            if (menu.MusicButton.text == "On")
            {
                menu.MusicButton.text = "Off";
            }
        }
    }

    public void GameSoundButtonClicked() 
    {
        Menus menu = Menu.GetComponent<Menus>();

    }

}

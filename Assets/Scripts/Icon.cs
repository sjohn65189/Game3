using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Icon : MonoBehaviour
{
    GameManager gameM;
    public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameM = GameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicButtonClicked() 
    {
        if (PlayerPrefs.GetInt("MusicEnabled", 0) == 0)
        {
            PlayerPrefs.SetInt("MusicEnabled", 1);
            gameM.Main_Music.Play();
        }
        else {
            gameM.Main_Music.mute = !gameM.Main_Music.mute;
        } 
    }

    public void GameSoundButtonClicked() 
    {
        if (PlayerPrefs.GetInt("SFXEnabled", 0) == 0)
        {
            PlayerPrefs.SetInt("SFXEnabled", 1);
            gameM.Wind_Sound.Play();
        }
        else {
            gameM.Wind_Sound.mute = !gameM.Wind_Sound.mute;
        } 
    }

}

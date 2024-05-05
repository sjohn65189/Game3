using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public GameObject GameManager;
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
        GameManager gameM = GameManager.GetComponent<GameManager>();
        gameM.Main_Music.mute = !gameM.Main_Music.mute;
    }

    public void GameSoundButtonClicked() 
    {
        GameManager gameM = GameManager.GetComponent<GameManager>();
        gameM.Wind_Sound.mute = !gameM.Wind_Sound.mute;
    }

}

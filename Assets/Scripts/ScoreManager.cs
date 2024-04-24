using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreTxt;
    public Text highscoreTxt;

    int score = 1000;
    int highscore = 0;

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreTxt.text = score.ToString() + " Points";
        highscoreTxt.text = "Highscore: " + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //for storing new highscores
    //PlayerPrefs.SetInt("highscore", score);
}

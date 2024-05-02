using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HighScore;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] public TextMeshProUGUI scoreTxt;
    [SerializeField] public TextMeshProUGUI highscoreTxt;

    int score = 0;
    int highscore = 0;

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //see if new highscore
        if(score > highscore)
        {
            highscore = PlayerPrefs.GetInt("highscore", score);
            //HS.SubmitHighScore(this, "Player", highscore);
        }

        //write texts to be read and displayed
        this.highscoreTxt.text = "Highscore: " + highscore.ToString("D4");
        this.scoreTxt.text = "Score: " + score.ToString("D4");
    }

    //for storing new highscores
    //PlayerPrefs.SetInt("highscore", score);

    // for getting points in level later on
    void GetRelic()
    {
        this.score += 1;
    }
}

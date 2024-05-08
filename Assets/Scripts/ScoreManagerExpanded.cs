using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HighScore;

public class ScoreManagerExpanded : MonoBehaviour
{
    public static ScoreManagerExpanded instance;

    //hud
    [SerializeField] public TextMeshProUGUI HUDScoreTxt;
    [SerializeField] public TextMeshProUGUI HUDHighscoreTxt;

    //victory
    [SerializeField] public TextMeshProUGUI VScoreTxt;
    [SerializeField] public TextMeshProUGUI VHighscoreTxt;
    [SerializeField] public Button submitHSBtn;
    [SerializeField] public TextMeshProUGUI playerNameTxt;

    //game over
    [SerializeField] public TextMeshProUGUI GOScoreTxt;
    [SerializeField] public TextMeshProUGUI GOHighscoreTxt; 

    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 8000);
        HS.Init(this, "Ice Monk");
    }

    // Update is called once per frame
    void Update()
    {
        //see if new highscore
        if (score > highscore)
        {
            highscore = PlayerPrefs.GetInt("highscore", score);
        }

        //update hud scores
        this.HUDHighscoreTxt.text = "Highscore:\n" + highscore.ToString("D4");
        this.HUDScoreTxt.text = "Score:\n" + score.ToString("D4");
        //update victory menu scores
        this.VHighscoreTxt.text = "Highscore:\n" + highscore.ToString("D4");
        this.VScoreTxt.text = "Score:\n" + score.ToString("D4");
        //update game over menu scores
        this.GOHighscoreTxt.text = "Highscore:\n" + highscore.ToString("D4");
        this.GOScoreTxt.text = "Score:\n" + score.ToString("D4");

    }

    //for storing new highscores
    //PlayerPrefs.SetInt("highscore", score);

    // for getting points in level later on
    void GetRelic()
    {
        this.score += 40;
    }

    public void AddTimeToScore(int deltaTime)
    {
        this.score += 9999 - deltaTime;
    }

    //check if new highscore
    public void NewHigh()
    {
        submitHSBtn.gameObject.SetActive(false);
        HS.SubmitHighScore(this, playerNameTxt.text, this.score);
    }
}
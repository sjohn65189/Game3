using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HighScore;

public class ScoreManagerExpanded : MonoBehaviour
{
    public static ScoreManagerExpanded instance;

    public int RELICPOINTVALUE = 40;

    //hud
    [SerializeField] public TextMeshProUGUI HUDScoreTxt;
    [SerializeField] public TextMeshProUGUI HUDHighscoreTxt;

    //victory
    [SerializeField] public GameObject VictoryPanel;
    [SerializeField] public TextMeshProUGUI VScoreTxt;
    [SerializeField] public TextMeshProUGUI VHighscoreTxt;
    [SerializeField] public Button submitHSBtn;
    [SerializeField] public TextMeshProUGUI playerNameTxt;

    [SerializeField] public TextMeshProUGUI ArtifactsRetrievedTxt;
    [SerializeField] public TextMeshProUGUI TimeBonusTxt;
    [SerializeField] public TextMeshProUGUI TotalScoreTxt;

    //game over
    [SerializeField] public GameObject GameOverPanel;
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
        //this gets the highscore from player prefs but sets it to 8000 if no score is found
        highscore = PlayerPrefs.GetInt("highscore", 0);
        HS.Init(this, "Yeti's Pass");
    }

    // Update is called once per frame
    void Update()
    {
        //see if new highscore
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            highscore = PlayerPrefs.GetInt("highscore", score);
        }

        UpdateCurrentUI();

        //for testing 
        //score++;
    }

    //see if menu is active thus needing to be updated
    public void UpdateCurrentUI()
    {
        bool firstTimeAtMenu = true;

        //update hud scores ; hud is always active
        this.HUDHighscoreTxt.text = "Highscore:\n" + highscore.ToString("D4");
        this.HUDScoreTxt.text = "Score:\n" + score.ToString("D4");

        //if game over is active
        if (GameOverPanel.activeSelf && firstTimeAtMenu)
        {
            firstTimeAtMenu = false;
            //update game over menu scores
            this.GOHighscoreTxt.text = "Highscore:\n" + highscore.ToString("D4");
            this.GOScoreTxt.text = "Score:\n" + score.ToString("D4");
        }

        //if victory is active
        if (VictoryPanel.activeSelf && firstTimeAtMenu)
        {
            firstTimeAtMenu = false;
            //update victory menu scores
            this.VHighscoreTxt.text = "Highscore:\n" + highscore.ToString("D4");
            this.VScoreTxt.text = "Score:\n" + score.ToString("D4");

            //update victory menu score breakdown
            this.ArtifactsRetrievedTxt.text = "Artifacts Retrieved: " + GetArtifactsGrabbed().ToString("D4");
            this.TimeBonusTxt.text = "Time Bonus: " + GetMultiplier((int)Timer.instance.elapsedTime).ToString();
            this.TotalScoreTxt.text = "Total Score: " + score.ToString("D4");
        }
    }


    // for getting points in level
    public void GetRelic()
    {
        this.score += RELICPOINTVALUE;
    }

    //time multiplier for score
    public void TimeMultiplier(int time)
    {
        float multiplier = GetMultiplier(time);

        this.score = (int)(score * multiplier);
    }

    //add health to score
    public void AddHealthToScore(int health)
    {
        this.score += health;
    }

    //check if new highscore
    public void NewHigh()
    {
        submitHSBtn.gameObject.SetActive(false);
        HS.SubmitHighScore(this, playerNameTxt.text, this.score);
    }

    public int GetArtifactsGrabbed()
    {
        int artifactsAmt = 0;

        return artifactsAmt;
    }

    public float GetMultiplier(int time)
    {
        float multiplier = 1;
        if (time >= 250)
        {
            multiplier = 1;
        }

        if (time < 250)
        {
            multiplier = Mathf.Lerp(1, 3, (1 - (time / 250)));
        }

        return multiplier;
    }
}
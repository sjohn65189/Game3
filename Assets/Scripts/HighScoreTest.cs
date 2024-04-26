using System.Collections;
using System.Collections.Generic;
using HighScore;
using UnityEngine;

public class HighScoreTest : MonoBehaviour
{
    private float delay;

    // Start is called before the first frame update
    void Start()
    {
        HS.Init(this, "Ice Monk");
        delay = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0) {
            delay -= Time.deltaTime;
            if (delay < 0) {
                // how to submit a new possible score (php file checks if it is a real high score)
                //HS.SubmitHighScore(this, "Troy Himself", Random.Range(554, 555));
            }
        }
    }
}

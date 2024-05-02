using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    bool timerIsOn = false;

    // Update is called once per frame
    void Update()
    {
        if (timerIsOn)
        {
            elapsedTime += Time.deltaTime;
        }

        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = "Time: " + seconds.ToString("D4");
    }

    void StartTimer()
    {
        elapsedTime = 0;
        timerIsOn = true;
    }
}

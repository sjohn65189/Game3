using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    [SerializeField] TextMeshProUGUI timerText;
    public float elapsedTime;
    bool timerIsOn = false;

    private void Awake() 
    {
        instance = this;
        timerIsOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsOn)
        {
            elapsedTime += Time.deltaTime;
        }

        // force time to be individual seconds
        int seconds = (int)elapsedTime;
        timerText.text = "Time: " + seconds.ToString("D4");
    }

    public void StartTimer()
    {
        timerIsOn = true;
    }

    public void StopTimer()
    {
        timerIsOn = false;
    }
}

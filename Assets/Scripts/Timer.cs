using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Timers;

public class Timer : MonoBehaviour
{
    public static System.Timers.Timer timer;
    void Start() 
    {
        StartTimer();

    }

    void StartTimer()
    {
        // Initialize timer
        timer = new System.Timers.Timer();
        timer.Interval = 1000; // 1 second interval
        
    }

    void ResetTimer(int clock) 
    {
        clock = 0;
    }
}

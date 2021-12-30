using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{ 
    public static TimeController instance;

    public Text timeController;

    private TimeSpan timeplaying;
    private bool timeGoing;

    private float elapsedTime;

    private void Awake()
    {
       instance = this;
    }

    private void Start()
    {
        timeController.text = "00:00";
        timeGoing = false;
        //BeginTimer();
    }

    public void BeginTimer()
    {
        timeGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }
    
    public void EndTimer()
    {
        timeGoing =false;
    }
    private IEnumerator UpdateTimer()
    {
        while (timeGoing)
        {
            elapsedTime += Time.deltaTime;
            timeplaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = timeplaying.ToString("mm':'ss");
            timeController.text = timePlayingStr;
      
            yield return null;
        }
    }
}

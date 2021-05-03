using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerB : MonoBehaviour
{
    public float timeToWait = 1.0f;
    private float previousEvent;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousEvent < (Time.time - timeToWait))
        {
            Debug.Log("The timer has ended.");
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        previousEvent = Time.time;
    }
}

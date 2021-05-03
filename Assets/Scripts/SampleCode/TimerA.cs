using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerA : MonoBehaviour
{
    public float timeToWait = 1.0f;
    private float nextEvent;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        nextEvent = Time.time + timeToWait;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextEvent)
        {
            Debug.Log("The timer has run out.");
            ResetTimer();
        }
    }
}

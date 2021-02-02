using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerC : MonoBehaviour
{
    public float timeToWait = 1f;
    private float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Debug.Log("The timer has finished.");
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timeRemaining = timeToWait;
    }
}

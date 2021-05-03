using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
[RequireComponent(typeof(TankShooter))]
public class SampleAIController1 : MonoBehaviour
{
    private TankData data;
    private TankMotor motor;
    private TankShooter shooter;

    public GameObject[] waypoints;
    private int currentWaypoint = 1;

    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType = LoopType.Stop;

    public float closeEnough = 1f;
    public bool reachedEnd = false;

    private bool isLoopingFoward = true;
    void Start()
    {
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
        shooter = GetComponent<TankShooter>();
    }

    void Update()
    {
        if (!reachedEnd)
        {
            // If not facing waypoint, turn to face it
            if (motor.RotateTowards(waypoints[currentWaypoint].transform.position, data.turnSpeed))
            {
                // Do nothing
            }
            // If facing waypoint, move towards it
            else
            {
                motor.Move(data.moveSpeed);
            }
        }

        if (loopType == LoopType.Stop)
        {
            if (Vector3.SqrMagnitude(transform.position - waypoints[currentWaypoint].transform.position) <= (closeEnough * closeEnough))
            {
                // If we are at the waypoint, go to the next one
                if (currentWaypoint < (waypoints.Length - 1))
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEnd = true;
                }
            }
        }

        else if (loopType == LoopType.Loop)
        {

            if (Vector3.SqrMagnitude(transform.position - waypoints[currentWaypoint].transform.position) <= (closeEnough * closeEnough))
            {
                // If we are at the waypoint, go to the next one
                if (currentWaypoint < (waypoints.Length - 1))
                {
                    currentWaypoint++;
                }
                // If at the last waypoint, go back to the beginning
                else
                {
                    currentWaypoint = 0;
                }
            }
        }

        else if (loopType == LoopType.PingPong)
        {
            if (isLoopingFoward)
            {
                if (Vector3.SqrMagnitude(transform.position - waypoints[currentWaypoint].transform.position) <= (closeEnough * closeEnough))
                {
                    // If we are at the waypoint, go to the next one
                    if (currentWaypoint < (waypoints.Length - 1))
                    {
                        currentWaypoint++;
                    }
                    // If at the last waypoint, go back to the beginning
                    else
                    {
                        isLoopingFoward = false;
                    }
                }
                else
                {
                    if (Vector3.SqrMagnitude(transform.position - waypoints[currentWaypoint].transform.position) <= (closeEnough * closeEnough))
                    {
                        // If we are at the waypoint, go to the next one
                        if (currentWaypoint > 0)
                        {
                            currentWaypoint--;
                        }
                        // If at the last waypoint, go back to the beginning
                        else
                        {
                            isLoopingFoward = true;
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("[SampleAIController1] unexpected LoopType");
            }

        }
    }
}

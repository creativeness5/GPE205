using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
[RequireComponent(typeof(TankShooter))]
public class SampleAIController3 : MonoBehaviour
{
    private TankData data;
    private TankMotor motor;
    private TankShooter shooter;

    public enum AttackState { Chase };
    public AttackState attackState = AttackState.Chase;

    public enum AvoidanceStage { NotAvoiding, ObstacleDetected, AvoidingObstacle };
    public AvoidanceStage avoidanceStage = AvoidanceStage.NotAvoiding;

    public float avoidanceTime = 2f;
    private float exitTime;
    public float closeEnough = 4f;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
        shooter = GetComponent<TankShooter>();
    }

    private void Update()
    {
        if (attackState == AttackState.Chase)
        {
            if (avoidanceStage != AvoidanceStage.NotAvoiding)
            {
                Avoid();
            }

            else
            {
                Chase(GameManager.Instance.Players[0]);
            }
        }
    }

    public void Chase(GameObject target)
    {
        if (motor.RotateTowards(target.transform.position, data.turnSpeed))
        {
            // Do nothing
        }

        else if (!CanMove(data.moveSpeed))
        {
            avoidanceStage = AvoidanceStage.ObstacleDetected;
        }

        else
        {
            if (Vector3.SqrMagnitude(transform.position - target.transform.position) >= (closeEnough * closeEnough))
            {
                motor.Move(data.moveSpeed);
            }
        }
    }

    public void Avoid()
    {
        if (avoidanceStage == AvoidanceStage.ObstacleDetected)
        {
            // Rotate left
            motor.Rotate(-1 * data.turnSpeed);

            // If can move forward, go to stage 2
            if (CanMove(data.moveSpeed))
            {
                avoidanceStage = AvoidanceStage.AvoidingObstacle;

                // Set number of seconds in timer before next stage
                exitTime = avoidanceTime;
            }
        }

        else if (avoidanceStage == AvoidanceStage.AvoidingObstacle)
        {
            // If can move forward, do so
            if (CanMove(data.moveSpeed))
            {
                // Subtract from our timer and mover
                exitTime -= Time.deltaTime;
                motor.Move(data.moveSpeed);

                // If we have moved long enough, return to chase mode
                if (exitTime <= 0)
                {
                    avoidanceStage = AvoidanceStage.NotAvoiding;
                }
            }

            else
            {
                avoidanceStage = AvoidanceStage.ObstacleDetected;
            }
        }
        
        // Turn to go around the obstacle
        // Move forward for a period of time
        // Attempt to go towards our target again
    }

    bool CanMove(float speed)
    {
        // Cast a ray froward in the distance that we sent in
        RaycastHit hit;

        // If our raycast hit something
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed))
        {
            // And if what we hit is not the player
            if(!hit.collider.CompareTag("Player"))
            {
                // Then can't move
                return false;
            }
        }

        // TODO: Check is we can move forward by "speed" units
        return true;
    }
}

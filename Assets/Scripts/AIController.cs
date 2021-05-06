using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(Health))]
public class AIController : MonoBehaviour
{
    private TankData data;
    private TankMotor motor;
    private TankShooter shooter;
    private Health health;

    public enum EnemyPersonality { Guard, Cowardly };
    public EnemyPersonality personality = EnemyPersonality.Guard;

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;

    public enum AvoidanceStage { NotAvoiding, ObstacleDetected, AvoidingObstacle };
    public AvoidanceStage avoidanceStage = AvoidanceStage.NotAvoiding;

    public float stateExitTime;
    public float fleeDistance = 1f;
    public float closeEnough = 4f;
    public float avoidanceTime = 2f;
    private float exitTime;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
        shooter = GetComponent<TankShooter>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (personality)
        {
            case EnemyPersonality.Guard:
                GuardFSM();
                break;
            case EnemyPersonality.Cowardly:
                CowardlyFSM();
                break;
            default:
                Debug.LogWarning("[SampleFSM] Unimplemented finite state machine");
                break;
        }

        if (avoidanceStage != AvoidanceStage.NotAvoiding)
        {
            Avoid();
        }

        else
        {
            GuardFSM();
        }
    }

    public void GuardFSM()
    {
        switch (aiState)
        {
            case AIState.Chase:
                // Do behaviors
                Chase(GameManager.Instance.Players[0]);
                // Check for transitions
                if (health.currentHealth < health.maxHealth * 0.5)
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (PlayerIsInRange())
                {
                    ChangeState(AIState.ChaseAndFire);
                }
                break;
            case AIState.ChaseAndFire:
                // Do something
                ChaseAndFire(GameManager.Instance.Players[0]);
                // Check for transitions
                if (health.currentHealth < health.maxHealth * 0.5)
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (!PlayerIsInRange())
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.CheckForFlee:
                // Do something
                CheckForFlee();
                // Check for transitions
                if (PlayerIsInRange())
                {
                    ChangeState(AIState.Flee);
                }
                else if (!PlayerIsInRange())
                {
                    ChangeState(AIState.Rest);
                }
                break;
            case AIState.Flee:
                // Do something
                Flee(GameManager.Instance.Players[0]);
                // Check for transitions
                if (stateExitTime > 5)
                {
                    ChangeState(AIState.CheckForFlee);
                }
                break;
            case AIState.Rest:
                // Do something
                Rest();
                // Check for transitions
                if (PlayerIsInRange())
                {
                    ChangeState(AIState.Flee);
                }
                else if (health.currentHealth == health.maxHealth)
                {
                    ChangeState(AIState.Chase);
                }
                break;
            default:
                Debug.LogWarning("[SampleFSM] State doesn't exist");
                break;
        }
    }

    private void Rest()
    {
        // TODO: write this method
    }

    private void Flee(GameObject target)
    {
        // TODO: write this method

        // Get the vector to our target
        Vector3 vectorToTarget = target.transform.position - transform.position;

        // Get the vector away from our target.
        Vector3 vectorAwayFromTarget = -1 * vectorToTarget;

        // Normalize the vector away from the target
        vectorAwayFromTarget.Normalize();

        // Adjust for flee distance
        vectorAwayFromTarget *= fleeDistance;

        // Set our flee position
        Vector3 fleePosition = vectorAwayFromTarget + transform.position;

        // This way to handle fleeing might make better sense than what is uncommented below
        //motor.RotateTowards(fleePosition, data.turnSpeed);
        //motor.Move(data.moveSpeed);

        if (motor.RotateTowards(fleePosition, data.turnSpeed))
        {
            // Do nothing
        }
        else
        {
            motor.Move(data.moveSpeed);
        }
    }

    private void CheckForFlee()
    {
        // TODO: write this method
    }

    private void ChaseAndFire(GameObject target)
    {
        // TODO: write this method

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
            shooter.Shoot();
        }
    }

    private bool PlayerIsInRange()
    {
        return true;
    }

    void Chase(GameObject target)
    {
        // TODO: write this method

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
            //shooter.Shoot();
        }
    }

    public void CowardlyFSM()
    {

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
    }

    bool CanMove(float speed)
    {
        // Cast a ray froward in the distance that we sent in
        RaycastHit hit;

        // If our raycast hit something
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed))
        {
            // And if what we hit is not the player
            if (!hit.collider.CompareTag("Player"))
            {
                // Then can't move
                return false;
            }
        }

        // TODO: Check is we can move forward by "speed" units
        return true;
    }

    void ChangeState(AIState newState)
    {
        aiState = newState;

        stateExitTime = Time.time;
    }


}

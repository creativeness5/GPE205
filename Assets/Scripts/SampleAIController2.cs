using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
[RequireComponent(typeof(TankShooter))]
public class SampleAIController2 : MonoBehaviour
{
    private TankData data;
    private TankMotor motor;
    private TankShooter shooter;

    public float fleeDistance = 1f;
    public float closeEnough = 4f;

    public enum AttackState { Chase, Flee };
    public AttackState attackState = AttackState.Chase;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
        shooter = GetComponent<TankShooter>();
    }

    // Update is called once per frame
    void Update()
    {
        //Chase(GameManager.Instance.Players[0]);
        Flee(GameManager.Instance.Players[0]);
    }

    public void Chase(GameObject target)
    {
        if (motor.RotateTowards(target.transform.position, data.turnSpeed))
        {
            // Do nothing
        }
        else
        {
            if (Vector3.SqrMagnitude(transform.position - target.transform.position) >= (closeEnough * closeEnough))
            {
                motor.Move(data.moveSpeed);
            }
        }
    }

    public void Flee(GameObject target)
    {
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
        motor.RotateTowards(fleePosition, data.turnSpeed);
        motor.Move(data.moveSpeed);
        
        if (motor.RotateTowards(vectorAwayFromTarget, data.turnSpeed))
        {
            // Do nothing
        }
        else
        {

        }

    }
}

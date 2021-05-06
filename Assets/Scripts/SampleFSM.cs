using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleFSM : MonoBehaviour
{
    public enum EnemyPersonality { Guard, Cowardly };
    public EnemyPersonality personality = EnemyPersonality.Guard;

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;
    private float health;
    private float maxHealth;

    public float stateExitTime;

    // Start is called before the first frame update
    void Start()
    {

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
    }

    public void GuardFSM()
    {
        switch (aiState)
        {
            case AIState.Chase:
                // Do behaviors
                Chase();
                // Check for transitions
                if (health < maxHealth * 0.5)
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
                ChaseAndFire();
                // Check for transitions
                if (health < maxHealth * 0.5)
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if(!PlayerIsInRange())
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
                Flee();
                // Check for transitions
                if(stateExitTime > 30)
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
                else if (health == maxHealth)
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

    private void Flee()
    {
        // TODO: write this method
    }

    private void CheckForFlee()
    {
        // TODO: write this method
    }

    private void ChaseAndFire()
    {
        // TODO: write this method
    }

    private bool PlayerIsInRange()
    {
        return true;
    }

    void Chase()
    {
        // TODO: write this method
    }

    public void CowardlyFSM()
    {

    }

    void ChangeState(AIState newState)
    {
        aiState = newState;

        stateExitTime = Time.time;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public GameObject attacker;
    public int attackDamage;

    public float timeToWait = 2f;
    private float timeRemaining = 2;

    private void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Debug.Log("Time's up");
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);


        Attack attackData = new Attack(attacker, attackDamage);

        //collision.gameObject.SendMessage("TakeDamage", attackData);

        // Destroy cannonball when it runs into another object
        Destroy(this.gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class TankShooter : MonoBehaviour
{
    public GameObject firePoint;  // Where cannon balls instantiate 
    public GameObject cannonBallPrefab;
    public int ballSpeed = 5;

    public float timeToWait = 2f;
    private float timeRemaining;
    private bool canShoot = true;

    private TankData data;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        rb = GetComponent<Rigidbody>();
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Debug.Log("Time to shoot again");
            canShoot = true;
        }
    }

    void ShootTimer()
    {
        
    }

    void ResetTimer()
    {
        timeRemaining = timeToWait;
        
    }

    public void Shoot()
    {
        // Check cooldown timer to see if we can shoot

        if (canShoot)
        {

            // Instantiate cannon ball - fix
            GameObject firedCannonBall = Instantiate(cannonBallPrefab, firePoint.transform.position, firePoint.transform.rotation);


            // Shoot the cannon ball forward with Rigidbody.AddForce()
            firedCannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * ballSpeed);

            // Cannon ball data needed: who, and how much damage will it cause
            CannonBall cannonBall = firedCannonBall.GetComponent<CannonBall>();
            cannonBall.attacker = this.gameObject;
            cannonBall.attackDamage = data.cannonBallDamage;

            Debug.Log("Instantiate success");

            //Can no longer shoot, have to wait for timer to expire to shoot again
            canShoot = false;
            ResetTimer();
        }
    }

}

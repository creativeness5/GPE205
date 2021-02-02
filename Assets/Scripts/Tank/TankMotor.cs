using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(CharacterController))]
// If attached to an object with neither of those components, it will automatically add them to that object
public class TankMotor : MonoBehaviour
{
    private TankData data;
    private CharacterController characterController;

    public void Start()
    {
        data = GetComponent<TankData>();
        characterController = GetComponent<CharacterController>();
    }

    public void Move(float moveSpeed)
    {
        Vector3 speedVector;
        // Vector to hold speed data

        speedVector = transform.forward * moveSpeed;
        // Start with vector pointing the same direction as the GameObject
        // Instead of 1 unit of length, applies our speed value

        characterController.SimpleMove(speedVector);
        // Calls SimpleMove() and sends it to the vector (applies Time.deltaTime)
    }

    public void Rotate(float rotateSpeed)
    {
        Vector3 rotateVector = Vector3.up * rotateSpeed * Time.deltaTime;
        // Vector to hold rotation data
        // Rotates right by one degree per frame draw (left = negative right)
        // Rotation based on our speed
        // Changes rotation to per second

        transform.Rotate(rotateVector, Space.Self);
        // Rotate by that value and in local space
    }
}

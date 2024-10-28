using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDrop : MonoBehaviour
{
    private Vector3 startPosition;
    public float dropForce = 5f;  // Initial force that throws the item upward
    public float gravity = 9.8f;  // Gravity value that simulates falling
    private float verticalSpeed;
    private bool isFalling = false;

    void Start ()
    {
        // Starting position on the Y axis representing the "floor"
        startPosition = transform.position;
        verticalSpeed = dropForce; // Applies force for the item to rise
        isFalling = true; // Starts falling state
    }

    void Update ()
    {
        if (isFalling)
        {
            // Updates position on the Y axis to simulate rising and then falling
            transform.position += new Vector3(0, verticalSpeed * Time.deltaTime, 0);
            
            // Reduce speed to simulate gravity
            verticalSpeed -= gravity * Time.deltaTime;

            // Check if the item has reached the floor 
            if (transform.position.y <= startPosition.y)
            {
                transform.position = startPosition; // Resets the position to the "floor"
                isFalling = false; // Stop falling
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class playermovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastDirection; // Track the last direction the player moved
    void Update()
    {
        //Check if the game isPaused here and the FixedUpdate() below
        if (Time.timeScale != 0) 
        {
            ProcessInputs();
        }
    }

    void FixedUpdate() 
    {
        if (Time.timeScale != 0) 
        {
            Move();
        }
        else
        {
            rb.velocity = Vector2.zero; //Prevents player from moving
        }
    }

  void ProcessInputs()
{
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    moveDirection = new Vector2(moveX, moveY).normalized;

    if (moveDirection != Vector2.zero)
    {
        lastDirection = moveDirection;
        // Update animator with the current movement
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.y);
        
        // Update the last movement direction for idle animations
        animator.SetFloat("LastX", moveDirection.x);
        animator.SetFloat("LastY", moveDirection.y);
    }
    else
    {
        // If idle, update the animator with the last direction the player was moving
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);

        // Keep last movement direction for idle animations
        animator.SetFloat("LastX", lastDirection.x);
        animator.SetFloat("LastY", lastDirection.y);
    }

    animator.SetFloat("Speed", moveDirection.sqrMagnitude);
}

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }
}

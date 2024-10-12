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
    private Vector2 lastDirection;

    void Update()
    {
        if (Time.timeScale != 0) 
        {
            ProcessInputs();
            UpdateLastDirectionByCursor();
            
            // Trigger shooting animation on mouse click
            if (Input.GetMouseButtonDown(0))
            {
                PlayShootingAnimation();
            }
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
            rb.velocity = Vector2.zero; // Prevent player from moving
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
            animator.SetFloat("LastX", moveDirection.x);
            animator.SetFloat("LastY", moveDirection.y);
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("LastX", lastDirection.x);
            animator.SetFloat("LastY", lastDirection.y);
            // This is probably s*** code that I'm too tired to fix now
        }

        animator.SetFloat("Speed", moveDirection.sqrMagnitude);
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void UpdateLastDirectionByCursor()
    {
        // Gets the cursor position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursorDirection = (mousePosition - transform.position).normalized;

        // Update lastDirection based on the cursor's direction
        if (cursorDirection != Vector2.zero)
        {
            lastDirection = cursorDirection;
        }
    }

    void PlayShootingAnimation()
    {
        // Set the animator's shooting trigger to start the shooting animation
        animator.SetTrigger("Shoot");

        // Update the animator with the shooting direction 
        // TO DO: REDO THIS PART SO IT PLAYS THE ANIMATION FOR THE MOUSE DIRECTION, NOT MOVEMENT DIRECTION
        animator.SetFloat("LastX", lastDirection.x);
        animator.SetFloat("LastY", lastDirection.y);
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

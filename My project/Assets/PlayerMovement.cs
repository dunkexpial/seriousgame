using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playermovement : MonoBehaviour
{

    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 moveDirection;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate() 
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.y);
        animator.SetFloat("Speed", moveDirection.sqrMagnitude);
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void  Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}

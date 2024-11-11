using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    // Property that provides access to the DialogueUI component.
    // This property is read-only, allowing other classes to read the value of the private variable dialogueUI,
    // but they cannot change it directly. This helps maintain encapsulation.
    public DialogueUI DialogueUI => dialogueUI;

    // Property that allows access to and modification of an object that implements the IInteractable interface.
    // This property has both a get method for reading and a set method for writing, 
    // enabling other parts of the code to both obtain the current value and set a new value for Interactable.
    // This is useful for storing a reference to any object that can be interacted with, such as NPCs or objects in the game.
    public Interactable Interactable { get; set; }

    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 moveDirection;
    private Vector2 lastDirection;

    void Update()
    {
        //Basically player interact with the NPC and stop moving

        // Checks if the dialogue box is open.
        // If the dialogue box is open, it does nothing and returns immediately,
        // preventing the player from moving while the conversation with the NPC is happening.
        if (dialogueUI.isOpen) return;


        // This is the key the player should press to interact with the NPC.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Checks if there is a referenced interactable object (Interactable) for the player.
            // This ensures that the player only tries to interact if there is actually something to interact with.
            if (Interactable != null)
            {
                // Calls the Interact method of the interactable object,
                // passing the player reference (this) as an argument, so the object knows who is interacting.
                Interactable.Interact(player: this);
            }
        }

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

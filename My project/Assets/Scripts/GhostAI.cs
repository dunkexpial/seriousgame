using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public float speed;
    public float radius;
    private GameObject player;
    private float distance;
    private bool hasLineOfSight = false;

    // LayerMask to ignore enemy collisions
    public LayerMask layerMask;
    public Animator animator;

    // Editable target raycast position on the player
    Vector2 playerTargetPositionOffset = new Vector2(0, 0);

    // Variables to store the last direction the ghost moved
    private float lastX;
    private float lastY;

    void Start()
    {
        // Encontra o GameObject com a tag "PlayerTag"
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            // Loop até encontrar o player
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if ((distance < radius) && hasLineOfSight)
        {
            // Move AI towards the player
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

            // Save the last direction the ghost moved
            lastX = direction.x;
            lastY = direction.y;

            // Pass the movement direction to the Animator
            animator.SetFloat("enemyX", direction.x);
            animator.SetFloat("enemyY", direction.y);
            animator.SetFloat("Moving", 1);
        }
        else
        {
            // If not moving, set movement to 0 but use lastX and lastY for idle direction
            animator.SetFloat("enemyX", 0);
            animator.SetFloat("enemyY", 0);
            animator.SetFloat("Moving", 0);

            // Set the last movement direction
            animator.SetFloat("lastX", lastX);
            animator.SetFloat("lastY", lastY);
        }
    }

    private void FixedUpdate() 
    {
        // Calculate the target position on the player based on the offset
        Vector2 targetPosition = (Vector2)player.transform.position + playerTargetPositionOffset;

        // Check for raycast
        RaycastHit2D ray = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, Mathf.Infinity, layerMask);
        if(ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
            if(hasLineOfSight)
            {
                Debug.DrawRay(transform.position, targetPosition - (Vector2)transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, targetPosition - (Vector2)transform.position, Color.red);
            }
        }
    }
}

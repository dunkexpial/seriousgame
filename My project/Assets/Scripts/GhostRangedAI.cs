using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRangedAI : MonoBehaviour
{
    public float speed;
    public float radius;
    public float inRange;
    public GameObject ghostProjectile;
    public Transform ghostProjPos;
    public LayerMask layerMask;
    public Animator animator;

    private GameObject player;
    private float distance;
    private bool hasLineOfSight = false;

    Vector2 playerTargetPositionOffset = new Vector2(0, 0);
    private float lastX;
    private float lastY;

    private float shootTimer;
    public float shootCooldown = 0.5f; // Time between shots

    void Start()
    {
        // Find the GameObject with the tag "PlayerTag"
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
        {
            // Loop until finding the player
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if (distance < radius && hasLineOfSight && distance > inRange)
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
        // Shooting logic: only shoot if within the radius
        if (distance < radius && hasLineOfSight)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootCooldown)
                {
                    shoot();
                    shootTimer = 0;
                }
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

    void shoot()
    {
        Instantiate(ghostProjectile, ghostProjPos.position, Quaternion.identity);
    }
}

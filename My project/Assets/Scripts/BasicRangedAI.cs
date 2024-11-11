using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedAI : MonoBehaviour
{
    public float speed;
    public float radius;
    public float inRange;
    public GameObject ghostProjectile;
    public GameObject ghostProjPos; // Changed to GameObject
    public LayerMask layerMask;
    public Animator animator;

    private GameObject player;
    private float distance;
    private bool hasLineOfSight = false;
    private bool hadLineOfSightLastFrame = false; // New variable to track previous line of sight

    private Vector2 playerTargetPositionOffset = new Vector2(0, 0);
    private float lastX;
    private float lastY;

    private float shootTimer;
    public float shootCooldown = 0.5f;

    // Float effect variables
    public float floatAmplitude = 2f;
    public float floatFrequency = 2f;
    private Vector3 basePosition; // Position without float effect

    private Vector2[] raycastOffsets = new Vector2[]
    {
        new Vector2(0, -2f),
        new Vector2(5.49f, -2f),
        new Vector2(-5.49f, -2f),
        new Vector2(0, 3.49f),
        new Vector2(0, -5.49f),
        new Vector2(5.49f, 3.49f),
        new Vector2(-5.49f, 3.49f),
        new Vector2(5.49f, -5.49f),
        new Vector2(-5.49f, -5.49f)
    };

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        basePosition = transform.position; // Set the initial base position
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        // Only update basePosition if moving towards the player
        if (distance < radius && hasLineOfSight && distance > inRange)
        {
            basePosition = Vector2.MoveTowards(basePosition, player.transform.position, speed * Time.deltaTime);

            lastX = direction.x;
            lastY = direction.y;

            animator.SetFloat("enemyX", direction.x);
            animator.SetFloat("enemyY", direction.y);
            animator.SetFloat("Moving", 1);
        }
        else
        {
            animator.SetFloat("enemyX", 0);
            animator.SetFloat("enemyY", 0);
            animator.SetFloat("Moving", 0);

            animator.SetFloat("lastX", lastX);
            animator.SetFloat("lastY", lastY);
        }

        // Calculate floating effect as a temporary offset
        float floatOffsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 visualPosition = basePosition;
        visualPosition.y += floatOffsetY;

        // Apply the floating position visually
        transform.position = visualPosition;

        // Shooting logic
        if (distance < radius && hasLineOfSight)
        {
            // If line of sight was gained this frame, reset shoot timer to 0 for an immediate shot
            if (!hadLineOfSightLastFrame)
            {
                shootTimer = shootCooldown; // Allow immediate shoot
            }

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootCooldown)
            {
                shoot();
                shootTimer = 0;
            }
        }

        // Update last frame line of sight status
        hadLineOfSightLastFrame = hasLineOfSight;
    }

    private void FixedUpdate() 
    {
        hasLineOfSight = false;

        foreach (Vector2 offset in raycastOffsets)
        {
            Vector2 targetPosition = (Vector2)player.transform.position + playerTargetPositionOffset + offset;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, Mathf.Infinity, layerMask);

            if(ray.collider != null && ray.collider.CompareTag("Player"))
            {
                hasLineOfSight = true;
                Debug.DrawRay(transform.position, targetPosition - (Vector2)transform.position, Color.green);
                break;
            }
            else
            {
                Debug.DrawRay(transform.position, targetPosition - (Vector2)transform.position, Color.red);
            }
        }
    }

    void shoot()
    {
        // Check if ghostProjPos is assigned before shooting
        if (ghostProjPos != null)
        {
            Instantiate(ghostProjectile, ghostProjPos.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No shooting point assigned for ghostProjPos!");
        }
    }
}

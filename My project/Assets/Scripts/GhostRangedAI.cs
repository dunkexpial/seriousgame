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
    public float shootCooldown = 0.5f;

    private Vector2[] raycastOffsets = new Vector2[]
    {
        new Vector2(0, -2f),
        new Vector2(5.49f, -2f),
        new Vector2(-5.49f, -2f),
        new Vector2(0, 13.49f),
        new Vector2(0, -17.49f),
        new Vector2(5.49f, 13.49f),
        new Vector2(-5.49f, 13.49f),
        new Vector2(5.49f, -17.49f),
        new Vector2(-5.49f, -17.49f)
    };

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

        if (distance < radius && hasLineOfSight && distance > inRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

            lastX = direction.x;
            lastY = direction.y;

            animator.SetFloat("enemyX", direction.x);
            animator.SetFloat("enemyY", direction.y);
            animator.SetFloat("Moving", 1);
        }

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
            animator.SetFloat("enemyX", 0);
            animator.SetFloat("enemyY", 0);
            animator.SetFloat("Moving", 0);

            animator.SetFloat("lastX", lastX);
            animator.SetFloat("lastY", lastY);
        }
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
        Instantiate(ghostProjectile, ghostProjPos.position, Quaternion.identity);
    }
}
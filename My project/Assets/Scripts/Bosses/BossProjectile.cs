using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float speed = 100;
    public float lifespan = 5f;

    // Rotation settings
    public float rotationSpeed = 360f; // Speed of rotation
    private int rotationDirection; // 1 for clockwise, -1 for counterclockwise

    // Inaccuracy settings
    public float inaccuracyAngle = 5f; // Maximum angle of inaccuracy in degrees
    private float difficulty;
    private float reverseDifficulty;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");

        // Calculate the direction from the projectile to the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Apply inaccuracy by adjusting the angle of the direction
        float randomAngle = Random.Range(-inaccuracyAngle, inaccuracyAngle);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        
        // Rotate the direction vector by the inaccuracy angle
        float rotatedX = direction.x * Mathf.Cos(angleInRadians) - direction.y * Mathf.Sin(angleInRadians);
        float rotatedY = direction.x * Mathf.Sin(angleInRadians) + direction.y * Mathf.Cos(angleInRadians);
        Vector3 inaccurateDirection = new Vector3(rotatedX, rotatedY, 0).normalized;

        // Set the velocity of the projectile towards the player with inaccuracy
        rb.velocity = inaccurateDirection * (speed * Mathf.Pow(difficulty, 0.5f));

        // Calculate the rotation to face the direction of movement
        float angle = Mathf.Atan2(inaccurateDirection.y, inaccurateDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Randomize the rotation direction: 1 for clockwise, -1 for counterclockwise
        rotationDirection = Random.Range(0, 2) == 0 ? 1 : -1;

        // Destroy the projectile after its lifespan
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Rotate the projectile based on the rotation direction
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * rotationDirection);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {   
        if (collider.CompareTag("ProjObstacle"))
        {
            // Only detach particles here if the projectile hits an obstacle
            DetachParticles();
            Destroy(gameObject); // Destroy on obstacle hit
        }
    }

    private void DetachParticles()
    {
        Transform particleSystemChild = transform.childCount > 0 ? transform.GetChild(0) : null;
        if (particleSystemChild != null)
        {
            particleSystemChild.parent = null;
            particleSystemChild.localScale = Vector3.one;
            Destroy(particleSystemChild.gameObject, 2f);
        }
    }
}

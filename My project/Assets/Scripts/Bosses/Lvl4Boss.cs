using UnityEngine;

public class Lvl4Boss : MonoBehaviour
{
    public float followForce = 10f;     // Force applied to move towards the player
    public float drag = 0.9f;          // How quickly the boss slows down (0 to 1, closer to 1 = stronger inertia)
    public float maxSpeed = 5f;        // The maximum speed the boss can move
    private Vector2 velocity;          // Current velocity of the boss
    private Transform playerTransform;

    void Start()
    {
        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No object tagged 'Player' found in the scene!");
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Calculate the direction to the player
        Vector2 directionToPlayer = (Vector2)(playerTransform.position - transform.position).normalized;

        // Apply a force towards the player
        velocity += directionToPlayer * followForce * Time.deltaTime;

        // Apply drag to simulate inertia
        velocity *= drag;

        // Clamp the velocity to the maximum speed
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        // Move the boss
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}

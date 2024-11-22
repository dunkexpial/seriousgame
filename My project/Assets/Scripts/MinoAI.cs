using UnityEngine;

public class PowerUpActivator : MonoBehaviour
{
    public float powerUpDuration = 5f; // Duration of the power-up
    public float spawnRate = 0.5f; // Rate of spawning prefabs during power-up
    public float powerUpCooldown = 10f; // Time between power-up activations
    public GameObject prefabUp, prefabDown, prefabLeft, prefabRight; // Prefabs for each direction
    public float multiplier = 3f;

    private BasicAI basicAI;
    private Animator animator;
    private Transform player; // Reference to the player
    private float originalSpeed;
    private float originalAnimationSpeed;
    private float powerUpTimer;
    private float spawnTimer;
    private float cooldownTimer;
    private bool isPowerUpActive;

    void Start()
    {
        basicAI = GetComponent<BasicAI>();
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (basicAI == null)
        {
            Debug.LogError("No BasicAI script found on this GameObject!");
            enabled = false;
            return;
        }

        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject!");
            enabled = false;
            return;
        }

        if (playerObject == null)
        {
            Debug.LogError("No GameObject with the 'Player' tag found in the scene!");
            enabled = false;
            return;
        }

        player = playerObject.transform;
        originalSpeed = basicAI.speed;
        originalAnimationSpeed = animator.speed;
    }

    void Update()
    {
        // Calculate distance and direction to the player
        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (Vector2)player.position - (Vector2)transform.position;

        // Handle cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Check for power-up activation
        if (!isPowerUpActive && cooldownTimer <= 0f && distance < basicAI.radius && basicAI.hasLineOfSight)
        {
            ActivatePowerUp();
        }

        // Manage the power-up behavior
        if (isPowerUpActive)
        {
            powerUpTimer -= Time.deltaTime;
            spawnTimer -= Time.deltaTime;

            // Spawn prefab based on direction
            if (spawnTimer <= 0f)
            {
                SpawnPrefabBasedOnDirection(direction);
                spawnTimer = spawnRate; // Reset spawn timer
            }

            // End power-up when time runs out
            if (powerUpTimer <= 0f)
            {
                DeactivatePowerUp();
            }
        }
    }

    void ActivatePowerUp()
    {
        isPowerUpActive = true;
        powerUpTimer = powerUpDuration;
        spawnTimer = 0f; // Start spawning immediately
        basicAI.speed *= multiplier; // Triple speed
        animator.speed *= multiplier; // Triple animation speed
    }

    void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        basicAI.speed = originalSpeed; // Reset speed
        animator.speed = originalAnimationSpeed; // Reset animation speed
        cooldownTimer = powerUpCooldown; // Set cooldown
    }

    void SpawnPrefabBasedOnDirection(Vector2 direction)
    {
        GameObject prefabToSpawn = null;

        // Choose the prefab based on direction
        if (direction.x > Mathf.Abs(direction.y)) // Moving right
        {
            prefabToSpawn = prefabRight;
        }
        else if (direction.x < -Mathf.Abs(direction.y)) // Moving left
        {
            prefabToSpawn = prefabLeft;
        }
        else if (direction.y > 0) // Moving up
        {
            prefabToSpawn = prefabUp;
        }
        else if (direction.y < 0) // Moving down
        {
            prefabToSpawn = prefabDown;
        }

        // Spawn the chosen prefab
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}

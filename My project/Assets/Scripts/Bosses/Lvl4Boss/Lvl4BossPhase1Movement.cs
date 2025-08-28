using System.Collections;
using UnityEngine;

public class Lvl4BossPhase1Movement : MonoBehaviour
{
    [Header("Target Tag")]
    [SerializeField] private string targetTag = "Boss4Point";  // Tag of the objects to move to
    private GameObject[] targets;        // All objects with the specified tag
    private Transform currentTarget;     // The current target object
    public float maxSpeed = 600f;       // Maximum speed
    private float minSpeed = 1f;         // Minimum speed to avoid stopping
    private bool isMoving = false;
    private float difficulty;
    private float reverseDifficulty;

    // Shooting behavior
    public bool canShoot = false;        // Boolean that determines if the boss can shoot
    private float shootCooldown = 1f;    // 1 second delay after each move before shooting is allowed

    void Start()
    {
        // Find all objects with the specified tag
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");

        // Start the movement coroutine
        StartCoroutine(MoveToRandomTarget());
    }

    private IEnumerator MoveToRandomTarget()
    {
        while (true) // Infinite loop to ensure it keeps moving
        {
            isMoving = true;

            // Filter targets: only keep those with the same name as this boss
            GameObject[] validTargets = System.Array.FindAll(targets, t => t.name == gameObject.name);

            if (validTargets.Length == 0)
            {
                Debug.LogWarning($"No object with tag '{targetTag}' and name '{gameObject.name}' found.");
                yield break; // Stop if no valid targets exist
            }

            // Choose a random valid target
            currentTarget = validTargets[Random.Range(0, validTargets.Length)].transform;

            // Store the starting position for progress calculation
            Vector2 startPosition = transform.position;
            Vector2 targetPosition = currentTarget.position;
            float journeyLength = Vector2.Distance(startPosition, targetPosition);
            float halfwayPoint = journeyLength / 2;
            float currentSpeed = 0f;
            float distanceTraveled = 0f;

            // Move towards the target
            while (distanceTraveled < journeyLength)
            {
                float remainingDistance = Vector2.Distance(transform.position, targetPosition);
                distanceTraveled = journeyLength - remainingDistance;

                if (distanceTraveled < halfwayPoint)
                {
                    currentSpeed = Mathf.Lerp(0f, maxSpeed * Mathf.Pow(difficulty, 1f), distanceTraveled / halfwayPoint);
                }
                else
                {
                    currentSpeed = Mathf.Lerp(maxSpeed * Mathf.Pow(difficulty, 1f), minSpeed,
                        (distanceTraveled - halfwayPoint) / (journeyLength - halfwayPoint));
                }

                currentSpeed = Mathf.Max(currentSpeed, minSpeed);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

                yield return null;
            }

            transform.position = targetPosition;
            canShoot = true;

            yield return new WaitForSeconds(shootCooldown);

            canShoot = false;
            isMoving = false;

            Debug.Log($"{gameObject.name} reached its target {currentTarget.name}.");
        }
    }
}

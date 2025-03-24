using System.Collections;
using UnityEngine;

public class MoveBetweenTaggedObjects : MonoBehaviour
{
    [Header("Target Tag")]
    public string targetTag = "Boss2FMP";  // Tag of the objects to move to
    private GameObject[] targets;        // Array to hold all objects with the specified tag
    private Transform currentTarget;     // The current target object
    public float maxSpeed = 800f;       // Maximum speed
    private float minSpeed = 1f;         // Minimum speed to avoid stopping
    private bool isMoving = false;

    void Start()
    {
        // Find all objects with the specified tag and store them in the array
        targets = GameObject.FindGameObjectsWithTag(targetTag);

        // Start the movement coroutine
        StartCoroutine(MoveToRandomTarget());
    }

    private IEnumerator MoveToRandomTarget()
    {
        while (true) // Infinite loop to ensure it keeps moving
        {
            isMoving = true;

            // Choose a random target from the list of targets
            currentTarget = targets[Random.Range(0, targets.Length)].transform;

            // Store the starting position for progress calculation
            Vector2 startPosition = transform.position;
            Vector2 targetPosition = currentTarget.position;
            float journeyLength = Vector2.Distance(startPosition, targetPosition);
            float halfwayPoint = journeyLength / 2;  // Halfway point of the journey
            float currentSpeed = 0f;  // Start speed at 0
            float distanceTraveled = 0f;  // Track how much distance has been traveled

            // Move towards the target
            while (distanceTraveled < journeyLength)
            {
                // Calculate the remaining distance
                float remainingDistance = Vector2.Distance(transform.position, targetPosition);

                // Calculate the distance traveled
                distanceTraveled = journeyLength - remainingDistance;

                // Gradually increase the speed until the halfway point
                if (distanceTraveled < halfwayPoint)
                {
                    currentSpeed = Mathf.Lerp(0f, maxSpeed, distanceTraveled / halfwayPoint);
                }
                else
                {
                    // Gradually decrease the speed after the halfway point
                    currentSpeed = Mathf.Lerp(maxSpeed, minSpeed, (distanceTraveled - halfwayPoint) / (journeyLength - halfwayPoint));
                }

                // Ensure that the speed doesn't drop below the minimum threshold
                currentSpeed = Mathf.Max(currentSpeed, minSpeed);

                // Move the object towards the target
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

                yield return null;  // Wait until the next frame
            }

            // Directly set the position to the target to avoid small floating-point errors
            transform.position = targetPosition;

            // Wait for a random duration before moving to the next target
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

            isMoving = false;
        }
    }
}

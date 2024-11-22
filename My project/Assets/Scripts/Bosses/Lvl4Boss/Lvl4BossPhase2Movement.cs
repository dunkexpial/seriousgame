using UnityEngine;
using System.Collections;

public class Lvl4BossPhase2Movement : MonoBehaviour
{
    [Header("Target Tag")]
    private string targetTag = "Boss4Point2"; // Tag of the object to move to
    private GameObject target;              // The target object
    public float maxSpeed = 1000f;          // Maximum speed
    private float minSpeed = 1f;            // Minimum speed to avoid stopping

    void Start()
    {
        // Find the object with the specified tag
        target = GameObject.FindGameObjectWithTag(targetTag);

        // If a target is found, move to it
        if (target != null)
        {
            StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.LogWarning($"No object with tag '{targetTag}' found.");
        }
    }

    private IEnumerator MoveToTarget()
    {
        Transform targetTransform = target.transform;

        // Store the starting position and calculate the journey length
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = targetTransform.position;
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

        Debug.Log("Boss reached the target.");
    }
}

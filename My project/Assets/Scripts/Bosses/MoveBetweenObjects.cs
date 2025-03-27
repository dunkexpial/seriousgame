using System.Collections;
using UnityEngine;

public class MoveBetweenObjects : MonoBehaviour
{
    [Header("Target Objects")]
    public string objectName1 = "Object1";  // Name of the first target object
    public string objectName2 = "Object2";  // Name of the second target object
    private Transform target1;
    private Transform target2;
    private Transform currentTarget;
    private float maxSpeed = 800f;          // Maximum speed
    private float minSpeed = 10f;            // Minimum speed to avoid stopping
    private float moveSpeed;                // Speed that will be applied during movement
    private bool isMoving = false;
    private float difficulty;
    private float reverseDifficulty;

    void Start()
    {
        // Find the target objects by name
        target1 = GameObject.Find(objectName1)?.transform;
        target2 = GameObject.Find(objectName2)?.transform;
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");

        // Debug if the objects are found
        if (target1 == null)
        {
        }
        else
        {
            Debug.Log($"Found target 1: {objectName1} at {target1.position}");
        }

        if (target2 == null)
        {
            Debug.LogError($"Target object '{objectName2}' not found!");
        }
        else
        {
            Debug.Log($"Found target 2: {objectName2} at {target2.position}");
        }

        // Check if both targets exist
        if (target1 == null || target2 == null)
        {
            Debug.LogError("One or both target objects could not be found!");
            return;
        }

        // Start at one of the targets
        currentTarget = target1;

        // Start the movement coroutine
        StartCoroutine(MoveBetweenTargets());
    }

    void Update()
    {
        if (!isMoving)
        {
            // Randomize the move speed each time the object starts moving
            moveSpeed = Random.Range(minSpeed*20, maxSpeed * difficulty);
            isMoving = true;
        }
    }

    // Coroutine to move between the two targets
    private IEnumerator MoveBetweenTargets()
    {
        while (true)
        {
            // Store the starting position for progress calculation
            Vector2 startPosition = transform.position;
            Vector2 targetPosition = currentTarget.position;
            float journeyLength = Vector2.Distance(startPosition, targetPosition);
            float halfwayPoint = journeyLength / 2;
            float currentSpeed = 0f;  // Start speed at 0
            float distanceTraveled = 0f;  // Track the distance traveled

            // Move towards the current target
            while (distanceTraveled < journeyLength)
            {
                // Calculate the remaining distance
                float remainingDistance = Vector2.Distance(transform.position, targetPosition);
                distanceTraveled = journeyLength - remainingDistance;

                // Gradually increase the speed until the halfway point
                if (distanceTraveled < halfwayPoint)
                {
                    currentSpeed = Mathf.Lerp(0f, moveSpeed, distanceTraveled / halfwayPoint);
                }
                else
                {
                    // Gradually decrease the speed after the halfway point
                    currentSpeed = Mathf.Lerp(moveSpeed, minSpeed, (distanceTraveled - halfwayPoint) / (journeyLength - halfwayPoint));
                }

                // Ensure that the speed doesn't drop below the minimum threshold
                currentSpeed = Mathf.Max(currentSpeed, minSpeed);

                // Move the object towards the target
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

                yield return null;  // Wait until the next frame
            }

            // Directly set the position to the target to avoid small floating-point errors
            transform.position = targetPosition;

            // Once we reach the target, pick the next target and wait a moment before moving again
            currentTarget = (currentTarget == target1) ? target2 : target1;

            // Randomize the move speed for the next target movement
            moveSpeed = Random.Range(minSpeed, maxSpeed);

            // Wait for a random duration before starting the next move
            yield return new WaitForSeconds(Random.Range(0.1f, 2.5f));

            isMoving = false;
        }
    }
}

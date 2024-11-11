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
    private float moveSpeed;
    private bool isMoving = false;

    void Start()
    {
        // Find the target objects by name
        target1 = GameObject.Find(objectName1)?.transform;
        target2 = GameObject.Find(objectName2)?.transform;

        // Debug if the objects are found
        if (target1 == null)
        {
            Debug.LogError($"Target object '{objectName1}' not found!");
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
            // Choose a random move speed between a minimum and maximum value
            moveSpeed = Random.Range(100f, 200f);
            Debug.Log($"Move speed set to: {moveSpeed}");
            isMoving = true;
        }
    }

    // Coroutine to move between the two targets
    private IEnumerator MoveBetweenTargets()
    {
        while (true)
        {
            // Debug current target and position
            Debug.Log($"Moving towards {currentTarget.name} at {currentTarget.position}");

            // Move towards the current target using MoveTowards
            while (Vector2.Distance(transform.position, currentTarget.position) > 0.1f)
            {
                // Move the object towards the target
                transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

                // Optionally log position to verify movement
                Debug.Log($"Current position: {transform.position}");

                yield return null;
            }

            // Once we reach the target, log the movement completion
            Debug.Log($"Reached {currentTarget.name} at position {transform.position}");

            // Once we reach the target, pick the next target and wait a moment before moving again
            currentTarget = (currentTarget == target1) ? target2 : target1;
            isMoving = false;

            // Wait for a moment before starting the next move
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }
}

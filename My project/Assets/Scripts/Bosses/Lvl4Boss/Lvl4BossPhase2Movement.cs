using UnityEngine;
using System.Collections;

public class Lvl4BossPhase2Movement : MonoBehaviour
{
    [Header("Target Tag")]
    [SerializeField] private string targetTag = "Boss4Point2"; // Tag of the object to move to
    private GameObject target;               // The target object
    public float maxSpeed = 600f;           // Maximum speed
    private float minSpeed = 1f;             // Minimum speed to avoid stopping
    public bool canShoot = false;
    private float difficulty;
    private float reverseDifficulty;

    void Start()
    {
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");

        // Find all objects with the specified tag
        GameObject[] candidates = GameObject.FindGameObjectsWithTag(targetTag);

        // Look for the one that has the same name as this boss
        foreach (GameObject candidate in candidates)
        {
            if (candidate.name == gameObject.name)
            {
                target = candidate;
                break;
            }
        }

        // If a valid target is found, move to it
        if (target != null)
        {
            StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.LogWarning($"No object with tag '{targetTag}' and name '{gameObject.name}' found.");
        }
    }

    private IEnumerator MoveToTarget()
    {
        Transform targetTransform = target.transform;

        // Store the starting position and calculate the journey length
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = targetTransform.position;
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
                currentSpeed = Mathf.Lerp(0f, maxSpeed * Mathf.Pow(difficulty, 0.5f), distanceTraveled / halfwayPoint);
            }
            else
            {
                currentSpeed = Mathf.Lerp(maxSpeed * Mathf.Pow(difficulty, 0.5f), minSpeed, (distanceTraveled - halfwayPoint) / (journeyLength - halfwayPoint));
            }

            currentSpeed = Mathf.Max(currentSpeed, minSpeed);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = targetPosition;
        canShoot = true;
        Debug.Log($"{gameObject.name} reached its target {target.name}.");
    }
}

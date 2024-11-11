using System.Collections;
using UnityEngine;

public class TeleportOnProximity : MonoBehaviour
{
    public float teleportRadius = 200f;
    public float safeDistanceFromPlayer = 80f;
    public string obstacleTag = "Obstacle";
    public GameObject teleportMarkerPrefab;
    public float teleportChance = 0.1f;
    public float markerLifetime = 0.5f;

    private Transform playerTransform;
    private BasicAI basicAI; // Reference to the BasicAI component

    void Start()
    {
        // Find the player object by tag and the BasicAI component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        basicAI = GetComponent<BasicAI>(); // Get the BasicAI component attached to this GameObject

        if (playerTransform != null && basicAI != null)
        {
            StartCoroutine(TeleportCheckRoutine());
        }
        else
        {
            Debug.LogError("Player or BasicAI component not found!");
        }
    }

    private IEnumerator TeleportCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second between teleport checks

            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= teleportRadius)
            {
                // Only proceed if the AI has line of sight to the player
                if (basicAI.hasLineOfSight && Random.value <= teleportChance)
                {
                    Vector2 teleportPosition = FindSafeTeleportPosition();
                    if (teleportPosition != Vector2.zero)
                    {
                        SpawnTeleportMarkers(transform.position, teleportPosition);
                        transform.position = teleportPosition;
                    }
                }
            }
        }
    }

    private Vector2 FindSafeTeleportPosition()
    {
        Vector2 playerPosition = playerTransform.position;

        // Try a few random positions within the radius to find an open spot
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector2 candidatePosition = playerPosition + randomDirection * safeDistanceFromPlayer;

            // Check if the candidate position is free of obstacles
            Collider2D hit = Physics2D.OverlapCircle(candidatePosition, 0.5f);
            if (hit == null || !hit.CompareTag(obstacleTag))
            {
                return candidatePosition;
            }
        }

        // Return zero vector if no suitable position was found
        return Vector2.zero;
    }

    private void SpawnTeleportMarkers(Vector2 startPosition, Vector2 endPosition)
    {
        if (teleportMarkerPrefab != null)
        {
            GameObject startMarker = Instantiate(teleportMarkerPrefab, startPosition, Quaternion.identity);

            Destroy(startMarker, markerLifetime);
        }
    }
}

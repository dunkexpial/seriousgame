using System.Collections;
using UnityEngine;

public class TeleportOnProximity : MonoBehaviour
{
    public float teleportRadius = 200f; // Distance within which teleporting can occur
    public float safeDistanceFromPlayer = 80f; // Minimum distance from player after teleport
    public string obstacleTag = "Obstacle"; // Tag for obstacles (e.g., walls)
    public GameObject teleportMarkerPrefab; // Prefab to spawn at the teleport start and end positions
    public float teleportChance = 0.1f; // 10% chance of teleporting each second
    public float markerLifetime = 0.5f; // Lifetime of the teleport markers in seconds

    private Transform playerTransform;

    void Start()
    {
        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            StartCoroutine(TeleportCheckRoutine());
        }
        else
        {
            Debug.LogError("Player not found in the scene. Make sure the player is tagged as 'Player'.");
        }
    }

    private IEnumerator TeleportCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second between teleport checks

            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= teleportRadius)
            {
                // Random chance for teleportation to occur
                if (Random.value <= teleportChance)
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

            // Destroy the markers after `markerLifetime` seconds
            Destroy(startMarker, markerLifetime);
        }
    }
}

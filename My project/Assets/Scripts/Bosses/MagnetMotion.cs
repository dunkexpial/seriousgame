using UnityEngine;

public class MagnetMotion : MonoBehaviour
{
    public string playerTag = "Player"; // The tag of the player
    private Transform playerTransform; // Reference to the player's transform
    public float angleOffset = 0f; // Angle offset in degrees
    public float rotationSpeed = 2f; // Speed of rotation towards the player

    void Start()
    {
        // Find the player in the scene by tag
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform; // Store the player's transform
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the player has the correct tag.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction to the player
            Vector3 direction = playerTransform.position - transform.position;
            direction.z = 0; // Set z to 0 for 2D

            // Calculate the rotation needed to look at the player
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            // Apply the angle offset
            targetRotation *= Quaternion.Euler(0, 0, angleOffset);

            // Smoothly interpolate between the current rotation and the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

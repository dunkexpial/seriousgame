using UnityEngine;

public class TeleportToMouse : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode teleportKey = KeyCode.Space; // Key to trigger teleport
    public float maxDistance = 5f; // Maximum teleportation distance
    public float cooldownTime = 1f; // Cooldown duration in seconds
    public GameObject teleportEffectPrefab; // Prefab to spawn at teleport locations
    public GameObject teleportIndicatorPrefab; // Prefab to display at potential teleport location
    public LayerMask obstacleLayerMask; // Layer mask for obstacles
    private float lastTeleportTime = -Mathf.Infinity; // Tracks the last teleport time
    private GameObject teleportIndicatorInstance; // Instance of the teleport indicator
    private SoundManager soundManager;
    private DialogueUI dialogueUI;

    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        dialogueUI = FindAnyObjectByType<DialogueUI>();
        // Instantiate the teleport indicator prefab
        if (teleportIndicatorPrefab != null)
        {
            teleportIndicatorInstance = Instantiate(teleportIndicatorPrefab);
            teleportIndicatorInstance.SetActive(false);
        }
    }

    void Update()
    {
        // Update teleport indicator position
        UpdateTeleportIndicator();
        // if (DialogueUI.isOpen || PauseMenu.isPaused) return;
        if ((dialogueUI != null && dialogueUI.isOpen) || PauseMenu.isPaused) return;


        // Check if the teleport key is pressed and cooldown has elapsed
        if (Input.GetKeyDown(teleportKey) && Time.time >= lastTeleportTime + cooldownTime)
        {
            if (Teleport())
            {
                lastTeleportTime = Time.time;
            }
        }
    }

    void UpdateTeleportIndicator()
    {
        if (teleportIndicatorInstance == null)
        {
            return;
        }

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure Z position is 0 for 2D

        // Calculate the direction and distance from the object to the mouse
        Vector3 directionToMouse = mousePosition - transform.position;
        float distanceToMouse = directionToMouse.magnitude;

        // Clamp the target position to the maximum distance
        Vector3 clampedTargetPosition = distanceToMouse > maxDistance
            ? transform.position + directionToMouse.normalized * maxDistance
            : mousePosition;

        // Perform a raycast to find the nearest valid position if blocked
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToMouse.normalized, maxDistance, obstacleLayerMask);
        if (hit.collider != null && hit.distance < distanceToMouse)
        {
            clampedTargetPosition = transform.position + directionToMouse.normalized * Mathf.Min(hit.distance, maxDistance);
        }

        // If the mouse position is closer than the obstacle, prioritize the mouse position
        if (distanceToMouse <= maxDistance && (hit.collider == null || hit.distance > distanceToMouse))
        {
            clampedTargetPosition = mousePosition;
        }

        // Update the position of the teleport indicator
        teleportIndicatorInstance.transform.position = clampedTargetPosition;
        teleportIndicatorInstance.SetActive(true);
    }

    bool Teleport()
    {
        if (teleportIndicatorInstance == null || !teleportIndicatorInstance.activeSelf)
        {
            return false;
        }

        // Store the current position for spawning the effect
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = teleportIndicatorInstance.transform.position;

        // Teleport to the target position
        transform.position = targetPosition;
        soundManager.PlaySoundBasedOnCollision("PlayerTeleport");

        // Spawn the teleport effect at the original and new positions
        if (teleportEffectPrefab != null)
        {
            Instantiate(teleportEffectPrefab, originalPosition, Quaternion.identity);
            Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
        }

        teleportIndicatorInstance.SetActive(false);
        return true; // Successful teleport
    }
}

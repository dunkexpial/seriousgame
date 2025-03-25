using UnityEngine;

public class MagnetMotion : MonoBehaviour
{
    public string playerTag = "Player"; // The tag of the player
    private Transform playerTransform; // Reference to the player's transform
    public float angleOffset = 0f; // Angle offset in degrees
    public float rotationSpeed = 2f; // Speed of rotation towards the player
    public float rotationSpeed360 = 30f; // Speed of rotation during 360 mode
    public float minSwitchInterval = 10f; // Minimum time in seconds between switching modes
    public float maxSwitchInterval = 10f; // Maximum time in seconds between switching modes
    public bool enableAlternation = false; // Whether the alternation is enabled
    private bool pointingAtPlayer = true; // Whether it's currently pointing at the player
    private float switchTimer; // Timer for switching modes
    private float rotationProgress = 0f; // Tracks rotation progress in degrees

    public float startRotationAngle = 0f; // Starting angle for the 360 rotation
    private float rotationStartTime; // Time when the rotation starts
    private float rotationDuration; // Duration to complete one full rotation

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        SetRandomSwitchInterval(); // Set the initial random switch interval
        rotationProgress = startRotationAngle; // Set the initial rotation to the startRotationAngle

        // Calculate the duration for a full 360-degree rotation based on the speed
        rotationDuration = 560f / rotationSpeed360;
    }

    void Update()
    {
        if (enableAlternation)
        {
            if (pointingAtPlayer)
            {
                switchTimer -= Time.deltaTime;
                if (switchTimer <= 0)
                {
                    pointingAtPlayer = false; // Switch to 360 rotation mode
                    SetRandomSwitchInterval(); // Set a new random switch interval
                    rotationProgress = startRotationAngle; // Reset rotation progress to start angle
                    rotationStartTime = Time.time; // Start timing the rotation
                    ToggleFreezeShooting(true); // Freeze shooting while rotating
                }
            }
        }

        if (pointingAtPlayer && playerTransform != null)
        {
            // Look at the player
            Vector3 direction = playerTransform.position - transform.position;
            direction.z = 0;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            targetRotation *= Quaternion.Euler(0, 0, angleOffset);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // Disable Shooting children and enable EnergyBeam children while pointing at the player
            ToggleChildren(true);
        }
        else
        {
            // Perform full 360-degree rotation with smooth transition
            if (Time.time - rotationStartTime < rotationDuration) // Check if the rotation time is not over
            {
                float rotationStep = rotationSpeed360 * Time.deltaTime; // Use independent speed
                rotationProgress += rotationStep;

                // Interpolate the rotation for smoother transition
                float lerpFactor = rotationProgress / 360f;
                Quaternion targetRotation = Quaternion.Euler(0, 0, rotationProgress);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpFactor);
            }
            else
            {
                rotationProgress = 0f; // Reset rotation progress to 0 after completing a full rotation
                pointingAtPlayer = true; // Return to tracking the player
                ToggleFreezeShooting(false); // Resume shooting
            }

            // Disable EnergyBeam children and enable Shooting children during the rotation mode
            ToggleChildren(false);
        }
    }

    private void ToggleChildren(bool isPointingAtPlayer)
    {
        foreach (Transform child in transform)
        {
            // Check if the child is named "Shooting" or "EnergyBeam" and toggle active state
            if (child.name.Contains("EnergyBeam"))
            {
                child.gameObject.SetActive(!isPointingAtPlayer); // Disable Shooting if not pointing at player
            }
            else if (child.name.Contains("Shooting"))
            {
                child.gameObject.SetActive(isPointingAtPlayer); // Enable EnergyBeam if pointing at player
            }
        }
    }

    private void SetRandomSwitchInterval()
    {
        switchTimer = Random.Range(minSwitchInterval, maxSwitchInterval); // Set a random time between min and max
    }

    private void ToggleFreezeShooting(bool freeze)
    {
        foreach (Transform child in transform)
        {
            BossShooting bossShooting = child.GetComponent<BossShooting>();
            if (bossShooting != null)
            {
                bossShooting.FreezeShooting(freeze);
            }
        }
    }
}

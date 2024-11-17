using UnityEngine;

public class DestroyIfFarFromPlayer : MonoBehaviour
{
    public float threshold = 0.1f;  // Threshold distance from the viewport edge
    private Camera mainCamera;       // Reference to the camera

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Get the object's position in the viewport
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

            // Check if the object is outside the viewport (adding a threshold buffer)
            if (viewportPosition.x < -threshold || viewportPosition.x > 1 + threshold ||
                viewportPosition.y < -threshold || viewportPosition.y > 1 + threshold)
            {
                Destroy(gameObject);
            }
        }
    }
}

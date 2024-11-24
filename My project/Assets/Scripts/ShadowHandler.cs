using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, -0.5f, 0); // Offset below the projectile

    private Transform parentTransform;

    void Start()
    {
        // Store the parent transform for reference
        parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogError("ShadowController: This object must have a parent!");
        }
    }

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            // Keep the shadow in the offset position relative to the parent
            transform.position = parentTransform.position + offset;
            // Optional: Match the rotation of the parent if needed, or keep it static
            transform.rotation = Quaternion.identity; // Keeps the shadow from rotating
        }
    }
}
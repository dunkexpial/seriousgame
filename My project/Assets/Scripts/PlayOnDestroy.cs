using UnityEngine;

public class PlayOnDestroy : MonoBehaviour
{
    public GameObject smokePrefab; // Assign the smoke prefab here
    public float smokeLifetime = 0.5f; // Adjust this to match your animation length

    // This method can be called by other scripts or on destruction
    void OnDestroy()
    {
        // Instantiate the smoke at the object's position
        if (smokePrefab != null)
        {
            GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
            Destroy(smoke, smokeLifetime); // Destroy smoke after the animation duration
        }
        else
        {
            Debug.LogWarning("Smoke prefab not assigned!");
        }
    }
}

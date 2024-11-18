using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Time in seconds before the object is destroyed
    public float timeBeforeDestroy = 2f;

    void Start()
    {
        // Destroy the object after the specified time
        Destroy(gameObject, timeBeforeDestroy);
    }
}


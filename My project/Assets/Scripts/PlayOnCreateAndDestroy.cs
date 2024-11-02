using UnityEngine;

public class PlayOnCreateAndDestroy : MonoBehaviour
{
    [SerializeField] private GameObject createSmokePrefab; // Smoke prefab for creation
    [SerializeField] private GameObject destroySmokePrefab; // Smoke prefab for destruction
    [SerializeField] private float createSmokeLifetime = 0.5f; // Creation animation duration
    [SerializeField] private float destroySmokeLifetime = 0.5f; // Destruction animation duration

    void Start()
    {
        PlayCreateSmokeEffect();
    }

    void OnDestroy()
    {
        PlayDestroySmokeEffect();
    }

    private void PlayCreateSmokeEffect()
    {
        if (createSmokePrefab != null)
        {
            GameObject smoke = Instantiate(createSmokePrefab, transform.position, Quaternion.identity);
            Destroy(smoke, createSmokeLifetime);
        }
    }

    private void PlayDestroySmokeEffect()
    {
        if (destroySmokePrefab != null)
        {
            GameObject smoke = Instantiate(destroySmokePrefab, transform.position, Quaternion.identity);
            Destroy(smoke, destroySmokeLifetime);
        }
    }
}

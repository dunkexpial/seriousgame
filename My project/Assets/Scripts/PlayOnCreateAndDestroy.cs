using UnityEngine;

public class PlayOnCreateAndDestroy : MonoBehaviour
{
    [SerializeField] private GameObject createSmokePrefab;
    [SerializeField] private GameObject destroySmokePrefab;
    [SerializeField] private float createSmokeLifetime = 0.5f;
    [SerializeField] private float destroySmokeLifetime = 0.5f;

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

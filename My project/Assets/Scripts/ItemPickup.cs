using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private SoundManager soundManager;

    void Start()
    {
        // Localiza o SoundManager na cena
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager não encontrado na cena!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (soundManager != null)
            {
                soundManager.PlayItemPickupSound();
            }

            Destroy(gameObject);
        }
    }
    
}

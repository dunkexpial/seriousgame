using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip playerProjParede;
    public AudioClip enemyDamage;
    public AudioClip EnemyProjParede;
    public AudioClip itemPickupSound; 
    public AudioClip itemHeal;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySoundBasedOnCollision(string collisionPair)
    {
        Debug.Log("Par de colisão: " + collisionPair);
        switch (collisionPair)
        {
            case "PlayerProjParede":
                if (playerProjParede != null)
                {
                    audioSource.PlayOneShot(playerProjParede);
                }
                break;

            case "DanoInimigo":
                if (enemyDamage != null)
                {
                    audioSource.PlayOneShot(enemyDamage);
                }
                break;

            case "EnemyProjParede":
                if (EnemyProjParede != null)
                {
                    audioSource.PlayOneShot(EnemyProjParede);
                }
                break;

            case "itemHeal":
                if (itemHeal != null)
                {
                    audioSource.PlayOneShot(itemHeal);
                }
                break;

            default:
                Debug.LogWarning("Par não reconhecido: " + collisionPair);
                break;
        }
    }

    public void PlayItemHealPickupSound()
    {
        if (itemPickupSound != null)
        {
            audioSource.PlayOneShot(itemHeal);
        }
        else
        {
            Debug.LogWarning("Som de itemPickupSound não configurado no SoundManager!");
        }
    }

    public void PlayItemPickupSound()
    {
        if (itemPickupSound != null)
        {
            audioSource.PlayOneShot(itemPickupSound);
        }
        else
        {
            Debug.LogWarning("Som de itemPickupSound não configurado no SoundManager!");
        }
    }
}

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip playerProjParede;
    public AudioClip enemyDamage;
    public AudioClip EnemyProjParede;

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

            default:
                Debug.LogWarning("Par n reconhecido: " + collisionPair);
                break;
        }
    }
}

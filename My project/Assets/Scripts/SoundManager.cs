using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Mantém entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Função para tocar som aleatório
    public void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];  // Escolhe um som aleatório
            audioSource.PlayOneShot(clip);
        }
    }
}
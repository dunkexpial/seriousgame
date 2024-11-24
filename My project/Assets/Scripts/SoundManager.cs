using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;
    public string tag1 = "Tag1";
    public string tag2 = "Tag2";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    public void PlayarSom(Collider2D colliderA, Collider2D colliderB)
    {
        if ((colliderA.CompareTag(tag1) && colliderB.CompareTag(tag2)) ||
            (colliderA.CompareTag(tag2) && colliderB.CompareTag(tag1)))
            Debug.Log('som tocando')
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
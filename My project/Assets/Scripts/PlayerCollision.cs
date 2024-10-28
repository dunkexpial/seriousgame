using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private HealthManager healthManager;
    public AudioClip damageClip;  // Som de dano
    private AudioSource audioSource;

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();  //Acess Health manager and restart the regen timer 
        audioSource = GetComponent<AudioSource>();  // Obtém o AudioSource do jogador                                                                                                      

    private void OnCollisionEnter2D(Collision2D collision) {

        if(collision.transform.tag == "Enemy")
        {
            
            HealthManager.health--;
            healthManager.ResetRegenTimer();

            if (HealthManager.health <= 0)
            {
                PlayerManager.GameOver = true;
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(TakeDamage());
                PlayDamageSound();  // Toca o som de dano
            }
        }
    }

    IEnumerator TakeDamage()
    {
        isInvincible = true;  // Activate invincibility
        GetComponent<Animator>().SetLayerWeight(1, 1);

        yield return new WaitForSeconds(2);

        GetComponent<Animator>().SetLayerWeight(1, 0);
        isInvincible = false;  // End invincibility
    }
    private void PlayDamageSound()
    {
        if (damageClip != null)
        {
            audioSource.PlayOneShot(damageClip);  // Toca o som de dano
        }
    }
}
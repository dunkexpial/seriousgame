using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private HealthManager healthManager;
    private bool isInvincible = false;
    public AudioClip damageClip;  // Som de dano
    private AudioSource audioSource;

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
       	audioSource = GetComponent<AudioSource>();  // Obtém o AudioSource do jogador
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if player is invincible before applying damage
        if ((collider.transform.tag == "Enemy" || collider.transform.tag == "EnemyProjectile") && !isInvincible)
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
		PlayDamageSound();  // Toca o som de dano
                StartCoroutine(TakeDamage());  // Trigger invincibility and damage effects
            }

            if (collider.transform.tag == "EnemyProjectile")
            {
                Destroy(collider.gameObject);
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
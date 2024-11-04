using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private HealthManager healthManager;
    public bool isInvincible = false;

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        // Check if player is invincible before applying damage
        if ((collider.transform.tag == "Enemy" || collider.transform.tag == "EnemyProjectile"))
        {
            if (!isInvincible)
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
                StartCoroutine(TakeDamage());  // Trigger invincibility and damage effects
            }
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
}

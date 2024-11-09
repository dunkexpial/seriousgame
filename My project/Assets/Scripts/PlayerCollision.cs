using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private HealthManager healthManager;
    private PlayerMovement playerMovement;
    public bool isInvincible = false;

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyProjectile") || collider.CompareTag("FreezeProjectile"))
        {
            if (!isInvincible)
            {
                if (collider.CompareTag("Enemy"))
                {
                    HealthManager.health--;
                }
                else if (collider.CompareTag("EnemyProjectile") || collider.CompareTag("FreezeProjectile"))
                {
                    HealthManager.health--;

                    DetachAndDestroyParticles(collider.gameObject);

                    if (collider.CompareTag("FreezeProjectile"))
                    {
                        playerMovement.isFrozen = true; // Freeze the player
                        StartCoroutine(UnfreezePlayerAfterDelay(1.0f)); // Unfreeze after delay
                    }

                    StartCoroutine(HandleProjectileHit(collider.gameObject)); // Destroy projectile after detaching particles
                }

                healthManager.ResetRegenTimer();

                if (HealthManager.health <= 0)
                {
                    PlayerManager.GameOver = true;
                    gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(TakeDamage());
                }
            }
        }
    }

    private void DetachAndDestroyParticles(GameObject projectile)
    {
        Transform particleSystemChild = projectile.transform.childCount > 0 ? projectile.transform.GetChild(0) : null;
        if (particleSystemChild != null)
        {
            particleSystemChild.parent = null;
            particleSystemChild.localScale = Vector3.one;
            Destroy(particleSystemChild.gameObject, 2f);
        }
    }

    IEnumerator HandleProjectileHit(GameObject projectile)
    {
        yield return null;
        Destroy(projectile);
    }

    IEnumerator TakeDamage()
    {
        isInvincible = true;
        GetComponent<Animator>().SetLayerWeight(1, 1);

        yield return new WaitForSeconds(2);

        GetComponent<Animator>().SetLayerWeight(1, 0);
        isInvincible = false;
    }

    IEnumerator UnfreezePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.isFrozen = false; // Unfreeze the player
    }
}

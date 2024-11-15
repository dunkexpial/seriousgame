using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private HealthManager healthManager;
    private PlayerMovement playerMovement;
    private Animator animator;
    public bool isInvincible = false;
    public GameObject slowEffectPrefab;
    public GameObject doubleDamageEffectPrefab;

    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyProjectile") || 
            collider.CompareTag("FreezeProjectile") || collider.CompareTag("IceProjectile") || 
            collider.CompareTag("FireProjectile"))
        {
            if (!isInvincible)
            {
                if (collider.CompareTag("Enemy"))
                {
                    HealthManager.health--;
                }
                else if (collider.CompareTag("EnemyProjectile") || collider.CompareTag("FreezeProjectile") ||
                         collider.CompareTag("IceProjectile") || collider.CompareTag("FireProjectile"))
                {
                    HealthManager.health--;

                    DetachAndDestroyParticles(collider.gameObject);

                    if (collider.CompareTag("FreezeProjectile"))
                    {
                        playerMovement.isFrozen = true;
                        StartCoroutine(UnfreezePlayerAfterDelay(1.0f));
                    }

                    HandleSpecialProjectileEffects(collider.gameObject);

                    StartCoroutine(HandleProjectileHit(collider.gameObject));
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

    private void HandleSpecialProjectileEffects(GameObject projectile)
    {
        // Slow effect for IceProjectile
        if (projectile.CompareTag("IceProjectile"))
        {
            playerMovement.moveSpeed = 50f; // Halve the speed
            playerMovement.isSlowed = true;
            GameObject slowEffect = Instantiate(slowEffectPrefab, transform);
            StartCoroutine(RemoveEffectAfterInvincibility(slowEffect));
        }
        // Double damage effect for FireProjectile
        else if (projectile.CompareTag("FireProjectile"))
        {
            GameObject doubleDamageEffect = Instantiate(doubleDamageEffectPrefab, transform);
            StartCoroutine(DoubleDamageSequence());
            StartCoroutine(RemoveEffectAfterInvincibility(doubleDamageEffect));
        }
    }

    private IEnumerator DoubleDamageSequence()
    {
        yield return new WaitForSeconds(1f); // Delay for second damage instance
        HealthManager.health--;
        
        healthManager.ResetRegenTimer();

        if (HealthManager.health <= 0)
        {
            PlayerManager.GameOver = true;
            gameObject.SetActive(false);
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

    private IEnumerator HandleProjectileHit(GameObject projectile)
    {
        yield return null;
        Destroy(projectile);
    }

    private IEnumerator TakeDamage()
    {
        isInvincible = true;
        GetComponent<Animator>().SetLayerWeight(1, 1);

        yield return new WaitForSeconds(2);

        GetComponent<Animator>().SetLayerWeight(1, 0);
        playerMovement.moveSpeed = 100f; // Reset speed to normal
        playerMovement.isSlowed = false;
        isInvincible = false;
    }

    private IEnumerator UnfreezePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.isFrozen = false;
    }

    private IEnumerator RemoveEffectAfterInvincibility(GameObject effect)
    {
        yield return new WaitForSeconds(2); // Invincibility duration
        Destroy(effect);
    }
}

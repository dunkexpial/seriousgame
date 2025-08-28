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

    private GameObject activeSlowEffect;
    private GameObject activeDoubleDamageEffect;
    private SoundManager soundManager;
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        healthManager = FindObjectOfType<HealthManager>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyProjectile") || 
            collider.CompareTag("FreezeProjectile") || collider.CompareTag("IceProjectile") || 
            collider.CompareTag("FireProjectile") && HealthManager.health > 0)
        {
            if (!isInvincible)
            {
                soundManager.PlaySoundBasedOnCollision("PlayerDano");
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
                        soundManager.PlaySoundBasedOnCollision("zaWarudo");
                        playerMovement.isFrozen = true;
                        StartCoroutine(UnfreezePlayerAfterDelay(1.0f));
                    }

                    HandleSpecialProjectileEffects(collider.gameObject);

                    StartCoroutine(HandleProjectileHit(collider.gameObject));
                }

                if (HealthManager.health <= 0)
                {
                    GetComponent<Animator>().SetLayerWeight(1, 0);
                    HandleGameOverCleanup();
                    PlayerManager.GameOver = true;
                    gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(TakeDamage());
                }
            }
        }

        if (collider.CompareTag("Heart") && HealthManager.health < 5)
        {
            HealthManager.health++;
            soundManager.PlayItemHealPickupSound();
            Destroy(collider.gameObject);
        }
    }

    public void EnergyBeamClusterfuck()
    {
        if (!isInvincible)
        {
            HealthManager.health--;
            soundManager.PlaySoundBasedOnCollision("PlayerQueimando");
            activeDoubleDamageEffect = Instantiate(doubleDamageEffectPrefab, transform);
            StartCoroutine(DoubleDamageSequence());
        }
        if (HealthManager.health <= 0)
        {
            GetComponent<Animator>().SetLayerWeight(1, 0);
            HandleGameOverCleanup();
            PlayerManager.GameOver = true;
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(TakeDamage());
        }
    }

    public void IceGroundClusterfuck()
    {
        if (!isInvincible) 
        {
            soundManager.PlaySoundBasedOnCollision("FicaFrioAi");
            playerMovement.moveSpeed = 50f; // Halve the speed
            playerMovement.isSlowed = true;

            // Spawn slow effect if not already active
            if (activeSlowEffect == null)
            {
                activeSlowEffect = Instantiate(slowEffectPrefab, transform);
            }

            // Start expiration
            StartCoroutine(ClearIceGroundSlowAfterDelay(2f)); // adjust duration as needed
        }
    }

    private IEnumerator ClearIceGroundSlowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset only if player isn’t under another effect
        if (!playerMovement.isPowerUpActive) 
        {
            playerMovement.moveSpeed = 100f; // Reset to normal speed
        }

        playerMovement.isSlowed = false;

        if (activeSlowEffect != null)
        {
            Destroy(activeSlowEffect);
            activeSlowEffect = null;
        }
    }

    private void HandleSpecialProjectileEffects(GameObject projectile)
    {
        // Slow effect for IceProjectile
        if (projectile.CompareTag("IceProjectile"))
        {
            soundManager.PlaySoundBasedOnCollision("FicaFrioAi");
            playerMovement.moveSpeed = 50f; // Halve the speed
            playerMovement.isSlowed = true;
            activeSlowEffect = Instantiate(slowEffectPrefab, transform);
        }
        // Double damage effect for FireProjectile
        else if (projectile.CompareTag("FireProjectile"))
        {
            soundManager.PlaySoundBasedOnCollision("PlayerQueimando");
            activeDoubleDamageEffect = Instantiate(doubleDamageEffectPrefab, transform);
            StartCoroutine(DoubleDamageSequence());
        }
    }

    private IEnumerator DoubleDamageSequence()
    {
        yield return new WaitForSeconds(1f); // Delay for second damage instance
        if (HealthManager.health > 0) // Ensure no extra damage is applied after death
        {
            HealthManager.health--;
        }
        if (HealthManager.health <= 0)
        {
            GetComponent<Animator>().SetLayerWeight(1, 0);
            HandleGameOverCleanup();
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

    public IEnumerator TakeDamage()
    {
        isInvincible = true;
        GetComponent<Animator>().SetLayerWeight(1, 1);

        yield return new WaitForSeconds(2f); // Fixed invincibility time

        // Cleanup effects if invincibility ends and the player didn’t die
        CleanupEffects();

        GetComponent<Animator>().SetLayerWeight(1, 0);

        // Only reset speed if the power-up is not active
        if (!playerMovement.isPowerUpActive)
        {
            playerMovement.moveSpeed = 100f; // Reset speed to normal
        }
        playerMovement.isSlowed = false;
        isInvincible = false;
    }

    private IEnumerator UnfreezePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.isFrozen = false;
    }

    private void HandleGameOverCleanup()
    {
        // Reset player movement speed and states
        if (!playerMovement.isPowerUpActive) // Don't reset speed if power-up is active
        {
            playerMovement.moveSpeed = 100f;
        }
        playerMovement.isSlowed = false;
        playerMovement.isFrozen = false;

        // Destroy active effects
        CleanupEffects();
    }

    private void CleanupEffects()
    {
        if (activeSlowEffect != null)
        {
            Destroy(activeSlowEffect);
            activeSlowEffect = null;
        }

        if (activeDoubleDamageEffect != null)
        {
            Destroy(activeDoubleDamageEffect);
            activeDoubleDamageEffect = null;
        }
    }
}

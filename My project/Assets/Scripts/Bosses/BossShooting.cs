using System.Collections;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject[] projectiles;
    public float baseShootingInterval = 1f;
    public float shootingIntervalVariance = 0.5f;
    private float nextShootTime;
    private GameObject player;
    public string ignoreTag = "IgnoreTag";
    public bool checkForTags = true;
    public string shootSound;
    private SoundManager soundManager;
    private float difficulty;
    private float reverseDifficulty;

    private bool isFrozen = false; // Flag to control shooting freeze

    void Start()
    {
        nextShootTime = Time.time + Random.Range(0, baseShootingInterval);
        player = GameObject.FindGameObjectWithTag("Player");
        soundManager = FindAnyObjectByType<SoundManager>();
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");
    }

    void Update()
    {
        if (player == null || isFrozen) return; // Stop shooting if frozen or no player

        if (checkForTags)
        {
            GameObject[] objectsToIgnore = GameObject.FindGameObjectsWithTag(ignoreTag);
            if (objectsToIgnore.Length > 0) return;
        }

        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + ((baseShootingInterval + Random.Range(-shootingIntervalVariance, shootingIntervalVariance)) * Mathf.Pow(reverseDifficulty, 0.5f));
        }
    }

    void Shoot()
    {
        if (player == null) return;

        if (soundManager != null)
        {
            soundManager.PlaySoundBasedOnCollision(shootSound);
        }

        int projectileIndex = Random.Range(0, projectiles.Length);
        GameObject projectilePrefab = projectiles[projectileIndex];
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            enemyProjectile.player = player;
            Vector3 direction = (enemyProjectile.player.transform.position - transform.position).normalized;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = direction * enemyProjectile.speed;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // Method to freeze/unfreeze shooting
    public void FreezeShooting(bool freeze)
    {
        isFrozen = freeze;
    }
}

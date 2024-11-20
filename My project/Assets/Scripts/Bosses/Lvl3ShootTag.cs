using System.Collections;
using UnityEngine;

public class Lvl3ShootTag : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public float minShootDelay = 1f;
    public float maxShootDelay = 3f;
    public string targetTag = "Target";
    public float projectileSpeed = 10f;

    [Header("Spawn Settings")]
    public GameObject spawnPrefab;

    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            // Wait for a random delay
            float delay = Random.Range(minShootDelay, maxShootDelay);
            yield return new WaitForSeconds(delay);

            // Find a random target with the specified tag
            GameObject target = GetRandomTargetWithTag(targetTag);

            if (target != null)
            {
                // Shoot a projectile at the target
                ShootAtTarget(target);
            }
        }
    }

    private GameObject GetRandomTargetWithTag(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

        if (targets.Length == 0) return null;

        // Choose a random target from the list
        int randomIndex = Random.Range(0, targets.Length);
        return targets[randomIndex];
    }

    private void ShootAtTarget(GameObject target)
    {
        if (projectilePrefab == null || target == null) return;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate the direction to the target (2D)
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // Assign the direction and speed to the projectile
        Lvl3SpawnProjectile projectileScript = projectile.GetComponent<Lvl3SpawnProjectile>();
        if (projectileScript != null)
        {
            projectileScript.direction = direction;
            projectileScript.speed = projectileSpeed;
            projectileScript.targetTag = targetTag;
            projectileScript.spawnPrefab = spawnPrefab;
        }
    }
}

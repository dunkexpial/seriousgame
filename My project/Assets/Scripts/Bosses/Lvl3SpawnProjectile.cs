using UnityEngine;

public class Lvl3SpawnProjectile : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;
    public Vector2 direction;  // Use Vector2 for 2D movement

    [Header("Collision Settings")]
    public string targetTag = "Target";
    public GameObject spawnPrefab;

    private void Update()
    {
        // Move the projectile in the assigned direction (2D)
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag(targetTag))
        {

            // Spawn the prefab at the collision point
            if (spawnPrefab != null)
            {
                Instantiate(spawnPrefab, transform.position, Quaternion.identity);
            }

            // Destroy the projectile
            DetachParticles();
            Destroy(gameObject); // Destroy on obstacle hit
        }
    }

    private void DetachParticles()
    {
        Transform particleSystemChild = transform.childCount > 0 ? transform.GetChild(0) : null;
        if (particleSystemChild != null)
        {
            particleSystemChild.parent = null;
            particleSystemChild.localScale = Vector3.one;
            Destroy(particleSystemChild.gameObject, 2f);
        }
    }
}

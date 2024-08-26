using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] projectilePrefabs; // Array pra guardar os prefabs
    public Transform shootingPoint;
    public float projectileSpeed;
    private playermovement playerMovement;
    private int selectedProjectileIndex = 0; // Index pra guardar projétil atual

    void Start()
    {
        playerMovement = FindObjectOfType<playermovement>();
    }

    public void SetProjectileType(int index)
    {
        if (index >= 0 && index < projectilePrefabs.Length)
        {
            selectedProjectileIndex = index;
        }
    }

    public void Shoot()
    {
        Vector2 direction = playerMovement.GetMoveDirection();
        if (direction == Vector2.zero)
        {
            direction = playerMovement.GetLastDirection();
        }

        if (direction != Vector2.zero)
        {
            GameObject projectile = Instantiate(projectilePrefabs[selectedProjectileIndex], shootingPoint.position, Quaternion.identity);
            BaseProjectile projectileScript = projectile.GetComponent<BaseProjectile>();
            if (projectileScript != null)
            {
                projectileScript.direction = direction.normalized;
                projectileScript.speed = projectileSpeed;
            }
        }
    }
}

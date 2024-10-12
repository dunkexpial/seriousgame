using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] projectilePrefabs; // Array dos prefabs dos projéteis
    public Transform shootingPoint;
    public float projectileSpeed;
    public GameObject player;
    private playermovement playerMovement;
    private int selectedProjectileIndex = 0;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<playermovement>().gameObject;
        }
        playerMovement = player.GetComponent<playermovement>();
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
        if (Time.timeScale == 0)
        {
            return; // Check if the game is paused, if it is return nothing = stop shooting 
        }

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Calculate the direction from the shooting point to the mouse position
        Vector2 direction = (mousePosition - shootingPoint.position).normalized;

        if (direction != Vector2.zero)
        {
            // Instantiate and shoot the projectile in the direction of the cursor
            GameObject projectile = Instantiate(projectilePrefabs[selectedProjectileIndex], shootingPoint.position, Quaternion.identity);
            BaseProjectile projectileScript = projectile.GetComponent<BaseProjectile>();
            if (projectileScript != null)
            {
                projectileScript.direction = direction;
                projectileScript.speed = projectileSpeed;
                projectileScript.Initialize(player);
            }
        }
    }
}
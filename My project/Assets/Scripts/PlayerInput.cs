using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public ProjectileManager projectileManager;
    // The 3 following variables could be either configurable in unity or a fixed value here, I'll see to that later in the game's development
    public Animator animator;
    private float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Update()
    {
        // Handle projectile type selection with number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            projectileManager.SetProjectileType(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            projectileManager.SetProjectileType(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            projectileManager.SetProjectileType(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            projectileManager.SetProjectileType(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            projectileManager.SetProjectileType(4);
        }

        if (projectileManager.selectedProjectileIndex  == 0)
        {
            fireRate = 0.5f;
        }
        else if (projectileManager.selectedProjectileIndex == 1)
        {
            fireRate = 0.2f;
        }
        else if (projectileManager.selectedProjectileIndex == 2)
        {
            fireRate = 1f;
        }
        else if (projectileManager.selectedProjectileIndex == 3)
        {
            fireRate = 0.7f;
        }
        else if (projectileManager.selectedProjectileIndex == 4)
        {
            fireRate = 1.2f;
        }


        // Check if the fire button is held down and if enough time has passed since the last shot
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime) 
        {
            projectileManager.Shoot();
            
            // Trigger the shooting animation
            animator.SetTrigger("Shoot"); // This has to be the name of the trigger parameter in the animator!!
            
            // Update the next fire time
            nextFireTime = Time.time + fireRate;
        }
    }
}

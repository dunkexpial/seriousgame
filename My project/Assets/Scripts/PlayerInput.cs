using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public ProjectileManager projectileManager;

    // The 3 following variables could be either configurable in unity or a fixed value here, I'll see to that later in the game's development
    public Animator animator;
    public float fireRate;
    private float nextFireTime = 0f;

    void Update()
    {
        // I have spent way too much time trying to make the fire rate depend on
        // the currently selected weapon using 3 different methods across 4 different scripts but I just 
        // couldn't manage it today even after hours so this will have to do (for now at least)

        // Check if the fire button is held down and if enough time has passed since the last shot
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime) 
        {
            projectileManager.Shoot();
            
            // Trigger the shooting animation
            animator.SetTrigger("Shoot"); // This has to be the name of the trigger parameter in the animator!!
            
            // Update the next fire time
            nextFireTime = Time.time + fireRate;
        }

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
    }
}

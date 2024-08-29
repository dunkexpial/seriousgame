using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public ProjectileManager projectileManager;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Replace "Fire1" with your input action
        {
            projectileManager.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Select first projectile type
        {
            projectileManager.SetProjectileType(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Select second projectile type
        {
            projectileManager.SetProjectileType(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Select third projectile type
        {
            projectileManager.SetProjectileType(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) // Select fourth projectile type
        {
            projectileManager.SetProjectileType(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Select fifth projectile type
        {
            projectileManager.SetProjectileType(4);
        }
    }
}

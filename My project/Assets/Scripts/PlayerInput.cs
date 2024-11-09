using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public ProjectileManager projectileManager;
    public Animator animator;
    private PlayerMovement playerMovement;
    private float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public DialogueUI DialogueUI => dialogueUI;

    void Start()
    {
        // Get the PlayerMovement component to check if the player is frozen
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (dialogueUI.isOpen) return;

        // Prevent shooting if the player is frozen
        if (playerMovement != null && playerMovement.isFrozen) return;

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

        // Set the fire rate based on the selected projectile type
        switch (projectileManager.selectedProjectileIndex)
        {
            case 0:
                fireRate = 0.35f;
                break;
            case 1:
                fireRate = 0.2f;
                break;
            case 2:
                fireRate = 1f;
                break;
            case 3:
                fireRate = 0.7f;
                break;
            case 4:
                fireRate = 1.2f;
                break;
        }

        // Check if the fire button is held down and if enough time has passed since the last shot
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            projectileManager.Shoot();

            // Trigger the shooting animation
            animator.SetTrigger("Shoot");

            // Update the next fire time
            nextFireTime = Time.time + fireRate;
        }
    }
}

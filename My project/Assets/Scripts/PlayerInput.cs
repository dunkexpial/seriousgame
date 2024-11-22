using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public ProjectileManager projectileManager;
    public Animator animator;
    private PlayerMovement playerMovement;

    private float[] fireRates = { 0.4f, 0.2f, 1.0f, 0.2f, 1.2f }; // Fire rates for each projectile type
    private float nextFireTime; // Shared timer for all projectile types

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

        HandleProjectileSelection();
        HandleShooting();
    }

    private void HandleProjectileSelection()
    {
        // Detect projectile selection input and set the selected projectile
        for (int i = 0; i < fireRates.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                projectileManager.SetProjectileType(i);
                break;
            }
        }
    }

    private void HandleShooting()
    {
        int currentProjectileIndex = projectileManager.selectedProjectileIndex;

        // Check if the fire button is held down and if enough time has passed since the last shot
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            projectileManager.Shoot();

            // Trigger the shooting animation
            animator.SetTrigger("Shoot");

            // Update the shared next fire time based on the current projectile's fire rate
            nextFireTime = Time.time + fireRates[currentProjectileIndex];
        }
    }
}

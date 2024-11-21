using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public ProjectileManager projectileManager;
    public Animator animator;
    private PlayerMovement playerMovement;

    private float[] fireRates = { 0.4f, 0.2f, 1.0f, 0.3f, 1.2f }; // Fire rates for each projectile type
    private float[] nextFireTimes; // Timers for each projectile type

    public DialogueUI DialogueUI => dialogueUI;

    void Start()
    {
        // Initialize the nextFireTimes array with the same length as the fireRates array
        nextFireTimes = new float[fireRates.Length];

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

        // Check if the fire button is held down and if enough time has passed since the last shot for the selected projectile
        if (Input.GetButton("Fire1") && Time.time >= nextFireTimes[currentProjectileIndex])
        {
            projectileManager.Shoot();

            // Trigger the shooting animation
            animator.SetTrigger("Shoot");

            // Update the next fire time for the selected projectile
            nextFireTimes[currentProjectileIndex] = Time.time + fireRates[currentProjectileIndex];
        }
    }
}

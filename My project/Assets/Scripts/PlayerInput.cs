using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public ProjectileManager projectileManager;
    public Animator animator;
    private PlayerMovement playerMovement;

    private float[] fireRates = { 0.4f, 0.2f, 1.0f, 0.2f, 1.2f }; // Fire rates for each projectile type
    private float nextFireTime; // Shared timer for all projectile types

    [Header("Audio Settings")]
    public AudioClip[] shootClips; // Array para os sons de tiro
    private AudioSource audioSource;

    public DialogueUI DialogueUI => dialogueUI;
    
    void Start()
    {
        // Get the PlayerMovement component to check if the player is frozen
        playerMovement = GetComponent<PlayerMovement>();

        // Inicializa o AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource não encontrado no objeto Player!");
        }
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
        if (PauseMenu.isPaused) return;
        int currentProjectileIndex = projectileManager.selectedProjectileIndex;

        // Check if the fire button is held down and if enough time has passed since the last shot
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            projectileManager.Shoot();

            // Trigger the shooting animation
            animator.SetTrigger("Shoot");

            // Randomiza e toca um som de tiro
            PlayRandomShootSound();

            // Update the shared next fire time based on the current projectile's fire rate
            nextFireTime = Time.time + fireRates[currentProjectileIndex];
        }
    }

   private void PlayRandomShootSound()
{
    if (audioSource == null)
    {
        Debug.LogError("AudioSource não encontrado!");
        return; // o AudioSource não está configurado
    }

    if (shootClips == null || shootClips.Length == 0)
    {
        Debug.LogError("O array shootClips está vazio ou não foi configurado.");
        return; // não há clipes no array
    }

    // Escolhe um som aleatório dentro do array
    int randomIndex = Random.Range(0, shootClips.Length);
    Debug.Log($"Reproduzindo som {randomIndex}: {shootClips[randomIndex].name}");
    audioSource.PlayOneShot(shootClips[randomIndex]); // Toca o som escolhido
}
}

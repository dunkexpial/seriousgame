using UnityEngine;
using TMPro; // for TextMeshProUGUI
using UnityEngine.UI; // if you want to change color

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public ProjectileManager projectileManager;
    public Animator animator;
    private PlayerMovement playerMovement;

    private float[] fireRates = { 0.4f, 0.2f, 1.0f, 0.2f, 0.3f }; 
    private float nextFireTime; 

    [Header("Ammo Settings")]
    public int[] defaultAmmoPerType = { 30, 60, 15, 40, 50 };
    public int[] maxAmmoPerType = { 90, 180, 45, 120, 150 }; // maximum ammo for each projectile type
    public int[] ammoCounts;

    [Header("Ammo UI (auto detected)")]
    private TextMeshProUGUI ammoText;

    [Header("Flash Settings")]
    public Color normalColor = Color.white;
    public Color emptyColor = Color.red;
    public float flashDuration = 0.2f; // how fast the flash occurs
    private float flashTimer;
    private bool isFlashing;

    [Header("Audio Settings")]
    public AudioClip[] shootClips; 
    private AudioSource audioSource;

    private float shootEnableTime; // Time after which shooting is allowed

    public DialogueUI DialogueUI => dialogueUI;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource não encontrado no objeto Player!");
        }

        // initialize ammo
        ammoCounts = new int[fireRates.Length];
        for (int i = 0; i < ammoCounts.Length; i++)
        {
            if (i < defaultAmmoPerType.Length)
                ammoCounts[i] = Mathf.Min(defaultAmmoPerType[i], (i < maxAmmoPerType.Length ? maxAmmoPerType[i] : int.MaxValue));
            else
                ammoCounts[i] = 0;
        }

        // auto find the UI text
        GameObject ammoObj = GameObject.Find("AmmoText_Projectile");
        if (ammoObj != null)
        {
            ammoText = ammoObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("AmmoText_Projectile object not found in scene!");
        }

        UpdateAmmoUI();

        shootEnableTime = Time.unscaledTime + 2f;
    }

    void Update()
    {
        if (dialogueUI.isOpen) return;
        if (playerMovement != null && playerMovement.isFrozen) return;

        HandleProjectileSelection();
        HandleShooting();

        // handle flashing when out of ammo
        if (isFlashing)
        {
            flashTimer += Time.unscaledDeltaTime;
            float t = Mathf.PingPong(flashTimer * 5f, 1f); 
            ammoText.color = Color.Lerp(normalColor, emptyColor, t);

            if (ammoCounts[projectileManager.selectedProjectileIndex] > 0)
            {
                isFlashing = false;
                ammoText.color = normalColor;
            }
        }
    }

    private void HandleProjectileSelection()
    {
        for (int i = 0; i < fireRates.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                projectileManager.SetProjectileType(i);
                UpdateAmmoUI();
                break;
            }
        }
    }

    private void HandleShooting()
    {
        if (PauseMenu.isPaused) return;

        // Only allow shooting after 2 seconds since instantiation
        if (Time.unscaledTime < shootEnableTime)
            return;

        int maxProjectiles = projectileManager.projectilePrefabs.Length;
        int currentProjectileIndex = projectileManager.selectedProjectileIndex;

        // Clamp index to available projectiles
        if (currentProjectileIndex >= maxProjectiles)
            currentProjectileIndex = maxProjectiles - 1;

        // Check if current projectile has ammo
        if (ammoCounts[currentProjectileIndex] <= 0)
        {
            Debug.Log("Out of ammo for projectile " + currentProjectileIndex);
            StartFlashing();

            // Try to switch to the next available projectile (only among available projectiles)
            int startIndex = currentProjectileIndex;
            bool found = false;
            do
            {
                currentProjectileIndex = (currentProjectileIndex + 1) % maxProjectiles;
                if (ammoCounts[currentProjectileIndex] > 0)
                {
                    projectileManager.SetProjectileType(currentProjectileIndex);
                    UpdateAmmoUI();
                    found = true;
                    break;
                }
            } while (currentProjectileIndex != startIndex);

            // If no projectile has ammo, just return (do not shoot)
            if (!found)
                return;
        }

        // Fire projectile if button is pressed and cooldown passed
        if (Input.GetButton("Fire1") && Time.unscaledTime >= nextFireTime)
        {
            projectileManager.Shoot();
            ammoCounts[currentProjectileIndex]--;

            animator.SetTrigger("Shoot");
            PlayRandomShootSound();

            // Use the correct fire rate for the current projectile index (within available projectiles)
            nextFireTime = Time.unscaledTime + fireRates[currentProjectileIndex];

            UpdateAmmoUI();
        }
    }

    private void PlayRandomShootSound()
    {
        if (audioSource == null) return;
        if (shootClips == null || shootClips.Length == 0) return;

        int randomIndex = Random.Range(0, shootClips.Length);
        audioSource.PlayOneShot(shootClips[randomIndex]); 
    }

    public void GiveAmmo(int projectileIndex, int minAmount, int maxAmount)
    {
        int amount = Random.Range(minAmount, maxAmount + 1);
        if (projectileIndex < ammoCounts.Length)
        {
            ammoCounts[projectileIndex] += amount;

            // Clamp to max ammo
            if (projectileIndex < maxAmmoPerType.Length)
                ammoCounts[projectileIndex] = Mathf.Min(ammoCounts[projectileIndex], maxAmmoPerType[projectileIndex]);

            Debug.Log($"Picked up {amount} ammo for projectile {projectileIndex}. Total: {ammoCounts[projectileIndex]}");
            UpdateAmmoUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AmmoPickup pickup = collision.GetComponent<AmmoPickup>();
        if (pickup != null)
        {
            GiveAmmo(pickup.projectileTypeIndex, pickup.minAmount, pickup.maxAmount);
            Destroy(collision.gameObject);
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText == null) return;

        int currentProjectileIndex = projectileManager.selectedProjectileIndex;
        ammoText.text = ammoCounts[currentProjectileIndex].ToString();

        if (ammoCounts[currentProjectileIndex] <= 0)
        {
            StartFlashing();
        }
        else
        {
            ammoText.color = normalColor;
            isFlashing = false;
        }
    }

    private void StartFlashing()
    {
        if (ammoText == null) return;
        isFlashing = true;
        flashTimer = 0f;
    }
}

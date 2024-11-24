using UnityEngine;
using UnityEngine.UI; // For handling the UI Image component

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private Image selectedProjectileThumbnail; // Reference to the UI Image for displaying the thumbnail
    public GameObject[] projectilePrefabs; // Array of projectile prefabs
    public Transform shootingPoint;
    public float projectileSpeed;
    public GameObject player;
    private PlayerMovement playerMovement;
    public int selectedProjectileIndex = 0;
    public DialogueUI DialogueUI => dialogueUI;

    // Animator component reference
    private Animator animator;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }
        playerMovement = player.GetComponent<PlayerMovement>();
        
        // Get the Animator component
        animator = player.GetComponent<Animator>();

        // Set the initial thumbnail on start
        UpdateProjectileThumbnail();
    }

    public void SetProjectileType(int index)
    {
        if (index >= 0 && index < projectilePrefabs.Length)
        {
            selectedProjectileIndex = index;
            UpdateProjectileThumbnail(); // Update the thumbnail when the projectile type changes
        }
    }

    public void Shoot()
    {
        if (dialogueUI.isOpen) return;
        
        if (Time.timeScale == 0)
        {
            return; // Check if the game is paused, if it is return nothing = stop shooting 
        }

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Calculate the direction from the shooting point to the mouse position
        Vector2 direction = (mousePosition - shootingPoint.position).normalized;

        animator.SetFloat("MouseX", direction.x);
        animator.SetFloat("MouseY", direction.y);

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

    // Method to update the thumbnail UI
    private void UpdateProjectileThumbnail()
    {
        if (selectedProjectileThumbnail != null && projectilePrefabs.Length > selectedProjectileIndex)
        {
            // Assuming each projectile prefab has a SpriteRenderer component with a sprite
            Sprite projectileSprite = projectilePrefabs[selectedProjectileIndex].GetComponent<SpriteRenderer>().sprite;
            
            // Set the sprite of the UI Image to the current projectile's sprite
            selectedProjectileThumbnail.sprite = projectileSprite;
        }
    }
}


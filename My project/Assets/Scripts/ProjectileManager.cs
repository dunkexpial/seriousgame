using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public GameObject[] projectilePrefabs; // Array dos prefabs dos projéteis
    public Transform shootingPoint;
    public float projectileSpeed;
    public GameObject player;
    private playermovement playerMovement;
    public int selectedProjectileIndex = 0;
    public DialogueUI DialogueUI => dialogueUI;

    // Animator component reference
    private Animator animator;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<playermovement>().gameObject;
        }
        playerMovement = player.GetComponent<playermovement>();
        
        // Get the Animator component
        animator = player.GetComponent<Animator>();
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
}

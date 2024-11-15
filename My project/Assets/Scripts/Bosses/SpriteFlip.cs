using UnityEngine;

public class FlipAnimator : MonoBehaviour
{
    public string playerTag = "Player";  // The tag for the player object
    private Transform player;            // Reference to the player's transform
    public Animator animator;            // Reference to the Animator component
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    void Start()
    {
        // Find the player object by tag and get its Transform component
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player with tag '" + playerTag + "' not found!");
        }
    }

    void Update()
    {
        if (player == null) return;  // Ensure the player is assigned

        // Check if the player is to the right or left of this object
        if (player.position.x > transform.position.x)
        {
            // Flip the animator and sprite renderer horizontally to face right
            if (animator != null)
                animator.transform.localScale = new Vector3(1, 1, 1); // Facing right
            if (spriteRenderer != null)
                spriteRenderer.flipX = false; // Facing right
        }
        else if (player.position.x < transform.position.x)
        {
            // Flip the animator and sprite renderer horizontally to face left
            if (animator != null)
                animator.transform.localScale = new Vector3(-1, 1, 1); // Facing left
            if (spriteRenderer != null)
                spriteRenderer.flipX = true; // Facing left
        }
    }
}

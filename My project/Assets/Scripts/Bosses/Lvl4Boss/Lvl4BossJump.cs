using System.Collections;
using UnityEngine;

public class BossJumpSlam : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpHeight = 10f;           // Height of the boss's jump
    public float upwardDuration = 1f;       // Duration of the upward motion
    public float downwardDuration = 0.2f;   // Duration of the downward motion
    public float shadowMinScale = 0.5f;     // Minimum scale of the shadow when fully "up"
    public float shadowMaxScale = 1f;       // Maximum scale of the shadow when "on ground"
    public float shadowMinAlpha = 0.3f;     // Minimum alpha of the shadow when fully "up"
    public float shadowMaxAlpha = 1f;       // Maximum alpha of the shadow when "on ground"

    [Header("Player Targeting")]
    public string playerTag = "Player";     // Tag used to find the player
    public Vector3 slamPositionOffset;      // Offset applied to the target position
    public float followSpeed = 200f;          // How fast the boss follows the player
    public Vector3 followOffset = new Vector3(0, -10, 0); // Default offset X: 0, Y: -10, Z: 0


    private Transform visual;
    private Transform shadow;
    private Collider2D bossCollider;
    private SpriteRenderer shadowRenderer;
    private Transform player;
    private Vector3 visualStartPos;
    private Vector3 shadowStartScale;
    public bool isJumping = false;
    private Vector3 targetSlamPosition;

    private void Awake()
    {
        // Automatically find Visual and Shadow children by name
        visual = transform.Find("Visual");
        shadow = transform.Find("Shadow");

        if (visual == null)
        {
            Debug.LogError("No child named 'Visual' found! Please ensure the Visual child is named correctly.");
            return;
        }

        if (shadow == null)
        {
            Debug.LogError("No child named 'Shadow' found! Please ensure the Shadow child is named correctly.");
            return;
        }

        // Automatically find the boss's collider
        bossCollider = GetComponent<Collider2D>();
        if (bossCollider == null)
        {
            Debug.LogError("No Collider2D component found on this GameObject! Please ensure the boss has a Collider2D.");
            return;
        }

        // Automatically find the Shadow's SpriteRenderer
        shadowRenderer = shadow.GetComponent<SpriteRenderer>();
        if (shadowRenderer == null)
        {
            Debug.LogError("No SpriteRenderer component found on the 'Shadow' child! Please ensure the Shadow child has a SpriteRenderer.");
            return;
        }
    }

    private void Start()
    {
        // Find the player by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject found with tag: " + playerTag);
            return;
        }

        // Save initial positions and scale
        visualStartPos = visual.localPosition;
        shadowStartScale = shadow.localScale;

        // Start the jump slam automatically
        StartJumpSlam();
    }

    public void StartJumpSlam()
    {
        if (!isJumping && player != null)
        {
            StartCoroutine(JumpSlamRoutine());
        }
    }

    private IEnumerator JumpSlamRoutine()
    {
        isJumping = true;
        bossCollider.enabled = false;

        // Jump Up
        float timer = 0f;
        while (timer < upwardDuration)
        {
            timer += Time.deltaTime;
            float t = timer / upwardDuration;

            // Follow the player's position while going up (horizontal and vertical)
            if (player != null)
            {
                targetSlamPosition = player.position + slamPositionOffset;
                // Smoothly move towards the player's position with offset, limiting speed
                Vector3 targetPosition = new Vector3(targetSlamPosition.x + followOffset.x, targetSlamPosition.y + followOffset.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }

            // Move visual upwards
            visual.localPosition = Vector3.Lerp(visualStartPos, visualStartPos + Vector3.up * jumpHeight, t);

            // Shrink shadow and reduce alpha
            float scale = Mathf.Lerp(shadowMaxScale, shadowMinScale, t);
            shadow.localScale = shadowStartScale * scale;

            float alpha = Mathf.Lerp(shadowMaxAlpha, shadowMinAlpha, t);
            shadowRenderer.color = new Color(1f, 1f, 1f, alpha);

            yield return null;
        }

        // Lock position for downward motion
        visual.localPosition = visualStartPos + Vector3.up * jumpHeight;

        // Slam Down
        timer = 0f;
        while (timer < downwardDuration)
        {
            timer += Time.deltaTime;
            float t = timer / downwardDuration;

            // Move visual downwards to the locked position
            visual.localPosition = Vector3.Lerp(visualStartPos + Vector3.up * jumpHeight, visualStartPos, t);

            // Grow shadow and increase alpha
            float scale = Mathf.Lerp(shadowMinScale, shadowMaxScale, t);
            shadow.localScale = shadowStartScale * scale;

            float alpha = Mathf.Lerp(shadowMinAlpha, shadowMaxAlpha, t);
            shadowRenderer.color = new Color(1f, 1f, 1f, alpha);

            yield return null;
        }

        // Reset and re-enable collider
        visual.localPosition = visualStartPos;
        shadow.localScale = shadowStartScale;
        shadowRenderer.color = new Color(1f, 1f, 1f, shadowMaxAlpha);
        bossCollider.enabled = true;
        yield return new WaitForSeconds(1);
        isJumping = false;
    }
}

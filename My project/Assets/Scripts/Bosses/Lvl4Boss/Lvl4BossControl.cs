using UnityEngine;
using System.Collections;

public class Lvl4BossControl : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player
    public float detectionDistance = 5f; // Distance within which the boss changes behavior
    public float cooldownDuration = 1f; // Cooldown time before returning to Phase 1 movement

    private GameObject player; // Cached reference to the player
    private Lvl4BossPhase1Movement phase1Movement;
    private BossJumpSlam jumpSlam;
    private bool isCooldownActive = false; // Tracks if cooldown is in progress

    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag(playerTag);

        if (player == null)
        {
            Debug.LogError($"No GameObject found with tag '{playerTag}'. Ensure the player has the correct tag.");
            return;
        }

        // Add Lvl4BossPhase1Movement script at the start
        phase1Movement = gameObject.AddComponent<Lvl4BossPhase1Movement>();
    }

    void Update()
    {
        // Ensure player is detected
        if (player == null) return;

        // Skip detection and state transition if already jumping or during cooldown
        if ((jumpSlam != null && jumpSlam.isJumping) || isCooldownActive) return;

        // Check the distance to the player every frame
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionDistance)
        {
            ActivateJumpSlam();
        }
        else
        {
            ActivatePhase1Movement();
        }
    }

    private void ActivateJumpSlam()
    {
        // Remove the existing Jump Slam script (if any) and add a fresh one
        if (jumpSlam != null)
        {
            Destroy(jumpSlam); // Remove the existing Jump Slam script
        }

        // Add a new Jump Slam script to the boss
        jumpSlam = gameObject.AddComponent<BossJumpSlam>();

        // Also, remove the Phase 1 Movement script
        if (phase1Movement != null)
        {
            Destroy(phase1Movement);
        }
    }

    private void ActivatePhase1Movement()
    {
        // Begin cooldown if not already active
        if (!isCooldownActive && jumpSlam != null)
        {
            StartCoroutine(HandleCooldown());
        }
    }

    private IEnumerator HandleCooldown()
    {
        // Set cooldown as active
        isCooldownActive = true;

        // Remove the Jump Slam script
        if (jumpSlam != null)
        {
            Destroy(jumpSlam); // Remove Jump Slam script
        }

        // Wait for cooldown duration
        yield return new WaitForSeconds(cooldownDuration);

        // After cooldown, check if player is still in detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionDistance)
        {
            // If the player is still in range, immediately add a new Jump Slam script
            ActivateJumpSlam();
        }
        else
        {
            // If the player is out of range, re-enable Phase 1 Movement
            phase1Movement = gameObject.AddComponent<Lvl4BossPhase1Movement>();
            isCooldownActive = false;
        }
    }
}

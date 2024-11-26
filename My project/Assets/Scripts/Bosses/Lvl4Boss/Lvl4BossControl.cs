using UnityEngine;
using System.Collections;

public class Lvl4BossControl : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player
    public float detectionDistance = 125f; // Distance within which the boss changes behavior
    private GameObject player; // Cached reference to the player
    private Lvl4BossPhase1Movement phase1Movement;
    private BossJumpSlam jumpSlam;

    private float lastJumpSlamTime = -Mathf.Infinity; // Tracks the time of the last jump slam
    private float jumpSlamCooldown = 5f; // Cooldown duration in seconds

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

        // Check the distance to the player every frame
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Handle Jump Slam cooldown and transitions
        if (jumpSlam != null && jumpSlam.isJumping)
        {
            // Wait until the jump slam completes
            return;
        }
        else if (jumpSlam != null && !jumpSlam.isJumping)
        {
            // Jump slam completed, transition back to Phase 1 Movement
            ActivatePhase1Movement();
        }

        // Handle behavior based on player distance
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
        // Check if enough time has passed since the last jump slam
        if (Time.time - lastJumpSlamTime < jumpSlamCooldown) return;

        // Remove the existing Jump Slam script (if any) and add a fresh one
        if (jumpSlam != null)
        {
            Destroy(jumpSlam); // Remove the existing Jump Slam script
        }

        // Add a new Jump Slam script to the boss
        jumpSlam = gameObject.AddComponent<BossJumpSlam>();

        // Update the last jump slam time
        lastJumpSlamTime = Time.time;

        // Also, remove the Phase 1 Movement script
        if (phase1Movement != null)
        {
            Destroy(phase1Movement);
        }
    }

    private void ActivatePhase1Movement()
    {
        // Ensure Phase 1 Movement script is active
        if (phase1Movement == null)
        {
            phase1Movement = gameObject.AddComponent<Lvl4BossPhase1Movement>();
        }

        // Remove the Jump Slam script (if any)
        if (jumpSlam != null)
        {
            Destroy(jumpSlam);
        }
    }
}

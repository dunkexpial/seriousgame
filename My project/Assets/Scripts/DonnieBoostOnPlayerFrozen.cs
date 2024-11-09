using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoostOnPlayerFrozen : MonoBehaviour
{
    private BasicRangedAI aiScript;           // Reference to the BasicRangedAI script
    private PlayerMovement playerMovement;    // Reference to the playerMovement script
    public GameObject frozenEffectPrefab;     // Prefab to spawn when player is frozen
    private GameObject spawnedEffect;         // Reference to the spawned effect object
    private float originalSpeed;
    private float originalAnimSpeed;

    void Start()
    {
        // Automatically find the BasicRangedAI component on the same GameObject
        aiScript = GetComponent<BasicRangedAI>();
        
        if (aiScript != null)
        {
            // Save the original speed and animation speed from the AI script
            originalSpeed = aiScript.speed;
            originalAnimSpeed = aiScript.animator.speed;
        }

        // Find the player and automatically get the playerMovement component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // Check if both components are assigned and if the player is frozen
        if (playerMovement != null && aiScript != null)
        {
            if (playerMovement.isFrozen)
            {
                // Double the enemy's speed and animation speed
                aiScript.speed = originalSpeed * 2f;
                aiScript.animator.speed = originalAnimSpeed * 2f;

                // Spawn the effect prefab if it hasn't been spawned already
                if (spawnedEffect == null && frozenEffectPrefab != null)
                {
                    spawnedEffect = Instantiate(frozenEffectPrefab, transform.position, Quaternion.identity, transform);
                }
            }
            else
            {
                // Reset to original speed and animation speed if the player is not frozen
                aiScript.speed = originalSpeed;
                aiScript.animator.speed = originalAnimSpeed;

                // Destroy the effect prefab if it is currently active
                if (spawnedEffect != null)
                {
                    Destroy(spawnedEffect);
                    spawnedEffect = null;
                }
            }
        }
    }
}

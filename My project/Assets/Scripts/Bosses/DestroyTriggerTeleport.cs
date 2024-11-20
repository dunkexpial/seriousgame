using UnityEngine;

public class DestroyTriggerTeleport : MonoBehaviour
{
    private Lvl3BossLogic bossLogic; // Reference to the Lvl3BossLogic script

    private void Awake()
    {
        // Automatically find the Lvl3BossLogic script in the scene
        bossLogic = FindObjectOfType<Lvl3BossLogic>();

        if (bossLogic == null)
        {
            Debug.LogError("Lvl3BossLogic script not found in the scene! Teleport will not work.");
        }
    }

    private void OnDestroy()
    {
        if (bossLogic != null)
        {
            bossLogic.Teleport();
        }
    }
}

using UnityEngine;
using System.Collections;

public class Lvl4BossPhase2Handler : MonoBehaviour
{
    private AttributesManager attributesManager; // Reference to the AttributesManager

    [SerializeField]
    private float phase1Duration = 10f; // Time spent in Phase 1

    [SerializeField]
    private float phase2Duration = 5f; // Time spent in Phase 2

    private Coroutine phaseCoroutine;

    void Start()
    {
        // Try to get the AttributesManager component from the same GameObject this script is attached to
        attributesManager = GetComponent<AttributesManager>();

        if (attributesManager == null)
        {
            Debug.LogError("AttributesManager not found on this GameObject!");
        }
        else
        {
            Debug.Log("AttributesManager found on the boss object.");
        }

        // Start the phase cycle
        phaseCoroutine = StartCoroutine(PhaseCycle());
    }

    private IEnumerator PhaseCycle()
    {
        while (true) // Infinite loop to repeat phases
        {
            // Phase 1
            Debug.Log("Entering Phase 1");
            AddPhase1Scripts();
            RemovePhase2Scripts();
            yield return new WaitForSeconds(phase1Duration); // Wait for Phase 1 duration

            // Phase 2
            Debug.Log("Entering Phase 2");
            RemovePhase1Scripts();
            AddPhase2Scripts();
            yield return new WaitForSeconds(phase2Duration); // Wait for Phase 2 duration
        }
    }

    private void AddPhase1Scripts()
    {
        if (GetComponent<Lvl4BossControl>() == null)
        {
            Debug.Log("Adding Lvl4BossControl script");
            gameObject.AddComponent<Lvl4BossControl>();
        }
    }

    private void RemovePhase1Scripts()
    {
        var lvl4BossControl = GetComponent<Lvl4BossControl>();
        if (lvl4BossControl != null)
        {
            Debug.Log("Removing Lvl4BossControl script");
            Destroy(lvl4BossControl);
        }

        var lvl4BossPhase1Movement = GetComponent<Lvl4BossPhase1Movement>();
        if (lvl4BossPhase1Movement != null)
        {
            Debug.Log("Removing Lvl4BossPhase1Movement script");
            Destroy(lvl4BossPhase1Movement);
        }
        var BossJumpSlam = GetComponent<BossJumpSlam>();
        if (BossJumpSlam != null)
        {
            Debug.Log("Removing Lvl4BossPhase1Movement script");
            Destroy(BossJumpSlam);
        }
    }

    private void AddPhase2Scripts()
    {
        if (GetComponent<Lvl4BossPhase2Movement>() == null)
        {
            Debug.Log("Adding Lvl4BossPhase2Movement script");
            gameObject.AddComponent<Lvl4BossPhase2Movement>();
        }
    }

    private void RemovePhase2Scripts()
    {
        var lvl4BossPhase2Movement = GetComponent<Lvl4BossPhase2Movement>();
        if (lvl4BossPhase2Movement != null)
        {
            Debug.Log("Removing Lvl4BossPhase2Movement script");
            Destroy(lvl4BossPhase2Movement);
        }
    }

    void OnDestroy()
    {
        // Stop the coroutine if the object is destroyed
        if (phaseCoroutine != null)
        {
            StopCoroutine(phaseCoroutine);
        }
    }
}
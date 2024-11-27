using System.Collections;
using UnityEngine;

public class FinalBossMovement : MonoBehaviour
{
    [Header("Teleportation Settings")]
    public string teleportTargetTag = "TeleportTarget";
    public GameObject teleportEffectPrefab;
    public GameObject teleportModePrefab; // Prefab to spawn during teleport mode
    public float baseTeleportInterval = 2f;
    public float teleportIntervalRandomnessPercentage = 0.5f;
    public float teleportYOffset = 2f;

    [Header("Movement Settings")]
    public string movementTargetTag = "Boss2FMP";
    public float maxMovementSpeed = 400f;
    private float minMovementSpeed = 1f;

    [Header("Mode Switching Settings")]
    public float modeSwitchInterval = 10f;

    private GameObject[] teleportTargets;
    private GameObject[] movementTargets;
    private Transform currentMovementTarget;
    private bool isTeleportMode = false;

    private GameObject activeTeleportModePrefab; // Reference to the active prefab

    private SoundManager soundManager;
    private AttributesManager attributesManager;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        attributesManager = GetComponent<AttributesManager>();

        teleportTargets = GameObject.FindGameObjectsWithTag(teleportTargetTag);
        movementTargets = GameObject.FindGameObjectsWithTag(movementTargetTag);

        InvokeRepeating(nameof(SwitchMode), modeSwitchInterval, modeSwitchInterval);
        StartMode();
    }

    private void SwitchMode()
    {
        isTeleportMode = !isTeleportMode;
        StopAllCoroutines();
        CancelInvoke(nameof(Teleport));

        if (!isTeleportMode)
        {
            Debug.Log("Switching to Movement Mode");
            if (activeTeleportModePrefab != null)
            {
                Destroy(activeTeleportModePrefab);
            }
            StartCoroutine(MoveToRandomTarget());
        }
        else
        {
            Debug.Log("Switching to Teleport Mode");
            if (teleportModePrefab != null)
            {
                activeTeleportModePrefab = Instantiate(teleportModePrefab, transform.position, Quaternion.identity, transform);
            }
            InvokeRepeating(nameof(Teleport), 0f, GetRandomizedTeleportInterval());
        }
    }

    private void StartMode()
    {
        if (isTeleportMode)
        {
            Debug.Log("Starting in Teleport Mode");
            if (teleportModePrefab != null)
            {
                activeTeleportModePrefab = Instantiate(teleportModePrefab, transform.position, Quaternion.identity, transform);
            }
            InvokeRepeating(nameof(Teleport), 0f, GetRandomizedTeleportInterval());
        }
        else
        {
            Debug.Log("Starting in Movement Mode");
            StartCoroutine(MoveToRandomTarget());
        }
    }

    private void Teleport()
    {
        Debug.Log("Teleporting...");

        if (teleportTargets.Length == 0)
        {
            attributesManager?.itemDrop();
            Destroy(gameObject);
            return;
        }

        GameObject currentTarget = null;
        foreach (GameObject obj in teleportTargets)
        {
            if (obj.transform.position == transform.position)
            {
                currentTarget = obj;
                break;
            }
        }

        if (teleportTargets.Length == 1 && currentTarget != null) return;

        GameObject newTarget;
        do
        {
            newTarget = teleportTargets[Random.Range(0, teleportTargets.Length)];
        } while (newTarget.transform.position == transform.position);

        Vector3 oldPosition = transform.position;
        Vector3 targetPosition = newTarget.transform.position;
        soundManager?.PlaySoundBasedOnCollision("teleportSound");
        transform.position = new Vector3(targetPosition.x, targetPosition.y + teleportYOffset, targetPosition.z);

        if (teleportEffectPrefab != null)
        {
            Instantiate(teleportEffectPrefab, oldPosition, Quaternion.identity);
        }
    }

    private float GetRandomizedTeleportInterval()
    {
        return baseTeleportInterval + Random.Range(-baseTeleportInterval * teleportIntervalRandomnessPercentage, baseTeleportInterval * teleportIntervalRandomnessPercentage);
    }

    private IEnumerator MoveToRandomTarget()
    {
        while (true)
        {
            if (movementTargets.Length == 0) yield break;

            Transform randomTarget = movementTargets[Random.Range(0, movementTargets.Length)].transform;
            Vector2 startPosition = transform.position;
            Vector2 targetPosition = randomTarget.position;
            float journeyLength = Vector2.Distance(startPosition, targetPosition);
            float halfwayPoint = journeyLength / 2;
            float currentSpeed = 0f;
            float distanceTraveled = 0f;

            while (distanceTraveled < journeyLength)
            {
                float remainingDistance = Vector2.Distance(transform.position, targetPosition);
                distanceTraveled = journeyLength - remainingDistance;

                if (distanceTraveled < halfwayPoint)
                {
                    currentSpeed = Mathf.Lerp(0f, maxMovementSpeed, distanceTraveled / halfwayPoint);
                }
                else
                {
                    currentSpeed = Mathf.Lerp(maxMovementSpeed, minMovementSpeed, (distanceTraveled - halfwayPoint) / (journeyLength - halfwayPoint));
                }

                currentSpeed = Mathf.Max(currentSpeed, minMovementSpeed);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

                yield return null;
            }

            transform.position = targetPosition;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }
}

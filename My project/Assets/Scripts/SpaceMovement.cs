using UnityEngine;

public class RandomWobbleDynamic : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float minRotationSpeed = 30f;  // degrees per second
    [SerializeField] private float maxRotationSpeed = 120f; 
    [SerializeField] private float minRotationDuration = 1f;
    [SerializeField] private float maxRotationDuration = 3f;

    [Header("Movement Settings")]
    [SerializeField] private float moveRadius = 3f;
    [SerializeField] private float minMoveSpeed = 0.5f;
    [SerializeField] private float maxMoveSpeed = 3f;
    [SerializeField] private float minMoveDuration = 1f;
    [SerializeField] private float maxMoveDuration = 3f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    // Rotation variables
    private float currentRotationSpeed;
    private float targetRotationSpeed;
    private float rotationTimer;
    private float rotationDuration;

    // Movement variables
    private float currentMoveSpeed;
    private float targetMoveSpeed;
    private float moveTimer;
    private float moveDuration;

    void Start()
    {
        startPosition = transform.position;

        // Initialize rotation
        currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        PickNewRotationSpeed();

        // Initialize movement
        currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        PickNewTargetPosition();
        PickNewMoveSpeed();
    }

    void Update()
    {
        HandleRotation();
        HandleSmoothMovement();
    }

    private void HandleRotation()
    {
        rotationTimer += Time.deltaTime;
        float t = Mathf.Clamp01(rotationTimer / rotationDuration);

        // Smoothly interpolate toward target rotation speed (handles direction changes)
        currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, targetRotationSpeed, t);

        // Apply rotation
        transform.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);

        if (t >= 1f)
        {
            PickNewRotationSpeed();
        }
    }

    private void PickNewRotationSpeed()
    {
        // Pick a new random speed (direction included)
        targetRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        if (Random.value > 0.5f) targetRotationSpeed *= -1;

        // Pick random duration to reach this speed
        rotationDuration = Random.Range(minRotationDuration, maxRotationDuration);

        rotationTimer = 0f;
    }

    private void HandleSmoothMovement()
    {
        moveTimer += Time.deltaTime;
        float tSpeed = Mathf.Clamp01(moveTimer / moveDuration);
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, tSpeed);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            PickNewTargetPosition();
            PickNewMoveSpeed();
        }
    }

    private void PickNewTargetPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
        targetPosition = startPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
    }

    private void PickNewMoveSpeed()
    {
        targetMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        moveDuration = Random.Range(minMoveDuration, maxMoveDuration);
        moveTimer = 0f;
        // remove: currentMoveSpeed = currentMoveSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 gizmoPos = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawWireSphere(gizmoPos, moveRadius);
    }
}

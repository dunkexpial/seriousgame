using UnityEngine;

public class ColliderAvoidance : MonoBehaviour
{
    public float avoidanceDistance = 0.5f; // Minimum distance to maintain from obstacles
    public LayerMask obstacleLayerMask;     // Assign this layer mask to "ProjObstacle" and "Obstacle" layers

    private Collider objectCollider;
    private Vector3 originalPosition;

    void Start()
    {
        objectCollider = GetComponent<Collider>();
        originalPosition = transform.position;
    }

    void Update()
    {
        AvoidObstacles();
    }

    private void AvoidObstacles()
    {
        Collider[] obstacles = Physics.OverlapSphere(objectCollider.bounds.center, avoidanceDistance, obstacleLayerMask);
        
        if (obstacles.Length > 0)
        {
            Vector3 totalAvoidanceDirection = Vector3.zero;

            foreach (Collider obstacle in obstacles)
            {
                // Calculate the closest point on the object's collider to the obstacle collider
                Vector3 closestPoint = obstacle.ClosestPoint(objectCollider.bounds.center);

                // Calculate direction from object to the obstacle collider's closest point
                Vector3 avoidanceDirection = objectCollider.bounds.center - closestPoint;

                // If the avoidance distance is violated, add it to the total avoidance direction
                if (avoidanceDirection.magnitude < avoidanceDistance)
                {
                    totalAvoidanceDirection += avoidanceDirection.normalized;
                }
            }

            // Average out the avoidance direction and apply movement to separate the colliders
            totalAvoidanceDirection /= obstacles.Length;
            transform.position += totalAvoidanceDirection * Time.deltaTime;
        }
        else
        {
            // If no obstacles are close, revert to the original position set by other movement scripts
            transform.position = originalPosition;
        }
    }
}

using UnityEngine;

public class DestroyIfFarFromPlayer : MonoBehaviour
{
    public float destroyDistance = 20f;  // Distance at which the object will be destroyed
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance > destroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CreaturePhysics : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
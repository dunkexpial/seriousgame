using UnityEngine;

public class TagBasedCollisionIgnorer : MonoBehaviour
{
    public string tagToIgnore1; // Tag 1
    public string tagToIgnore2; // Tag 2

    private Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.LogError("No Collider2D component found on this GameObject.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (myCollider != null)
        {
            Collider2D otherCollider = collision.collider;
            string otherTag = otherCollider.CompareTag(tagToIgnore1) ? tagToIgnore1 : 
                              otherCollider.CompareTag(tagToIgnore2) ? tagToIgnore2 : 
                              null;

            // CHECA SE O TRIGGER DE COLISÃO TEM AS TAGS CORRETAS
            if (otherTag != null || otherCollider.CompareTag(gameObject.tag))
            {
                Physics2D.IgnoreCollision(otherCollider, myCollider);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (myCollider != null)
        {
            string otherTag = collider.CompareTag(tagToIgnore1) ? tagToIgnore1 :
                              collider.CompareTag(tagToIgnore2) ? tagToIgnore2 :
                              null;

            // CHECA SE O TRIGGER DE COLISÃO TEM AS TAGS CORRETAS
            if (otherTag != null || collider.CompareTag(gameObject.tag))
            {
                Physics2D.IgnoreCollision(collider, myCollider);
            }
        }
    }
}

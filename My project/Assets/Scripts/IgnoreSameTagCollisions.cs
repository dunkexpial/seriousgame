using UnityEngine;

public class IgnoreSameTagCollisions : MonoBehaviour
{
    void Start()
    {
        // Get the collider of the current object
        Collider2D thisCollider = GetComponent<Collider2D>();

        if (thisCollider == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have a Collider2D component.");
            return;
        }

        // Find all objects with the same tag
        GameObject[] objectsWithSameTag = GameObject.FindGameObjectsWithTag(gameObject.tag);

        foreach (GameObject obj in objectsWithSameTag)
        {
            // Ensure the object is not the same as itself
            if (obj != gameObject)
            {
                Collider2D otherCollider = obj.GetComponent<Collider2D>();
                if (otherCollider != null)
                {
                    Physics2D.IgnoreCollision(thisCollider, otherCollider);
                }
            }
        }
    }
}
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // Tag to check for existing objects
    public string requiredTag;

    // Player's tag
    public string playerTag;

    // Toggle to enable or disable tag logic
    public bool useTagLogic = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that collided is the player
        if (collision.CompareTag(playerTag))
        {
            if (useTagLogic)
            {
                // Check if there are any objects with the specified tag in the scene
                GameObject existingObject = GameObject.FindWithTag(requiredTag);

                if (existingObject == null)
                {
                    // If no objects with the tag are found, destroy this object
                    Destroy(gameObject);
                }
                // If an object with the tag is found, do nothing
            }
            else
            {
                // If tag logic is disabled, directly destroy this object
                Destroy(gameObject);
            }
        }
    }
}

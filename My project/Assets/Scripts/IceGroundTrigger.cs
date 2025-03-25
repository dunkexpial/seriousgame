using UnityEngine;

public class IceGroundTrigger : MonoBehaviour
{
    // This function is called when the collider enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that collided is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Get the PlayerCollision component from the player object
            PlayerCollision playerCollision = other.GetComponent<PlayerCollision>();
            
            // If the PlayerCollision component is found, call the IceGroundClusterfuck method
            if (playerCollision != null)
            {
                playerCollision.IceGroundClusterfuck();
            }
        }
    }
}

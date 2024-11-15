using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    public List<string> requiredTags; // List of tags to check for

    void Update()
    {
        bool foundObject = false;

        // Check each tag in the list
        foreach (string tag in requiredTags)
        {
            // Check if there's any active object with the current tag
            if (GameObject.FindGameObjectWithTag(tag) != null)
            {
                foundObject = true;
                break; // Exit the loop as we found at least one matching object
            }
        }

        // Destroy this object if no matching objects were found
        if (!foundObject)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCue : MonoBehaviour
{
    [Header("VisualCue")]
    [SerializeField] private GameObject visualCue; 
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    private bool playerInRange; 
    private InventoryManager iventoryManager;
    private void Awake() 
    {
        playerInRange = false; // The player is not in range at the start
        visualCue.SetActive(false); // The visual cue is initially disabled
    }

    private void Start()
    {
        //Find the Canvas then, it gets the component of type IventoryManager attached to that GameObject and assigns it to the iventoryManager variable.
        iventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }
    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true); // Show the icon if the player can interact
            
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Add slot sprite 
                iventoryManager.AddItem(itemName, sprite);// i'll destroy it for now, i'll figure out how to save the items to the next scene
                Destroy(gameObject);

                // Calls the portal Singleton's ActivatePortal method
                if (SceneChangeOnHover.instance != null)
                {
                    SceneChangeOnHover.instance.ActivePortal();
                }
                else
                {
                    Debug.LogError("Portal not found!");
                }     
            }
        }
        else
        {
            visualCue.SetActive(false); // Hide the icon if the player is out of range
        }
    }

    // Detects when the player enters the interaction range of the NPC
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Checks if the object that entered the area has the "Player" tag and if it's the player
        if (collider.CompareTag("Player") )
        {
            playerInRange = true; // Player is in range, quite obvious
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Checks if the object that left the area is the player
        if (collider.CompareTag("Player"))
        {
            playerInRange = false; //Player out of the range, quite obvious      
            visualCue.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, Interactable
{
    // Dialogue object associated with the NPC that triggers the dialogue
    [SerializeField] private DialogueObject dialogueObject;

    // Variables for the visual cue system (interaction icon)
    [Header("VisualCue")]
    private bool playerInRange; 
    [SerializeField] private GameObject visualCue; 
    [SerializeField] private DialogueUI dialogueUI;

    private void Awake() 
    {
        playerInRange = false; // The player is not in range at the start
        visualCue.SetActive(false); // The visual cue is initially disabled
    }
    private void Update()
    {
        // Show the visual cue when the player is in range and the dialogue is not active
        if (playerInRange && !dialogueUI.isOpen)
        {
            visualCue.SetActive(true); // Show the icon if the player can interact
        }
        else
        {
            visualCue.SetActive(false); // Hide the icon if the player is out of range or the dialogue is active
        }
    }

    // Detects when the player enters the interaction range of the NPC
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Checks if the object that entered the area has the "Player" tag and if it's the player
        if (collider.CompareTag("Player") && collider.TryGetComponent(out playermovement player))
        {
            playerInRange = true; // Player is in range, quite obvious
            player.Interactable = this; // Set this object as interactable by the player
            // this: Refers to the current instance of the DialogueActivator class.
        }
    }

    // Detects when the player leaves the interaction range of the NPC
    private void OnTriggerExit2D(Collider2D collider)
    {
        // Checks if the object that left the area is the player
        if (collider.CompareTag("Player") && collider.TryGetComponent(out playermovement player))
        {
            playerInRange = false; //Player out of the range, quite obvious

            // If the player is interacting with the NPC, remove the interaction
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactable = null; // Remove the object as interactable
            }
            // Hide the visual cue when the player leaves the range
            visualCue.SetActive(false);
        }
    }

    // Interaction function, called when the player presses the interaction button
    public void Interact(playermovement player)
    {
        // Disable the visual cue when the dialogue starts
        visualCue.SetActive(false);

        // Show the dialogue using the player's UI system
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}

// I don't blame you if don't understand, i hardly know how i did this shit

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInteraction : MonoBehaviour
{
    public GameObject visualCue;
    public GameObject creditsObject;
    private bool playerInRange = false;
    public InventoryManager inventoryManager;

    void Start()
    {
        visualCue.SetActive(false);
        creditsObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (IsInventoryEmpty())
                {
                    creditsObject.SetActive(true);
                    visualCue.SetActive(false);
                    Destroy(GameObject.FindWithTag("Player"));
                }
                else if (IsInventoryFull())
                {
                    Debug.Log("Inventory full. Get rid of it!");
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private bool IsInventoryFull()
    {
        foreach (var slot in inventoryManager.itemSlot)
        {
            if (!slot.isFull)
            {
                return false; // Not full
            }
        }
        return true; // Slots Full
    }

    private bool IsInventoryEmpty()
    {
        foreach (var slot in inventoryManager.itemSlot)
        {
            if (slot.isFull)
            {
                return false; //Has itens in the inventory
            }
        }
        return true; // No itens
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

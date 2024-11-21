using UnityEngine;

public class DialogueInventoryHandler : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI; 
    [SerializeField] private InventoryManager inventoryManager; 

    private void Update()
    {

        if (dialogueUI.IsDialogueActive())
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClearInventory(); 
            }
        }
    }
    private void ClearInventory()
    {
        foreach (var slot in inventoryManager.itemSlot)
        {
            slot.ClearSlot(); 
        }

        Debug.Log("Inventory Clear");
    }

}

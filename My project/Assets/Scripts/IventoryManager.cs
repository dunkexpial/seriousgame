using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IventoryManager : MonoBehaviour
{
    public ItemSlot[] itemSlot; // An array of ItemSlot objects

    public void AddItem(string itemName, Sprite itemSprite)
    {
        Debug.Log("itemName = " + itemName + " itemSprite= " + itemSprite);

        // Loop through each slot in the itemSlot array
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // Check if the current itemSlot is not full (has space for an item)
            if (itemSlot[i].isFull == false)
            {
                // If the slot is empty, add the item to this slot by calling the AddItem method on that slot
                itemSlot[i].AddItem(itemName, itemSprite);
                
                // Exit the method after adding the item to avoid placing it in multiple slots
                return;
            }
        }
    }

}

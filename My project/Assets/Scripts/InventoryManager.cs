using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot[] itemSlot;

    [System.Serializable]
    public class PredefinedItem
    {
        public string itemName;
        public Sprite itemSprite;
    }

    [Header("Items for This Level")]
    public List<PredefinedItem> levelItems; // A list of items predefined for this level


    private void Start()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        // Clear all inventory slots to make sure there's nothing left
        foreach (var slot in itemSlot)
        {
            slot.ClearSlot();
        }


        for (int i = 0; i < levelItems.Count && i < itemSlot.Length; i++)
        {
             // Add the predefined item to the current inventory slot
            itemSlot[i].AddItem(levelItems[i].itemName, levelItems[i].itemSprite);
        }
    }

    public void AddItem(string itemName, Sprite itemSprite)
    {
        // Loop through each slot in the itemSlot array
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // Check if the current slot is empty && Add the item to this slot
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(itemName, itemSprite);
                return;
            }
        }
    }
}

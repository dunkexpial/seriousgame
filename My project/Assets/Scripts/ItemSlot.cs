using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite;

    // A boolean flag to check if the slot is full (has an item)
    public bool isFull;
    [SerializeField] private Image itemImage;

    // This method adds an item to the slot
    public void AddItem(string itemName, Sprite itemSprite)
    {
        // Assign the name of the item [Not necessary, it is what it is] >:p
        this.itemName = itemName;

        // Assign the sprite of the item [Same above]
        this.itemSprite = itemSprite;

        // Mark the slot as full since an item has been added
        isFull = true;

        // Update the UI image component with the new item's sprite
        itemImage.sprite = itemSprite;
    }
}

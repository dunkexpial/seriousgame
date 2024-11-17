using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite;
    public bool isFull;
    [SerializeField] private Image itemImage;

    public void AddItem(string itemName, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        isFull = true;
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
    }

    public void ClearSlot()
    {
        itemName = "";
        itemSprite = null;
        isFull = false;
        itemImage.sprite = null;
        itemImage.enabled = false;
    }
}


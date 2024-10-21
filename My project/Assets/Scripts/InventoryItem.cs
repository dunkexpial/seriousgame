using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InventoryItem 
{
    string Name { get; }
    Sprite Image { get; }

    void OnPickup(); 
}
public class IventoryEventsArgs : EventArgs
{
      public IventoryEventsArgs(InventoryItem item)
      {
        Item = item;
      }

      public InventoryItem Item; 
}

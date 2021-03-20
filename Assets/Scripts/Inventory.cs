using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Inventory 
{
    public List<Item> Items = new List<Item>();
    public UnityEvent InventoryChanged = new UnityEvent();

    public int MaxWeight = 100;
    public int CarriedWeight { get { return combinedWeight(); } }

    int combinedWeight()
    {
        int weight = 0;
        for ( int i = 0; i < Items.Count; i++ )
        {
            weight += Items[i].Weight;
        }
        return ( weight > 0 ) ? weight : 0; 
    }

    public void AddItemToInventory(Item item)
    {
        Items.Add( item );
        InventoryChanged.Invoke();
    }
}

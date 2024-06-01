using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List <Consumable> Items { get; private set; } = new List <Consumable> ();
    public int maxItems;

    public bool AddItem (Consumable item)
    {
        if (Items.Count < maxItems)
        {
            Items.Add (item);
            return true;
        }
        return false;
    }
    public void DropItem (Consumable item)
    {
        Items.Remove(item);
    }
    public bool IsFull
    {
        get { return Items.Count >= maxItems; }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryDataEntry
{
    public string itemName;
    public int amount;
}

public class InventoryData : MonoBehaviour
{
    [SerializeField] private List<InventoryDataEntry> items = new List<InventoryDataEntry>();

    public int GetItemAmount(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            return 0;
        }

        foreach (InventoryDataEntry entry in items)
        {
            if (entry == null)
            {
                continue;
            }

            if (string.Equals(entry.itemName, itemName, StringComparison.OrdinalIgnoreCase))
            {
                return entry.amount;
            }
        }

        return 0;
    }

    public void AddItem(string itemName, int amount)
    {
        if (string.IsNullOrEmpty(itemName) || amount <= 0)
        {
            return;
        }

        foreach (InventoryDataEntry entry in items)
        {
            if (entry == null)
            {
                continue;
            }

            if (string.Equals(entry.itemName, itemName, StringComparison.OrdinalIgnoreCase))
            {
                entry.amount += amount;
                return;
            }
        }

        items.Add(new InventoryDataEntry { itemName = itemName, amount = amount });
    }
}

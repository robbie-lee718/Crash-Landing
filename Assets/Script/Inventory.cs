using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlotUI[] slotsUIs;
    public InventorySlotData[] slots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slots = new InventorySlotData[slotsUIs.Length];
        UpdateUI();
    }

    public void AddItem(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            if (slots[i].item == item && slots[i].amount < item.maxStackSize)
            {
                int spaceLeft = item.maxStackSize - slots[i].amount;
                int amountToAdd = Mathf.Min(spaceLeft, amount);

                slots[i].amount += amountToAdd;
                amount -= amountToAdd;

                if (amount <= 0)
                {
                    UpdateUI();
                    return;
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                continue;
            }

            int amountToAdd = Mathf.Min(item.maxStackSize, amount);
            slots[i] = new InventorySlotData(item, amountToAdd);
            amount -= amountToAdd;

            if (amount <= 0)
            {
                UpdateUI();
                return;
            }
        }

        UpdateUI();
    }

    public int GetItemAmount(string itemName)
    {
        if (slots == null)
        {
            return 0;
        }

        int total = 0;

        foreach (InventorySlotData slot in slots)
        {
            if (slot == null || slot.item == null)
            {
                continue;
            }

            if (string.Equals(slot.item.itemName, itemName, StringComparison.OrdinalIgnoreCase))
            {
                total += slot.amount;
            }
        }

        return total;
    }

    public int GetItemAmount(ItemData item)
    {
        if (item == null)
        {
            return 0;
        }

        return GetItemAmount(item.itemName);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotsUIs.Length; i++)
        {
            if (slots[i] == null)
            {
                slotsUIs[i].ClearSlot();
            } else
            {
                slotsUIs[i].SetSlot(slots[i].item, slots[i].amount);
            }
        }
    }
}

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

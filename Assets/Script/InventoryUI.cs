using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isInventoryOpen;

    void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
            isInventoryOpen = false;
        }
        else
        {
            Debug.LogError("Inventory panel is not assigned in the inspector.");
        }
    }
    
    public void OnInventory(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        ToggleInventory();
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isInventoryOpen);
        }
    }
}
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class FixShip : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private string promptText = "Press E to repair ship";
    [SerializeField] private TextMeshProUGUI interactionPromptUI;
    [SerializeField] private bool showPrompt;
    [SerializeField] private bool canShowRepairPrompt;
    [System.Serializable]
    private class RepairRequirement
    {
        public ItemData item;
        public int amountNeeded = 1;
    }

    [Header("Repair Requirements")]
    [SerializeField] private List<RepairRequirement> repairRequirements = new List<RepairRequirement>();

    [Header("Repair State")]
    [SerializeField] private bool isRepaired;

    private Inventory inventory;
    private Transform playerTransform;

    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (interactionPromptUI != null)
        {
            interactionPromptUI.text = string.Empty;
        }
    }

    private void Update()
    {
        if (isRepaired)
        {
            showPrompt = false;
            canShowRepairPrompt = false;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.text = string.Empty;
            }

            return;
        }

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (playerTransform == null)
        {
            showPrompt = false;
            canShowRepairPrompt = false;

            if (interactionPromptUI != null)
            {
                interactionPromptUI.text = string.Empty;
            }

            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        showPrompt = distance <= interactionRange;
        canShowRepairPrompt = showPrompt && CanRepair();

        if (interactionPromptUI != null)
        {
            interactionPromptUI.text = showPrompt ? promptText : string.Empty;
        }

        if (showPrompt && Input.GetKeyDown(interactionKey))
        {
            TryRepair();
        }
    }

    private void OnGUI()
    {
        if (!showPrompt || interactionPromptUI != null)
        {
            return;
        }

        if (showPrompt)
        {
            GUI.Box(new Rect(Screen.width / 2f - 120f, Screen.height - 70f, 240f, 30f), promptText);
        }
    }

    public bool CanRepair()
    {
        if (isRepaired || inventory == null || inventory.slots == null)
        {
            return false;
        }

        for (int i = 0; i < repairRequirements.Count; i++)
        {
            if (!HasRequiredItems(repairRequirements[i].item, repairRequirements[i].amountNeeded))
            {
                return false;
            }
        }

        return true;
    }

    public bool TryRepair()
    {
        if (!CanRepair())
        {
            return false;
        }

        for (int i = 0; i < repairRequirements.Count; i++)
        {
            if (!ConsumeRequiredItems(repairRequirements[i].item, repairRequirements[i].amountNeeded))
            {
                return false;
            }
        }

        isRepaired = true;
        showPrompt = false;
        canShowRepairPrompt = false;

        if (interactionPromptUI != null)
        {
            interactionPromptUI.text = string.Empty;
        }

        Debug.Log("Ship repaired!");
        return true;
    }

    private bool HasRequiredItems(ItemData item, int amountNeeded)
    {
        if (item == null || inventory == null || inventory.slots == null)
        {
            return false;
        }

        int foundAmount = 0;

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i] == null || inventory.slots[i].item == null)
            {
                continue;
            }

            if (inventory.slots[i].item == item)
            {
                foundAmount += inventory.slots[i].amount;
            }
        }

        return foundAmount >= amountNeeded;
    }

    private bool ConsumeRequiredItems(ItemData item, int amountNeeded)
    {
        if (!HasRequiredItems(item, amountNeeded) || inventory == null || inventory.slots == null)
        {
            return false;
        }

        int remainingToConsume = amountNeeded;

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (remainingToConsume <= 0)
            {
                break;
            }

            if (inventory.slots[i] == null || inventory.slots[i].item == null || inventory.slots[i].item != item)
            {
                continue;
            }

            int amountTaken = Mathf.Min(inventory.slots[i].amount, remainingToConsume);
            inventory.slots[i].amount -= amountTaken;
            remainingToConsume -= amountTaken;

            if (inventory.slots[i].amount <= 0)
            {
                inventory.slots[i] = null;
            }
        }

        return remainingToConsume <= 0;
    }
}

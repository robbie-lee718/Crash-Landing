using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FixShip : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private Transform playerArmeture;
    [SerializeField] private Inventory playerInventory;

    [Header("Prompt")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI resourcesText;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private string promptMessage = "Press E to interact";
    [SerializeField] private string promptReadyMessage = "Ship is ready to repair";
    [SerializeField] private string winMessage = "You repaired the ship!";

    private static readonly string[] RequiredResourceTypes = { "Wood", "Ore", "Rock", "Barrel" };
    private static readonly int[] RequiredResourceAmounts = { 5, 3, 10, 3 };

    private bool isPlayerNear;
    private bool isGameWon;

    private void Start()
    {
        ResolvePromptReferences();
        SetPromptVisible(false);
    }

    private void Update()
    {
        if (isGameWon)
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }

            return;
        }

        bool foundPlayer = CheckForPlayer();
        SetPromptVisible(foundPlayer);

        if (foundPlayer && HasAllRequiredResources() && IsInteractPressed())
        {
            ShowWinText();
        }
    }

    private bool IsInteractPressed()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            return true;
        }

        return Input.GetKeyDown(KeyCode.E);
    }

    private bool CheckForPlayer()
    {
        bool foundPlayer = false;
        Transform resolvedPlayer = ResolvePlayerArmeture();

        if (resolvedPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, resolvedPlayer.position);
            foundPlayer = distanceToPlayer <= detectionRadius;
        }

        if (isPlayerNear != foundPlayer)
        {
            isPlayerNear = foundPlayer;
        }

        return foundPlayer;
    }

    private Transform ResolvePlayerArmeture()
    {
        if (playerArmeture != null)
        {
            return playerArmeture;
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject candidate in players)
        {
            Transform candidateTransform = candidate.transform;
            if (candidateTransform.name.Contains("Armeture", StringComparison.OrdinalIgnoreCase) ||
                candidateTransform.name.Contains("Armature", StringComparison.OrdinalIgnoreCase))
            {
                playerArmeture = candidateTransform;
                return playerArmeture;
            }
        }

        if (players.Length > 0)
        {
            playerArmeture = players[0].transform;
            return playerArmeture;
        }

        return null;
    }

    private void ResolvePromptReferences()
    {
        if (interactionPrompt == null)
        {
            interactionPrompt = transform.Find("InteractionPrompt")?.gameObject;
        }

        if (interactionPrompt == null)
        {
            interactionPrompt = transform.Find("Prompt")?.gameObject;
        }

        if (promptText == null && interactionPrompt != null)
        {
            promptText = interactionPrompt.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void SetPromptVisible(bool visible)
    {
        bool hasAllResources = HasAllRequiredResources();

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(visible);
        }

        if (promptText != null)
        {
            promptText.gameObject.SetActive(visible && !hasAllResources);

            if (visible && !hasAllResources)
            {
                promptText.text = promptMessage;
            }
        }

        if (resourcesText != null)
        {
            resourcesText.gameObject.SetActive(visible && !hasAllResources);

            if (visible && !hasAllResources)
            {
                resourcesText.text = BuildResourcesText();
            }
        }

        if (readyText != null)
        {
            readyText.gameObject.SetActive(visible && hasAllResources && !IsWinShown());

            if (visible && hasAllResources && !IsWinShown())
            {
                readyText.text = promptReadyMessage;
            }
        }

        if (winText != null)
        {
            winText.gameObject.SetActive(IsWinShown());
        }
    }

    private string BuildResourcesText()
    {
        if (RequiredResourceTypes == null || RequiredResourceTypes.Length == 0)
        {
            return "Resources needed: none";
        }

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.AppendLine("Resources needed:");

        for (int i = 0; i < RequiredResourceTypes.Length; i++)
        {
            string type = RequiredResourceTypes[i];
            int requiredAmount = 0;

            if (RequiredResourceAmounts != null && i < RequiredResourceAmounts.Length)
            {
                requiredAmount = RequiredResourceAmounts[i];
            }

            builder.AppendLine($"- {type}: {requiredAmount}");
        }

        return builder.ToString();
    }

    private int GetInventoryAmount(Inventory inventory, InventoryData inventoryData, string resourceType)
    {
        if (inventory != null)
        {
            return inventory.GetItemAmount(resourceType);
        }

        if (inventoryData == null)
        {
            return 0;
        }

        return inventoryData.GetItemAmount(resourceType);
    }

    private bool HasAllRequiredResources()
    {
        if (RequiredResourceTypes == null || RequiredResourceTypes.Length == 0)
        {
            return true;
        }

        Inventory inventory = ResolvePlayerInventory();
        InventoryData inventoryData = ResolveInventoryData();

        if (inventory == null && inventoryData == null)
        {
            return false;
        }

        for (int i = 0; i < RequiredResourceTypes.Length; i++)
        {
            string type = RequiredResourceTypes[i];
            int requiredAmount = 0;

            if (RequiredResourceAmounts != null && i < RequiredResourceAmounts.Length)
            {
                requiredAmount = RequiredResourceAmounts[i];
            }

            int availableAmount = GetInventoryAmount(inventory, inventoryData, type);

            if (requiredAmount <= 0)
            {
                continue;
            }

            if (availableAmount < requiredAmount)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsWinShown()
    {
        return winText != null && winText.gameObject.activeSelf;
    }

    private void ShowWinText()
    {
        if (winText == null)
        {
            return;
        }

        isGameWon = true;
        Time.timeScale = 0f;
        winText.text = winMessage;
        winText.gameObject.SetActive(true);

        if (readyText != null)
        {
            readyText.gameObject.SetActive(false);
        }

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        if (resourcesText != null)
        {
            resourcesText.gameObject.SetActive(false);
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private Inventory ResolvePlayerInventory()
    {
        if (playerInventory != null)
        {
            return playerInventory;
        }

        Inventory[] inventories = FindObjectsByType<Inventory>();

        if (inventories == null || inventories.Length == 0)
        {
            return null;
        }

        foreach (Inventory candidate in inventories)
        {
            if (candidate != null)
            {
                playerInventory = candidate;
                return playerInventory;
            }
        }

        return null;
    }

    private InventoryData ResolveInventoryData()
    {
        InventoryData[] inventories = FindObjectsByType<InventoryData>();

        if (inventories == null || inventories.Length == 0)
        {
            return null;
        }

        foreach (InventoryData candidate in inventories)
        {
            if (candidate != null)
            {
                return candidate;
            }
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

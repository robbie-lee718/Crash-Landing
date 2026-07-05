using UnityEngine;

public class InteractibleResource : MonoBehaviour
{
    public string resourceName = "Apple";
    public int amountPerCollect = 1;
    public int useRemaining = 1;

    public string promptText = "Press 'E' to interact";
    public string animaterTrigger = "PickFruit";

    public bool destroyOnUse = true;

    private ResourceCounter resourceCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceCounter = FindFirstObjectByType<ResourceCounter>();
    }

    public void Interact()
    {
        if (useRemaining <= 0)
        {
            return;
        }

        if (resourceCounter != null)
        {
            resourceCounter.AddResource(resourceName, amountPerCollect);
        }
        
        useRemaining--;
        
        if (useRemaining <= 0 && destroyOnUse)
        {
            gameObject.SetActive(false);
        }
    }
}

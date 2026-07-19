using UnityEngine;

public class InteractibleResource : MonoBehaviour
{
    public ItemData item;
    public int amountPerCollect = 1;
    public int useRemaining = 1;

    public string promptText = "Press 'E' to interact";
    public string animaterTrigger = "PickFruit";

    public bool destroyOnUse = true;

    public void Interact(Inventory inventory)
    {
        if (useRemaining <= 0)
        {
            return;
        }

        if (item != null)
        {
            if (inventory != null)
            {
                inventory.AddItem(item, amountPerCollect);
            }
        }
        
        useRemaining--;
        
        if (useRemaining <= 0 && destroyOnUse)
        {
            gameObject.SetActive(false);
        }
    }
}

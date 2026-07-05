using TMPro;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    public TextMeshProUGUI appleText;
    public TextMeshProUGUI oreText;

    private int apples;
    private int ores;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        appleText.text = $"Apples: {apples}";
        oreText.text = $"Ores: {ores}";
    }

    public void AddResource(string resourceName, int amount)
    {
        if (resourceName == "Apple")
        {
            apples += amount;
        }
        else if (resourceName == "Ore")
        {
            ores += amount;
        }

        UpdateUI();
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public Light sun;
    public float cycleInSeconds = 120f;
    private float dayIntensity = 0.2f;
    private float nightIntensity = 0.05f;

    private Color dayColor = new Color(0.55f, 0.58f, 0.62f);
    private Color nightColor = new Color(0.01f, 0.012f, 0.010f);

    private Color dayTint = new Color(0.35f, 0.45f, 0.6f);
    private Color nightTint = new Color(0.005f, 0.0065f, 0.01f);

    private float timeOfDay = 0.5f;

    public bool IsNight { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sun == null)
        {
            return;
        }

        timeOfDay += Time.deltaTime / cycleInSeconds;

        if (timeOfDay >= 1f)
        {
            timeOfDay = 0f;
        }

        UpdateLighting();
    }

    private void UpdateLighting()
    {
        float sunAngle = timeOfDay * 360f - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        float lightAmount = Mathf.Clamp01(Mathf.Sin(timeOfDay * Mathf.PI));
        sun.intensity = Mathf.Lerp(nightIntensity, dayIntensity, lightAmount);

        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(nightTint, dayTint, lightAmount));
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(0.00f, 0.65f, lightAmount));
        }

        IsNight = lightAmount < 0.25f;
    }
}

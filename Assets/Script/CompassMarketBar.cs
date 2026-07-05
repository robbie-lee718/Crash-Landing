using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CompassMarketBar : MonoBehaviour
{
    public Transform player;
    public RectTransform markerContainer;
    public TMP_Text markerPrefab;
    private float panelWidth = 500f;
    float pixelsPerDegree = 4f;

    private readonly string[] labels =
    {
        "N", "NE", "E", "SE", "S", "SW", "W", "NW"
    };

    private TMP_Text[] markers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        markers = new TMP_Text[labels.Length];

        for (int i = 0; i < labels.Length; i++)
        {
            TMP_Text marker = Instantiate(markerPrefab, markerContainer);
            marker.text = labels[i];
            marker.alignment = TextAlignmentOptions.Center;
            marker.gameObject.SetActive(true);
            markers[i] = marker;
        }

        markerPrefab.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || markerContainer == null || markers == null)
        {
            return;
        }

        float heading = player.eulerAngles.y;

        for (int i = 0; i < markers.Length; i++)
        {
            float markerAngle = i * 45f;
            float relativeAngle = Mathf.DeltaAngle(heading, markerAngle);
            float xPos = relativeAngle * pixelsPerDegree;

            markers[i].rectTransform.anchoredPosition = new Vector2(xPos, 0f);

            bool visible = Mathf.Abs(xPos) < panelWidth / 2f;
            markers[i].gameObject.SetActive(visible);
        }
    }
}

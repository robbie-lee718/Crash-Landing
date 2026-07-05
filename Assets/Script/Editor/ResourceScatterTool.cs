using UnityEngine;
using UnityEditor;

public class ResourceScatterTool : EditorWindow
{
    private Terrain targetTerrain;
    private GameObject resourcePrefab;
    private Transform resourceParent;

    private int amount = 20;
    private float minDistance = 8f;

    [MenuItem("Tools/Resource Scatter Tool")]

    public static void ShowWindow()
    {
        GetWindow<ResourceScatterTool>("Resource Scatter Tool");
    }

    public void OnGUI()
    {
        GUILayout.Label("Resource Scatter Tool", EditorStyles.boldLabel);
        targetTerrain = (Terrain)EditorGUILayout.ObjectField(
            "Target Terrain",
            targetTerrain,
            typeof(Terrain),
            true
        );

        resourcePrefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab to Scatter",
            resourcePrefab,
            typeof(GameObject),
            false
        );

        resourceParent = (Transform)EditorGUILayout.ObjectField(
            "Parent Object",
            resourceParent,
            typeof(Transform),
            true
        );

        amount = EditorGUILayout.IntField("Amount", amount);
        minDistance = EditorGUILayout.FloatField("Minimum Distance", minDistance);

        if (GUILayout.Button("Scatter Resources"))
        {
            ScatterResources();
        }
    }

    private void ScatterResources()
    {
        if (targetTerrain == null || resourcePrefab == null || resourceParent == null)
        {
            Debug.LogError("Please assign all fields before scattering resources.");
            return;
        }

        TerrainData terrainData = targetTerrain.terrainData;
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPosition = targetTerrain.transform.position;

        int placedCount = 0;
        int attempts = 0;
        int maxAttempts = amount * 50;

        while (placedCount < amount && attempts < maxAttempts)
        {
            attempts++;
            float randomX = Random.Range(0f, terrainSize.x);
            float randomZ = Random.Range(0f, terrainSize.z);

            float y = terrainData.GetInterpolatedHeight(
                randomX / terrainSize.x,
                randomZ / terrainSize.z
            );

            Vector3 spawnPosition = new Vector3(
                terrainPosition.x + randomX,
                terrainPosition.y + y,
                terrainPosition.z + randomZ
            );

            if (!IsFarEnough(spawnPosition))
            {
                continue;
            }

            GameObject newResource = (GameObject) PrefabUtility.InstantiatePrefab(resourcePrefab);

            Undo.RegisterCreatedObjectUndo(newResource, "Scatter Resource");

            newResource.transform.position = spawnPosition;
            newResource.transform.SetParent(resourceParent);
            newResource.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            placedCount++;
        }

        Debug.Log($"Scattered {placedCount} resources after {attempts} attempts.");
    }

    private bool IsFarEnough(Vector3 position)
    {
        foreach (Transform child in resourceParent)
        {
            if (Vector3.Distance(child.position, position) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}

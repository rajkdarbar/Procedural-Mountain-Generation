using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTerrain : MonoBehaviour
{
    public GameObject terrainPrefab;
    public int chunkSize = 30;
    public float triggerDistance = 5f;
    public Transform cameraTransform;

    private float masterSeed = 1.5f;
    private Vector3 lastCamPos;
    private Dictionary<string, GameObject> existingChunks = new Dictionary<string, GameObject>();

    void Start()
    {
        lastCamPos = cameraTransform.position;
        GenerateInitialChunks();
    }

    void Update()
    {
        Vector3 camMove = cameraTransform.position - lastCamPos;
        if (camMove.magnitude > triggerDistance)
        {
            lastCamPos = cameraTransform.position;
            GenerateTerrainAroundCamera();
        }
    }

    void GenerateInitialChunks()
    {
        // Center camera
        cameraTransform.position = new Vector3(chunkSize / 2, cameraTransform.position.y, chunkSize / 2);

        // Generate 3x3 chunks
        for (int z = -1; z <= 1; z++)
        {
            for (int x = -1; x <= 1; x++)
            {
                GenerateTerrainChunk(x, z);
            }
        }
    }

    void GenerateTerrainAroundCamera()
    {
        int camX = Mathf.FloorToInt(cameraTransform.position.x / chunkSize);
        int camZ = Mathf.FloorToInt(cameraTransform.position.z / chunkSize);

        List<string> toRemove = new List<string>();

        // Remove old chunks
        foreach (var key in existingChunks.Keys)
        {
            var parts = key.Split(',');
            int x = int.Parse(parts[0]);
            int z = int.Parse(parts[1]);

            if (Mathf.Abs(x - camX) > 1 || Mathf.Abs(z - camZ) > 1)
            {
                Destroy(existingChunks[key]);
                toRemove.Add(key);
            }
        }

        foreach (var key in toRemove)
        {
            existingChunks.Remove(key);
        }

        // Generate new chunks
        for (int z = camZ - 1; z <= camZ + 1; z++)
        {
            for (int x = camX - 1; x <= camX + 1; x++)
            {
                GenerateTerrainChunk(x, z);
            }
        }
    }

    void GenerateTerrainChunk(int x, int z)
    {
        string chunkId = $"{x},{z}";
        if (existingChunks.ContainsKey(chunkId))
            return;

        Vector3 offset = new Vector3(x * chunkSize, 0, z * chunkSize);
        GameObject newTerrain = Instantiate(terrainPrefab, offset, Quaternion.identity);
        existingChunks.Add(chunkId, newTerrain);

        float localSeed = masterSeed + (x * 1000 + z);
        CustomTerrain newTerrainScript = newTerrain.GetComponent<CustomTerrain>();
        newTerrainScript.GenerateTerrain(offset, localSeed);
    }
}

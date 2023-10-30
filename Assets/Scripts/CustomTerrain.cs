using UnityEngine;

public class CustomTerrain : MonoBehaviour
{
    public int xSize = 30;
    public int zSize = 30;
    private float masterSeed = 1.5f;

    void Start()
    {
        Vector3 offset = new Vector3(0, 0, 0);
        float localSeed = masterSeed + (transform.position.x * 1000 + transform.position.z);

        GenerateTerrain(offset, localSeed);
    }

    public void GenerateTerrain(Vector3 offset, float seed)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        int[] triangles = new int[xSize * zSize * 6];
        Vector2[] uv = new Vector2[(xSize + 1) * (zSize + 1)];

        float baseScale = 0.1f;
        float heightMultiplier = 10f;
        float minHeight = 0;

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float worldX = x + offset.x;
                float worldZ = z + offset.z;

                float y = 0;

                if (x == 0 || x == xSize || z == 0 || z == zSize)
                {
                    y = 10.5f;
                }
                else
                {
                    y += Mathf.PerlinNoise((worldX + seed) * baseScale, (worldZ + seed) * baseScale) * 2f;
                    y += Mathf.PerlinNoise((worldX + seed) * baseScale * 2f, (worldZ + seed) * baseScale * 2f) * 1f;
                    y += Mathf.PerlinNoise((worldX + seed) * baseScale * 4f, (worldZ + seed) * baseScale * 4f) * 0.5f;
                    y *= heightMultiplier;

                    if (y < minHeight)
                        y = minHeight;
                }

                vertices[i] = new Vector3(x, y, z);
                uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }


        int triangleIndex = 0, vertexIndex = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + xSize + 1;
                triangles[triangleIndex + 2] = vertexIndex + 1;
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + xSize + 1;
                triangles[triangleIndex + 5] = vertexIndex + xSize + 2;

                triangleIndex += 6;
                vertexIndex++;
            }
            vertexIndex++;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
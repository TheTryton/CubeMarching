using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Terrain : MonoBehaviour
{
    public Vector3Int chunks;
    private List<Chunk> instantiatedChunks = new List<Chunk>();
    public Vector3Int voxelDimensions;
    public float solidnessThreshold;
    private float lastSolidnessThreshold;
    public ComputeShader march3D;
    public ComputeShader trianglesToMesh;
    public Material terrainMaterial;

    private void RegenerateTerrain()
    {
        if (lastSolidnessThreshold != solidnessThreshold)
        {
            foreach(Chunk c in instantiatedChunks)
            {
                c.solidnessThreshold = solidnessThreshold;
            }
            lastSolidnessThreshold = solidnessThreshold;
        }
    }

    public void Start()
    {
        for (int x = 0; x < chunks.x; x++)
        {
            for (int y = 0; y < chunks.y; y++)
            {
                for (int z = 0; z < chunks.z; z++)
                {
                    GameObject g = Instantiate(new GameObject(), new Vector3((float)x, (float)y, (float)z), Quaternion.identity);
                    g.AddComponent<MeshFilter>();
                    g.AddComponent<MeshRenderer>().sharedMaterial = terrainMaterial;
                    Chunk c = g.AddComponent<Chunk>();
                    c.march3D = march3D;
                    c.trianglesToMesh = trianglesToMesh;
                    c.voxelDimensions = voxelDimensions;
                    c.solidnessThreshold = solidnessThreshold;
                    instantiatedChunks.Add(c);
                }
            }
        }
    }

    public void Update()
    {
        RegenerateTerrain();
    }
}

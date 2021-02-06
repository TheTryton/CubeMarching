using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
class Chunk : MonoBehaviour
{
    public Vector3Int voxelDimensions;
    public ComputeShader march3D;
    public ComputeShader trianglesToMesh;
    public float solidnessThreshold;
    private float lastSolidnessThreshold = Mathf.Infinity;
    private IMeshGenerator generator;
    private DefaultTerrainGenerator terrainGenerator;

    private void RegenerateTerrain()
    {
        if (lastSolidnessThreshold != solidnessThreshold)
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            Mesh mesh = filter.sharedMesh;
            terrainGenerator = new DefaultTerrainGenerator();
            generator.GenerateMeshFromData(ref mesh, terrainGenerator.GenerateTerrain(
                new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z - 1),
                new Vector3(transform.position.x, transform.position.y, transform.position.z), voxelDimensions), solidnessThreshold);
            filter.sharedMesh = mesh;
            lastSolidnessThreshold = solidnessThreshold;
            transform.localScale = new Vector3(1.0f / voxelDimensions.x, 1.0f / voxelDimensions.y, 1.0f / voxelDimensions.y);
        }
    }

    public void Start()
    {
        MeshGeneratorFactory factory = new MeshGeneratorFactory(march3D, trianglesToMesh);
        generator = factory.CreateGenerator(MeshGeneratorFactory.EGeneratorType.MarchingCubes, voxelDimensions);
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = new Mesh();
    }

    public void Update()
    {
        RegenerateTerrain();
    }
}

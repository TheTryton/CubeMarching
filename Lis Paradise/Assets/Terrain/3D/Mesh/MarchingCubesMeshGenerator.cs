using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MarchingCubesMeshGenerator : IMeshGenerator
{
    private Vector3Int VoxelDimensions { get; }
    private ComputeShader MarchingCubesAlgorithm { get; }
    private ComputeShader TriangleToMeshTransformer { get; }

    private ComputeBuffer solidnessBuffer;
    private ComputeBuffer textureCoeffBuffer;
    private ComputeBuffer triangleBuffer;
    private ComputeBuffer triCountBuffer;
    private ComputeBuffer verticesBuffer;
    private ComputeBuffer indicesBuffer;
    private ComputeBuffer uvsBuffer;

    public MarchingCubesMeshGenerator(Vector3Int voxelDimensions, ComputeShader marchingCubesAlgorithm, ComputeShader triangleToMeshTransformer)
    {
        if (marchingCubesAlgorithm == null || triangleToMeshTransformer == null)
        {
            throw new System.ArgumentNullException();
        }

        VoxelDimensions = voxelDimensions;
        MarchingCubesAlgorithm = marchingCubesAlgorithm;
        TriangleToMeshTransformer = triangleToMeshTransformer;

        int pointsCount = PointsCount();
        int voxelCount = VoxelsCount();

        solidnessBuffer = new ComputeBuffer(pointsCount, sizeof(float));
        textureCoeffBuffer = new ComputeBuffer(pointsCount, sizeof(float));
        triangleBuffer = new ComputeBuffer(voxelCount * 5, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

        verticesBuffer = new ComputeBuffer(voxelCount * 5, sizeof(float) * 3);
        indicesBuffer = new ComputeBuffer(voxelCount * 5, sizeof(int));
        uvsBuffer = new ComputeBuffer(voxelCount * 5, sizeof(float) * 2);
    }

    private int PointsCount()
    {
        return (VoxelDimensions.x + 1) * (VoxelDimensions.y + 1) * (VoxelDimensions.z + 1);
    }

    private int VoxelsCount()
    {
        return VoxelDimensions.x * VoxelDimensions.y * VoxelDimensions.z;
    }

    private void GenerateTriangles(TerrainData terrainData, float solidnessThreshold)
    {
        int pointsCount = PointsCount();
        int voxelCount = VoxelsCount();

        solidnessBuffer.SetData(terrainData.Solidness, 0, 0, pointsCount);
        textureCoeffBuffer.SetData(terrainData.TextureCoeff, 0, 0, pointsCount);

        triangleBuffer.SetCounterValue(0);

        int kernel = MarchingCubesAlgorithm.FindKernel("March3D");

        if (kernel == -1)
        {
            throw new System.InvalidProgramException();
        }

        MarchingCubesAlgorithm.SetInt("VoxelsSizeX", terrainData.VoxelDimensions.x);
        MarchingCubesAlgorithm.SetInt("VoxelsSizeY", terrainData.VoxelDimensions.y);
        MarchingCubesAlgorithm.SetInt("VoxelsSizeZ", terrainData.VoxelDimensions.z);

        MarchingCubesAlgorithm.SetInt("DataPointsSizeX", terrainData.VoxelDimensions.x + 1);
        MarchingCubesAlgorithm.SetInt("DataPointsSizeY", terrainData.VoxelDimensions.y + 1);
        MarchingCubesAlgorithm.SetInt("DataPointsSizeZ", terrainData.VoxelDimensions.z + 1);

        MarchingCubesAlgorithm.SetFloat("SolidnessThreshold", solidnessThreshold);

        MarchingCubesAlgorithm.SetBuffer(kernel, "outTriangles", triangleBuffer);
        MarchingCubesAlgorithm.SetBuffer(kernel, "inSolidnessDataPoints", solidnessBuffer);
        MarchingCubesAlgorithm.SetBuffer(kernel, "inTextureCoeffDataPoints", textureCoeffBuffer);

        uint threadGroupX;
        uint threadGroupY;
        uint threadGroupZ;

        MarchingCubesAlgorithm.GetKernelThreadGroupSizes(kernel, out threadGroupX, out threadGroupY, out threadGroupZ);

        MarchingCubesAlgorithm.Dispatch(kernel,
            (int)Mathf.Ceil(terrainData.VoxelDimensions.x / (float)threadGroupX),
            (int)Mathf.Ceil(terrainData.VoxelDimensions.y / (float)threadGroupY),
            (int)Mathf.Ceil(terrainData.VoxelDimensions.z / (float)threadGroupZ)
        );
    }
    private void TransformTrianglesToMesh(int triCount)
    {
        int kernel = TriangleToMeshTransformer.FindKernel("TransformTrianglesToMesh");

        if (kernel == -1)
        {
            throw new System.InvalidProgramException();
        }

        uint threadGroupX;
        uint threadGroupY;
        uint threadGroupZ;

        TriangleToMeshTransformer.GetKernelThreadGroupSizes(kernel, out threadGroupX, out threadGroupY, out threadGroupZ);

        TriangleToMeshTransformer.SetInt("inTrianglesCount", triCount);
        TriangleToMeshTransformer.SetBuffer(kernel, "inTriangles", triangleBuffer);
        TriangleToMeshTransformer.SetBuffer(kernel, "outVertices", verticesBuffer);
        TriangleToMeshTransformer.SetBuffer(kernel, "outIndices", indicesBuffer);
        TriangleToMeshTransformer.SetBuffer(kernel, "outUvs", uvsBuffer);

        TriangleToMeshTransformer.Dispatch(kernel,
            (int)Mathf.Max(Mathf.Ceil(triCount / (float)threadGroupX), 1.0f),
            1,
            1
        );
    }

    public void GenerateMeshFromData(ref Mesh mesh, TerrainData terrainData, float solidnessThreshold)
    {
        if (terrainData.VoxelDimensions != terrainData.VoxelDimensions)
        {
            throw new System.ArgumentNullException();
        }

        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.MarkDynamic();
        }
        else
        {
            mesh.Clear();
        }

        GenerateTriangles(terrainData, solidnessThreshold);

        ComputeBuffer.CopyCount(triangleBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int triCount = triCountArray[0];

        TransformTrianglesToMesh(triCount);

        Vector3[] vertices = new Vector3[triCount * 3];
        Vector2[] uvs = new Vector2[triCount * 3];
        int[] triangles = new int[triCount * 3];

        verticesBuffer.GetData(vertices, 0, 0, triCount * 3);
        indicesBuffer.GetData(triangles, 0, 0, triCount * 3);
        uvsBuffer.GetData(uvs, 0, 0, triCount * 3);

        mesh.vertices = vertices;

        mesh.triangles = triangles;

        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }
}
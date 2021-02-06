using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneratorFactory
{
    public ComputeShader MarchingCubesAlgorithm { get; }
    public ComputeShader TriangleToMeshTransformer { get; }

    public enum EGeneratorType
    {
        MarchingCubes
    }

    public MeshGeneratorFactory(ComputeShader marchingCubesAlgorithm, ComputeShader triangleToMeshTransformer)
    {
        if (marchingCubesAlgorithm == null || triangleToMeshTransformer == null)
        {
            throw new System.ArgumentNullException();
        }

        MarchingCubesAlgorithm = marchingCubesAlgorithm;
        TriangleToMeshTransformer = triangleToMeshTransformer;
    }

    public IMeshGenerator CreateGenerator(EGeneratorType generatorType, Vector3Int voxelDimensions)
    {
        switch (generatorType)
        {
            case EGeneratorType.MarchingCubes:
                return new MarchingCubesMeshGenerator(voxelDimensions, MarchingCubesAlgorithm, TriangleToMeshTransformer);
        }

        return null;
    }
}

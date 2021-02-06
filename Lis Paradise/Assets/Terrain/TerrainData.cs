using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct TerrainData
{
    public Vector3Int VoxelDimensions { get; }

    public float[] Solidness { get; }
    public float[] TextureCoeff { get; }

    public TerrainData(Vector3Int voxelDimensions, float[] solidness, float[] textureCoeff)
    {
        VoxelDimensions = voxelDimensions;
        Solidness = solidness;
        TextureCoeff = textureCoeff;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeshGenerator
{
    public void GenerateMeshFromData(ref Mesh mesh, TerrainData terrainData, float solidnessThreshold);
}

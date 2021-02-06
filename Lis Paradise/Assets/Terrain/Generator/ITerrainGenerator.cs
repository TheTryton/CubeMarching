using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITerrainGenerator
{
    public TerrainData GenerateTerrain(Vector3 bottomLeftBack, Vector3 topRightFront, Vector3Int dimensions);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GNoise;

public class DefaultTerrainGenerator : ITerrainGenerator
{
    private GNoise.Generators.RidgedMultifractal Perlin { get; }
    private GNoise.Generators.Billow Billow { get; }
    public DefaultTerrainGenerator()
    {
        Perlin = new GNoise.Generators.RidgedMultifractal();
        Billow = new GNoise.Generators.Billow();

        Perlin.ModuleComputationTarget = ModuleComputationTarget.GPU;
        Billow.ModuleComputationTarget = ModuleComputationTarget.GPU;

        Perlin.GPUTarget = NoiseModule.AvailableGPUs[0];
        Billow.GPUTarget = NoiseModule.AvailableGPUs[0];

        Perlin.Frequency = 0.5f;
        Perlin.Lacunarity = 2.0f;
        Billow.Frequency = 0.5f;
    }
    public TerrainData GenerateTerrain(Vector3 bottomLeftBack, Vector3 topRightFront, Vector3Int dimensions)
    {
        Range3f range = new Range3f();
        range.x.x = bottomLeftBack.x;
        range.x.y = topRightFront.x;

        range.y.x = bottomLeftBack.y;
        range.y.y = topRightFront.y;

        range.z.x = bottomLeftBack.z;
        range.z.y = topRightFront.z;

        Precision3 precision = new Precision3();
        precision.x = (ulong)(dimensions.x + 1);
        precision.y = (ulong)(dimensions.y + 1);
        precision.z = (ulong)(dimensions.z + 1);

        float[] solidness = Perlin.ComputeFlat(range, precision);
        float[] textureCoeff = Billow.ComputeFlat(range, precision);
        return new TerrainData(dimensions, solidness, textureCoeff);
    }
}

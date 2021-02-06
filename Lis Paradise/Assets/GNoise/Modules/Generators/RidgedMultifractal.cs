using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class RidgedMultifractal : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructRidgedMultifractalGeneratorModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetRidgedMultifractalFrequency(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetRidgedMultifractalFrequency(IntPtr module, float frequency);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetRidgedMultifractalLacunarity(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetRidgedMultifractalLacunarity(IntPtr module, float lacunarity);
        [DllImport(GNoiseImport.DllName)]
        static private extern uint GetRidgedMultifractalOctaveCount(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetRidgedMultifractalOctaveCount(IntPtr module, uint octaveCount);
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetRidgedMultifractalSeed(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetRidgedMultifractalSeed(IntPtr module, int seed);
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetRidgedMultifractalQuality(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetRidgedMultifractalQuality(IntPtr module, int seed);
        #endregion

        public RidgedMultifractal()
        {
            nativeObject = ConstructRidgedMultifractalGeneratorModule();
        }

        public float Frequency
        {
            get
            {
                return GetRidgedMultifractalFrequency(nativeObject);
            }
            set
            {
                SetRidgedMultifractalFrequency(nativeObject, value);
            }
        }
        public float Lacunarity
        {
            get
            {
                return GetRidgedMultifractalLacunarity(nativeObject);
            }
            set
            {
                SetRidgedMultifractalLacunarity(nativeObject, value);
            }
        }
        public uint OctaveCount
        {
            get
            {
                return GetRidgedMultifractalOctaveCount(nativeObject);
            }
            set
            {
                SetRidgedMultifractalOctaveCount(nativeObject, value);
            }
        }
        public int Seed
        {
            get
            {
                return GetRidgedMultifractalSeed(nativeObject);
            }
            set
            {
                SetRidgedMultifractalSeed(nativeObject, value);
            }
        }
        public NoiseQuality Quality
        {
            get
            {
                return NoiseQualityMethods.FromInt(GetRidgedMultifractalQuality(nativeObject));
            }
            set
            {
                SetRidgedMultifractalQuality(nativeObject, NoiseQualityMethods.ToInt(value));
            }
        }
    }
}

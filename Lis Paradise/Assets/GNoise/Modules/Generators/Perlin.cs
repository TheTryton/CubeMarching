using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class Perlin : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructPerlinGeneratorModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetPerlinFrequency(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetPerlinFrequency(IntPtr module, float frequency);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetPerlinLacunarity(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetPerlinLacunarity(IntPtr module, float lacunarity);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetPerlinPersistence(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetPerlinPersistence(IntPtr module, float persistence);
        [DllImport(GNoiseImport.DllName)]
        static private extern uint GetPerlinOctaveCount(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetPerlinOctaveCount(IntPtr module, uint octaveCount);
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetPerlinSeed(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetPerlinSeed(IntPtr module, int seed);
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetPerlinQuality(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetPerlinQuality(IntPtr module, int seed);
        #endregion

        public Perlin()
        {
            nativeObject = ConstructPerlinGeneratorModule();
        }

        public float Frequency
        {
            get
            {
                return GetPerlinFrequency(nativeObject);
            }
            set
            {
                SetPerlinFrequency(nativeObject, value);
            }
        }
        public float Lacunarity
        {
            get
            {
                return GetPerlinLacunarity(nativeObject);
            }
            set
            {
                SetPerlinLacunarity(nativeObject, value);
            }
        }
        public float Persistence
        {
            get
            {
                return GetPerlinPersistence(nativeObject);
            }
            set
            {
                SetPerlinPersistence(nativeObject, value);
            }
        }
        public uint OctaveCount
        {
            get
            {
                return GetPerlinOctaveCount(nativeObject);
            }
            set
            {
                SetPerlinOctaveCount(nativeObject, value);
            }
        }
        public int Seed
        {
            get
            {
                return GetPerlinSeed(nativeObject);
            }
            set
            {
                SetPerlinSeed(nativeObject, value);
            }
        }
        public NoiseQuality Quality
        {
            get
            {
                return NoiseQualityMethods.FromInt(GetPerlinQuality(nativeObject));
            }
            set
            {
                SetPerlinQuality(nativeObject, NoiseQualityMethods.ToInt(value));
            }
        }
    }
}

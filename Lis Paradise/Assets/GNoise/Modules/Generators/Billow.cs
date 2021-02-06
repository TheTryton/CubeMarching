using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class Billow : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructBillowGeneratorModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetBillowFrequency(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetBillowFrequency(IntPtr module, float frequency);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetBillowLacunarity(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetBillowLacunarity(IntPtr module, float lacunarity);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetBillowPersistence(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetBillowPersistence(IntPtr module, float persistence);
        [DllImport(GNoiseImport.DllName)]
        static private extern uint GetBillowOctaveCount(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetBillowOctaveCount(IntPtr module, uint octaveCount);
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetBillowSeed(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetBillowSeed(IntPtr module, int seed);
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetBillowQuality(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetBillowQuality(IntPtr module, int seed);
        #endregion

        public Billow()
        {
            nativeObject = ConstructBillowGeneratorModule();
        }

        public float Frequency
        {
            get
            {
                return GetBillowFrequency(nativeObject);
            }
            set
            {
                SetBillowFrequency(nativeObject, value);
            }
        }
        public float Lacunarity
        {
            get
            {
                return GetBillowLacunarity(nativeObject);
            }
            set
            {
                SetBillowLacunarity(nativeObject, value);
            }
        }
        public float Persistence
        {
            get
            {
                return GetBillowPersistence(nativeObject);
            }
            set
            {
                SetBillowPersistence(nativeObject, value);
            }
        }
        public uint OctaveCount
        {
            get
            {
                return GetBillowOctaveCount(nativeObject);
            }
            set
            {
                SetBillowOctaveCount(nativeObject, value);
            }
        }
        public int Seed
        {
            get
            {
                return GetBillowSeed(nativeObject);
            }
            set
            {
                SetBillowSeed(nativeObject, value);
            }
        }
        public NoiseQuality Quality
        {
            get
            {
                return NoiseQualityMethods.FromInt(GetBillowQuality(nativeObject));
            }
            set
            {
                SetBillowQuality(nativeObject, NoiseQualityMethods.ToInt(value));
            }
        }
    }
}

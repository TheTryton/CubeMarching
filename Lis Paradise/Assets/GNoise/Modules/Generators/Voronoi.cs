using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class Voronoi : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructVoronoiGeneratorModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetVoronoiFrequency(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetVoronoiFrequency(IntPtr module, float frequency);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetVoronoiDisplacement(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetVoronoiDisplacement(IntPtr module, float displacement);
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetVoronoiSeed(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetVoronoiSeed(IntPtr module, int seed);
        [DllImport(GNoiseImport.DllName)]
        static private extern bool GetVoronoiUseDistance(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetVoronoiUseDistance(IntPtr module, bool useDistance);
        #endregion

        public Voronoi()
        {
            nativeObject = ConstructVoronoiGeneratorModule();
        }

        public float Frequency
        {
            get
            {
                return GetVoronoiFrequency(nativeObject);
            }
            set
            {
                SetVoronoiFrequency(nativeObject, value);
            }
        }
        public float Displacement
        {
            get
            {
                return GetVoronoiDisplacement(nativeObject);
            }
            set
            {
                SetVoronoiDisplacement(nativeObject, value);
            }
        }
        public int Seed
        {
            get
            {
                return GetVoronoiSeed(nativeObject);
            }
            set
            {
                SetVoronoiSeed(nativeObject, value);
            }
        }
        public bool UseDistance
        {
            get
            {
                return GetVoronoiUseDistance(nativeObject);
            }
            set
            {
                SetVoronoiUseDistance(nativeObject, value);
            }
        }
    }
}

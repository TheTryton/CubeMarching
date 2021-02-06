using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class Spheres : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructSpheresGeneratorModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetSpheresFrequency(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetSpheresFrequency(IntPtr module, float frequency);
        #endregion

        public Spheres()
        {
            nativeObject = ConstructSpheresGeneratorModule();
        }

        public float Frequency
        {
            get
            {
                return GetSpheresFrequency(nativeObject);
            }
            set
            {
                SetSpheresFrequency(nativeObject, value);
            }
        }
    }
}

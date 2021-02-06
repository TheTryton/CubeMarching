using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class Cylinders : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructCylindersGeneratorModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetCylindersFrequency(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetCylindersFrequency(IntPtr module, float frequency);
        #endregion

        public Cylinders()
        {
            nativeObject = ConstructCylindersGeneratorModule();
        }

        public float Frequency
        {
            get
            {
                return GetCylindersFrequency(nativeObject);
            }
            set
            {
                SetCylindersFrequency(nativeObject, value);
            }
        }
    }
}

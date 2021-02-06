using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class Const : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructConstGeneratorModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetConstValue(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetConstValue(IntPtr module, float constValue);
        #endregion

        public Const()
        {
            nativeObject = ConstructConstGeneratorModule();
        }

        public float ConstValue
        {
            get
            {
                return GetConstValue(nativeObject);
            }
            set
            {
                SetConstValue(nativeObject, value);
            }
        }
    }
}

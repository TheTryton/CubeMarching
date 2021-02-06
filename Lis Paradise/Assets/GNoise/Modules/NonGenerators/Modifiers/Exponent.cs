using System;
using System.Runtime.InteropServices;

namespace GNoise.Modifiers
{
    public class Exponent : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructExponentModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetExponentModuleExponent(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetExponentModuleExponent(IntPtr module, float exponent);
        #endregion

        public Exponent()
        {
            nativeObject = ConstructExponentModule();
        }

        public float ExponentOfExponentiation
        {
            get
            {
                return GetExponentModuleExponent(nativeObject);
            }
            set
            {
                SetExponentModuleExponent(nativeObject, value);
            }
        }
    }
}

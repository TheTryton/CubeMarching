using System;
using System.Runtime.InteropServices;

namespace GNoise.Modifiers
{
    public class Base : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructBaseModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetBaseModuleBase(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetBaseModuleBase(IntPtr module, float baseOfExponentiation);
        #endregion

        public Base()
        {
            nativeObject = ConstructBaseModule();
        }

        public float BaseOfExponentiation
        {
            get
            {
                return GetBaseModuleBase(nativeObject);
            }
            set
            {
                SetBaseModuleBase(nativeObject, value);
            }
        }
    }
}

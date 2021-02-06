using System;
using System.Runtime.InteropServices;

namespace GNoise.Modifiers
{
    public class Clamp : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructClampModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetClampLowerBound(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetClampLowerBound(IntPtr module, float lowerBound);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetClampUpperBound(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetClampUpperBound(IntPtr module, float upperBound);
        #endregion

        public Clamp()
        {
            nativeObject = ConstructClampModule();
        }

        public float LowerBound
        {
            get
            {
                return GetClampLowerBound(nativeObject);
            }
            set
            {
                SetClampLowerBound(nativeObject, value);
            }
        }
        public float UpperBound
        {
            get
            {
                return GetClampUpperBound(nativeObject);
            }
            set
            {
                SetClampUpperBound(nativeObject, value);
            }
        }
    }
}

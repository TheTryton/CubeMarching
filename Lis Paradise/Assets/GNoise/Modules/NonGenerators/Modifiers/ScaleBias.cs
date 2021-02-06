using System;
using System.Runtime.InteropServices;

namespace GNoise.Modifiers
{
    public class ScaleBias : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructScaleBiasModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetScaleBiasScale(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetScaleBiasScale(IntPtr module, float scale);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetScaleBiasBias(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetScaleBiasBias(IntPtr module, float bias);
        #endregion

        public ScaleBias()
        {
            nativeObject = ConstructScaleBiasModule();
        }

        public float Scale
        {
            get
            {
                return GetScaleBiasScale(nativeObject);
            }
            set
            {
                SetScaleBiasScale(nativeObject, value);
            }
        }
        public float Bias
        {
            get
            {
                return GetScaleBiasBias(nativeObject);
            }
            set
            {
                SetScaleBiasBias(nativeObject, value);
            }
        }
    }
}

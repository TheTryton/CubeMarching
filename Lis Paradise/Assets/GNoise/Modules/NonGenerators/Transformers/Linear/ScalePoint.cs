using System;
using System.Runtime.InteropServices;

namespace GNoise.Transformers.Linear
{
    public class ScalePoint : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructScalePointModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetXScale(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetXScale(IntPtr modul, float scale);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetYScale(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetYScale(IntPtr modul, float scale);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetZScale(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetZScale(IntPtr modul, float scale);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetWScale(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetWScale(IntPtr modul, float scale);
        #endregion

        public ScalePoint()
        {
            nativeObject = ConstructScalePointModule();
        }

        public float ScaleX
        {
            get
            {
                return GetXScale(nativeObject);
            }
            set
            {
                SetXScale(nativeObject, value);
            }
        }
        public float ScaleY
        {
            get
            {
                return GetYScale(nativeObject);
            }
            set
            {
                SetYScale(nativeObject, value);
            }
        }
        public float ScaleZ
        {
            get
            {
                return GetZScale(nativeObject);
            }
            set
            {
                SetZScale(nativeObject, value);
            }
        }
        public float ScaleW
        {
            get
            {
                return GetWScale(nativeObject);
            }
            set
            {
                SetWScale(nativeObject, value);
            }
        }

        public Vector4f Scale
        {
            get
            {
                Vector4f vector = new Vector4f();
                vector.x = ScaleX;
                vector.y = ScaleY;
                vector.z = ScaleZ;
                vector.w = ScaleW;
                return vector;
            }
            set
            {
                ScaleX = value.x;
                ScaleY = value.y;
                ScaleZ = value.z;
                ScaleW = value.w;
            }
        }
    }
}

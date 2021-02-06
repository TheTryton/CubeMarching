using System;
using System.Runtime.InteropServices;

namespace GNoise.Transformers.Linear
{
    public class RotatePoint : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructRotatePointModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetXYPlaneRotation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetXYPlaneRotation(IntPtr modul, float rotation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetYZPlaneRotation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetYZPlaneRotation(IntPtr modul, float rotation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetZXPlaneRotation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetZXPlaneRotation(IntPtr modul, float rotation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetXWPlaneRotation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetXWPlaneRotation(IntPtr modul, float rotation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetYWPlaneRotation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetYWPlaneRotation(IntPtr modul, float rotation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetZWPlaneRotation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetZWPlaneRotation(IntPtr modul, float rotation);
        #endregion

        public RotatePoint()
        {
            nativeObject = ConstructRotatePointModule();
        }

        public float XYPlaneRotation
        {
            get
            {
                return GetXYPlaneRotation(nativeObject);
            }
            set
            {
                SetXYPlaneRotation(nativeObject, value);
            }
        }
        public float YZPlaneRotation
        {
            get
            {
                return GetYZPlaneRotation(nativeObject);
            }
            set
            {
                SetYZPlaneRotation(nativeObject, value);
            }
        }
        public float ZXPlaneRotation
        {
            get
            {
                return GetZXPlaneRotation(nativeObject);
            }
            set
            {
                SetZXPlaneRotation(nativeObject, value);
            }
        }
        public float XWPlaneRotation
        {
            get
            {
                return GetXWPlaneRotation(nativeObject);
            }
            set
            {
                SetXWPlaneRotation(nativeObject, value);
            }
        }
        public float YWPlaneRotation
        {
            get
            {
                return GetYWPlaneRotation(nativeObject);
            }
            set
            {
                SetYWPlaneRotation(nativeObject, value);
            }
        }
        public float ZWPlaneRotation
        {
            get
            {
                return GetZWPlaneRotation(nativeObject);
            }
            set
            {
                SetZWPlaneRotation(nativeObject, value);
            }
        }
    }
}

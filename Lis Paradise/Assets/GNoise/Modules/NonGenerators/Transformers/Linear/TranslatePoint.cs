using System;
using System.Runtime.InteropServices;

namespace GNoise.Transformers.Linear
{
    public class TranslatePoint : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructTranslatePointModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetXTranslation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetXTranslation(IntPtr modul, float translation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetYTranslation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetYTranslation(IntPtr modul, float translation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetZTranslation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetZTranslation(IntPtr modul, float translation);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetWTranslation(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetWTranslation(IntPtr modul, float translation);
        #endregion

        public TranslatePoint()
        {
            nativeObject = ConstructTranslatePointModule();
        }

        public float TranslationX
        {
            get
            {
                return GetXTranslation(nativeObject);
            }
            set
            {
                SetXTranslation(nativeObject, value);
            }
        }
        public float TranslationY
        {
            get
            {
                return GetYTranslation(nativeObject);
            }
            set
            {
                SetYTranslation(nativeObject, value);
            }
        }
        public float TranslationZ
        {
            get
            {
                return GetZTranslation(nativeObject);
            }
            set
            {
                SetZTranslation(nativeObject, value);
            }
        }
        public float TranslationW
        {
            get
            {
                return GetWTranslation(nativeObject);
            }
            set
            {
                SetWTranslation(nativeObject, value);
            }
        }

        public Vector4f Translation
        {
            get
            {
                Vector4f vector = new Vector4f();
                vector.x = TranslationX;
                vector.y = TranslationY;
                vector.z = TranslationZ;
                vector.w = TranslationW;
                return vector;
            }
            set
            {
                TranslationX = value.x;
                TranslationY = value.y;
                TranslationZ = value.z;
                TranslationW = value.w;
            }
        }
    }
}

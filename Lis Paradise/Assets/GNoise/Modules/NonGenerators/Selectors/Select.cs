using System;
using System.Runtime.InteropServices;

namespace GNoise.Selectors
{
    public class Select : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructSelectModule();

        [DllImport(GNoiseImport.DllName)]
        static private extern float GetSelectFalloff(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetSelectFalloff(IntPtr module, float falloff);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetSelectMin(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetSelectMin(IntPtr module, float min);
        [DllImport(GNoiseImport.DllName)]
        static private extern float GetSelectMax(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void SetSelectMax(IntPtr module, float max);
        #endregion

        public Select()
        {
            nativeObject = ConstructSelectModule();
        }

        public float Falloff
        {
            get
            {
                return GetSelectFalloff(nativeObject);
            }
            set
            {
                SetSelectFalloff(nativeObject, value);
            }
        }
        public float Min
        {
            get
            {
                return GetSelectMin(nativeObject);
            }
            set
            {
                SetSelectMin(nativeObject, value);
            }
        }
        public float Max
        {
            get
            {
                return GetSelectMax(nativeObject);
            }
            set
            {
                SetSelectMax(nativeObject, value);
            }
        }

        public NoiseModule ControlModule
        {
            get
            {
                return GetInputModule(0);
            }
            set
            {
                SetInputModule(0, value);
            }
        }
        public NoiseModule FirstInputModule
        {
            get
            {
                return GetInputModule(1);
            }
            set
            {
                SetInputModule(1, value);
            }
        }
        public NoiseModule SecondInputModule
        {
            get
            {
                return GetInputModule(2);
            }
            set
            {
                SetInputModule(2, value);
            }
        }
    }
}

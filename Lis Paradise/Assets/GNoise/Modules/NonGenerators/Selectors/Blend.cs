using System;
using System.Runtime.InteropServices;

namespace GNoise.Selectors
{
    public class Blend : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructBlendModule();
        #endregion

        public Blend()
        {
            nativeObject = ConstructBlendModule();
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

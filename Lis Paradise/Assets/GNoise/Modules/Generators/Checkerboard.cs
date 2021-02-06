using System;
using System.Runtime.InteropServices;

namespace GNoise.Generators
{
    public class Checkerboard : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructCheckerboardGeneratorModule();
        #endregion

        public Checkerboard()
        {
            nativeObject = ConstructCheckerboardGeneratorModule();
        }
    }
}

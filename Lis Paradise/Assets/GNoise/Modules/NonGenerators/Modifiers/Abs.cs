using System;
using System.Runtime.InteropServices;

namespace GNoise.Modifiers
{
    public class Abs : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructAbsModule();
        #endregion

        public Abs()
        {
            nativeObject = ConstructAbsModule();
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace GNoise.Combiners
{
    public class Max : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructMaxModule();
        #endregion

        public Max()
        {
            nativeObject = ConstructMaxModule();
        }
    }
}

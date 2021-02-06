using System;
using System.Runtime.InteropServices;

namespace GNoise.Combiners
{
    public class Min : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructMinModule();
        #endregion

        public Min()
        {
            nativeObject = ConstructMinModule();
        }
    }
}

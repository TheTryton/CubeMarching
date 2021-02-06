using System;
using System.Runtime.InteropServices;

namespace GNoise.Combiners
{
    public class Divide : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructDivideModule();
        #endregion

        public Divide()
        {
            nativeObject = ConstructDivideModule();
        }
    }
}

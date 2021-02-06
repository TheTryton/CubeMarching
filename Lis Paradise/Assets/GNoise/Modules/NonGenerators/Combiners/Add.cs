using System;
using System.Runtime.InteropServices;

namespace GNoise.Combiners
{
    public class Add : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructAddModule();
        #endregion

        public Add()
        {
            nativeObject = ConstructAddModule();
        }
    }
}

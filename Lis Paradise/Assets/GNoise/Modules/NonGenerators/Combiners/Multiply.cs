using System;
using System.Runtime.InteropServices;

namespace GNoise.Combiners
{
    public class Multiply : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructMultiplyModule();
        #endregion

        public Multiply()
        {
            nativeObject = ConstructMultiplyModule();
        }
    }
}

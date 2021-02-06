using System;
using System.Runtime.InteropServices;

namespace GNoise.Combiners
{
    public class Subtract : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructSubtractModule();
        #endregion

        public Subtract()
        {
            nativeObject = ConstructSubtractModule();
        }
    }
}

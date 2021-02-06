using System;
using System.Runtime.InteropServices;

namespace GNoise.Combiners
{
    public class Power : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructPowerModule();
        #endregion

        public Power()
        {
            nativeObject = ConstructPowerModule();
        }
    }
}

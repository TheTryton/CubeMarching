﻿using System;
using System.Runtime.InteropServices;

namespace GNoise.Transformers
{
    public class Displace : NonGeneratorNoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ConstructDisplaceModule();
        #endregion

        public Displace()
        {
            nativeObject = ConstructDisplaceModule();
        }

        public NoiseModule ComputationModule
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
        public NoiseModule XDisplaceModule
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
        public NoiseModule YDisplaceModule
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
        public NoiseModule ZDisplaceModule
        {
            get
            {
                return GetInputModule(3);
            }
            set
            {
                SetInputModule(3, value);
            }
        }
        public NoiseModule WDisplaceModule
        {
            get
            {
                return GetInputModule(4);
            }
            set
            {
                SetInputModule(4, value);
            }
        }
    }
}

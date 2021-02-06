using System;
using System.Runtime.InteropServices;

namespace GNoise
{
    public class NonGeneratorNoiseModule : NoiseModule
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern int GetNonGeneratorInputModulesCount(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr GetNonGeneratorInputModule(IntPtr module, int index);
        [DllImport(GNoiseImport.DllName)]
        static private extern bool SetNonGeneratorInputModule(IntPtr module, int index, IntPtr inputModule);
        #endregion

        private NoiseModule[] inputModules;

        public NoiseModule[] InputModules
        {
            get
            {
                if(inputModules == null)
                {
                    inputModules = new NoiseModule[InputModulesCount];
                }

                return inputModules;
            }
            set
            {
                if (inputModules == null)
                {
                    inputModules = new NoiseModule[InputModulesCount];
                }

                for (int i = 0; i < Math.Min(InputModulesCount, value.Length); i++)
                {
                    if(!SetInputModule(i, value[i]))
                    {
                        throw new ArgumentException("Incorrect module passed");
                    }
                }
            }
        }

        public int InputModulesCount
        {
            get
            {
                return GetNonGeneratorInputModulesCount(nativeObject);
            }
        }

        public bool SetInputModule(int index, NoiseModule module)
        {
            if (inputModules == null)
            {
                inputModules = new NoiseModule[InputModulesCount];
            }

            inputModules[index] = module;

            return SetNonGeneratorInputModule(nativeObject, index, GetPtrFromModule(module));
        }

        public NoiseModule GetInputModule(int index)
        {
            if (inputModules == null)
            {
                inputModules = new NoiseModule[InputModulesCount];
            }

            return inputModules[index];
        }
    }
}

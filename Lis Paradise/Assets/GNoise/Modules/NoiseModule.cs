using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GNoise
{
    public static class GNoiseImport
    {
        public const string DllName = @"gnoise.dll";
    }

    public enum ModuleComputationTarget
    {
        SingleThreadCPU,
        MultiThreadCPU,
        GPU
    }

    public enum ModuleType
    {
        Generator,
        Modifier,
        Combiner,
        Selector,
        Transformer,
        Miscellaneous,
        Other
    }

    public enum NoiseQuality
    {
        Fast,
        Standard,
        Best
    }

    static class NoiseQualityMethods
    {
        public static int ToInt(NoiseQuality quality)
        {
            switch (quality)
            {
                default:
                case NoiseQuality.Fast:
                    return 0;
                case NoiseQuality.Standard:
                    return 1;
                case NoiseQuality.Best:
                    return 2;
            }
        }

        public static NoiseQuality FromInt(int quality)
        {
            switch (quality)
            {
                default:
                case 0:
                    return NoiseQuality.Fast;
                case 1:
                    return NoiseQuality.Standard;
                case 2:
                    return NoiseQuality.Best;
            }
        }
    }

    public struct Vector1f
    {
        public float x;
    }
    public struct Vector2f
    {
        public float x, y;
    }
    public struct Vector3f
    {
        public float x, y, z;
    }
    public struct Vector4f
    {
        public float x, y, z, w;
    }

    public struct Range1f
    {
        public Vector2f x;
    }
    public struct Range2f
    {
        public Vector2f x, y;
    }
    public struct Range3f
    {
        public Vector2f x, y, z;
    }
    public struct Range4f
    {
        public Vector2f x, y, z, w;
    }

    public struct Precision1
    {
        public ulong x;
    }
    public struct Precision2
    {
        public ulong x, y;
    }
    public struct Precision3
    {
        public ulong x, y, z;
    }
    public struct Precision4
    {
        public ulong x, y, z, w;
    }

    public class NoiseModule : IDisposable
    {
        #region native code
        [DllImport(GNoiseImport.DllName)]
        static private extern void DestroyModule(IntPtr module);

        [DllImport(GNoiseImport.DllName)]
        static private extern float ModuleCompute1(IntPtr module, Vector1f point);
        [DllImport(GNoiseImport.DllName)]
        static private extern float ModuleCompute2(IntPtr module, Vector2f point);
        [DllImport(GNoiseImport.DllName)]
        static private extern float ModuleCompute3(IntPtr module, Vector3f point);
        [DllImport(GNoiseImport.DllName)]
        static private extern float ModuleCompute4(IntPtr module, Vector4f point);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute5(IntPtr module, Vector1f[] points, float[] returnArray, int size);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute6(IntPtr module, Vector2f[] points, float[] returnArray, int size);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute7(IntPtr module, Vector3f[] points, float[] returnArray, int size);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute8(IntPtr module, Vector4f[] points, float[] returnArray, int size);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute9(IntPtr module, Range1f range, Precision1 precision, float[] returnArray);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute10(IntPtr module, Range2f range, Precision2 precision, float[] returnArray);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute11(IntPtr module, Range3f range, Precision3 precision, float[] returnArray);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleCompute12(IntPtr module, Range4f range, Precision4 precision, float[] returnArray);

        [DllImport(GNoiseImport.DllName)]
        static private extern int GetModuleType(IntPtr module);

        [DllImport(GNoiseImport.DllName)]
        static private extern int ModuleGetComputationTarget(IntPtr module);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleSetComputationTarget(IntPtr module, int target);

        [DllImport(GNoiseImport.DllName)]
        static private extern float ModuleGetMultithreadAffinity(IntPtr module, ref bool result);
        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleSetMultithreadAffinity(IntPtr module, float affinity, ref bool result);

        [DllImport(GNoiseImport.DllName)]
        static private extern int GetAvailableGPUCount();
        [DllImport(GNoiseImport.DllName)]
        static private extern void GetAvailableGPUs(IntPtr[] devices);

        [DllImport(GNoiseImport.DllName)]
        static private extern void GetGPUName(IntPtr deviceId, int availableSize, sbyte[] name, ref int fullSize);

        [DllImport(GNoiseImport.DllName)]
        static private extern void ModuleSetGPUTarget(IntPtr module, IntPtr deviceId, ref bool result);
        [DllImport(GNoiseImport.DllName)]
        static private extern IntPtr ModuleGetGPUTarget(IntPtr module, ref bool result);
        #endregion

        protected NoiseModule()
        {

        }

        protected static IntPtr GetPtrFromModule(NoiseModule module)
        {
            return module.nativeObject;
        }

        protected IntPtr nativeObject;

        private static IntPtr[] availableGPUs;
        private static string[] availableGPUNames;

        private static string RetrieveGPUName(IntPtr deviceId)
        {
            int fullSize = 0;
            GetGPUName(deviceId, 0, null, ref fullSize);
            sbyte[] data = new sbyte[fullSize];
            GetGPUName(deviceId, fullSize, data, ref fullSize);
            string gpuName = "";
            for (int i = 0; i < fullSize; i++)
            {
                gpuName += (char)data[i];
            }
            return gpuName;
        }

        public static string[] AvailableGPUs
        {
            get
            {
                if (availableGPUs == null)
                {
                    availableGPUs = new IntPtr[GetAvailableGPUCount()];
                    GetAvailableGPUs(availableGPUs);

                    availableGPUNames = new string[availableGPUs.Length];
                    for (int i = 0; i < availableGPUs.Length; i++)
                    {
                        availableGPUNames[i] = RetrieveGPUName(availableGPUs[i]);
                    }
                }

                return availableGPUNames;
            }
        }

        public virtual float Compute(Vector1f point)
        {
            return ModuleCompute1(nativeObject, point);
        }
        public virtual float Compute(Vector2f point)
        {
            return ModuleCompute2(nativeObject, point);
        }
        public virtual float Compute(Vector3f point)
        {
            return ModuleCompute3(nativeObject, point);
        }
        public virtual float Compute(Vector4f point)
        {
            return ModuleCompute4(nativeObject, point);
        }

        public virtual float[] Compute(Vector1f[] points)
        {
            float[] returnArray = new float[points.Length];
            ModuleCompute5(nativeObject, points, returnArray, points.Length);
            return returnArray;
        }
        public virtual float[] Compute(Vector2f[] points)
        {
            float[] returnArray = new float[points.Length];
            ModuleCompute6(nativeObject, points, returnArray, points.Length);
            return returnArray;
        }
        public virtual float[] Compute(Vector3f[] points)
        {
            float[] returnArray = new float[points.Length];
            ModuleCompute7(nativeObject, points, returnArray, points.Length);
            return returnArray;
        }
        public virtual float[] Compute(Vector4f[] points)
        {
            float[] returnArray = new float[points.Length];
            ModuleCompute8(nativeObject, points, returnArray, points.Length);
            return returnArray;
        }

        public virtual float[] Compute(Range1f range, Precision1 precision)
        {
            float[] returnArray = new float[precision.x];
            ModuleCompute9(nativeObject, range, precision, returnArray);
            return returnArray;
        }
        public virtual float[,] Compute(Range2f range, Precision2 precision)
        {
            float[] intermediateReturnArray = new float[precision.x * precision.y];
            ModuleCompute10(nativeObject, range, precision, intermediateReturnArray);
            float[,] returnArray = new float[precision.x, precision.y];

            for (ulong x = 0; x < precision.x; x++)
            {
                for (ulong y = 0; y < precision.y; y++)
                {
                    returnArray[x, y] = intermediateReturnArray[y * precision.x + x];
                }
            }

            return returnArray;
        }
        public virtual float[,,] Compute(Range3f range, Precision3 precision)
        {
            float[] intermediateReturnArray = new float[precision.x * precision.y * precision.z];
            ModuleCompute11(nativeObject, range, precision, intermediateReturnArray);
            float[,,] returnArray = new float[precision.x, precision.y, precision.z];

            for (ulong x = 0; x < precision.x; x++)
            {
                for (ulong y = 0; y < precision.y; y++)
                {
                    for (ulong z = 0; z < precision.z; z++)
                    {
                        returnArray[x, y, z] = intermediateReturnArray[z * precision.y * precision.x + y * precision.x + x];
                    }
                }
            }

            return returnArray;
        }
        public virtual float[,,,] Compute(Range4f range, Precision4 precision)
        {
            float[] intermediateReturnArray = new float[precision.x * precision.y * precision.z * precision.w];
            ModuleCompute12(nativeObject, range, precision, intermediateReturnArray);
            float[,,,] returnArray = new float[precision.x, precision.y, precision.z, precision.w];

            for (ulong x = 0; x < precision.x; x++)
            {
                for (ulong y = 0; y < precision.y; y++)
                {
                    for (ulong z = 0; z < precision.z; z++)
                    {
                        for (ulong w = 0; w < precision.w; w++)
                        {
                            returnArray[x, y, z, w] = intermediateReturnArray[w * precision.z * precision.y * precision.x + z * precision.y * precision.x + y * precision.x + x];
                        }
                    }
                }
            }

            return returnArray;
        }

        public virtual float[] ComputeFlat(Range2f range, Precision2 precision)
        {
            float[] returnArray = new float[precision.x * precision.y];
            ModuleCompute10(nativeObject, range, precision, returnArray);

            return returnArray;
        }
        public virtual float[] ComputeFlat(Range3f range, Precision3 precision)
        {
            float[] returnArray = new float[precision.x * precision.y * precision.z];
            ModuleCompute11(nativeObject, range, precision, returnArray);

            return returnArray;
        }
        public virtual float[] ComputeFlat(Range4f range, Precision4 precision)
        {
            float[] returnArray = new float[precision.x * precision.y * precision.z * precision.w];
            ModuleCompute12(nativeObject, range, precision, returnArray);

            return returnArray;
        }



        public virtual ModuleType ModuleType
        {
            get
            {
                switch (GetModuleType(nativeObject))
                {
                    default:
                        return ModuleType.Generator;
                    case 1:
                        return ModuleType.Modifier;
                    case 2:
                        return ModuleType.Combiner;
                    case 3:
                        return ModuleType.Selector;
                    case 4:
                        return ModuleType.Transformer;
                    case 5:
                        return ModuleType.Miscellaneous;
                    case 6:
                        return ModuleType.Other;
                }
            }
        }



        public ModuleComputationTarget ModuleComputationTarget
        {
            get
            {
                switch (ModuleGetComputationTarget(nativeObject))
                {
                    default:
                    case 0:
                        return ModuleComputationTarget.SingleThreadCPU;
                    case 1:
                        return ModuleComputationTarget.MultiThreadCPU;
                    case 2:
                        return ModuleComputationTarget.GPU;
                }
            }
            set
            {
                switch (value)
                {
                    default:
                    case ModuleComputationTarget.SingleThreadCPU:
                        ModuleSetComputationTarget(nativeObject, 0);
                        break;
                    case ModuleComputationTarget.MultiThreadCPU:
                        ModuleSetComputationTarget(nativeObject, 1);
                        break;
                    case ModuleComputationTarget.GPU:
                        ModuleSetComputationTarget(nativeObject, 2);
                        break;
                }
            }
        }

        public float MultithreadedAffinity
        {
            get
            {
                bool result = false;
                float affinity = ModuleGetMultithreadAffinity(nativeObject, ref result);
                if (!result)
                    throw new InvalidOperationException("Module must be configured in MultithreadCPU mode");

                return affinity;
            }
            set
            {
                bool result = false;
                ModuleSetMultithreadAffinity(nativeObject, value, ref result);
                if (!result)
                    throw new InvalidOperationException("Module must be configured in MultithreadCPU mode");
            }
        }

        public string GPUTarget
        {
            get
            {
                bool result = false;
                string target = RetrieveGPUName(ModuleGetGPUTarget(nativeObject, ref result));
                if (!result)
                    throw new InvalidOperationException("Module must be configured in GPU mode");

                return target;
            }
            set
            {
                int index = System.Array.IndexOf(AvailableGPUs, value);

                if (index == -1)
                {
                    throw new InvalidOperationException("Invalid GPU selected");
                }

                bool result = false;
                ModuleSetGPUTarget(nativeObject, availableGPUs[index], ref result);
                if (!result)
                    throw new InvalidOperationException("Module must be configured in GPU mode");
            }
        }



        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool bDisposing)
        {
            if (nativeObject != IntPtr.Zero)
            {
                DestroyModule(nativeObject);
                nativeObject = IntPtr.Zero;
            }

            if (bDisposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        ~NoiseModule()
        {
            Dispose(false);
        }
    }
}

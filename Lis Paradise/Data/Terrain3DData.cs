using UnityEngine;
using UnityEditor;

namespace VoxelTerrain
{
    //[PreferBinarySerialization]
    /*[CreateAssetMenu(fileName = "VoxelTerrain3DData", menuName = "VoxelTerrain3DData", order = 1)]
    public class Terrain3DData : ScriptableObject
    {
        /*public enum DataElement
        {
            PatchResolution,
            PatchScale,
            PatchesCount
        }*/

        //public delegate void DataChangedHandler(DataElement element);

        //public event DataChangedHandler DataChanged;

        /*[SerializeField, HideInInspector]
        private TerrainPatch3DData[,,] patches;
        [SerializeField, HideInInspector]
        private Vector3Int patchesCount = new Vector3Int(1,1,1);

        [SerializeField, HideInInspector]
        private float solidnessThreshold = 0.0f;

        [SerializeField, HideInInspector]
        private Vector3Int patchResolution = new Vector3Int(16, 16, 16);

        [SerializeField, HideInInspector]
        private Vector3 patchScale = new Vector3(16.0f, 16.0f, 16.0f);

        public Vector3Int PatchesCount
        {
            set
            {
                if (patchesCount != value)
                {
                    Debug.Log("REALLOC");

                    patchesCount = value;

                    TerrainPatch3DData[,,] newPatches = new TerrainPatch3DData[patchesCount.x, patchesCount.y, patchesCount.z];

                    if (patches != null)
                    {
                        int mnx = Mathf.Min(patchesCount.x, patches.GetLength(0));
                        int mny = Mathf.Min(patchesCount.y, patches.GetLength(1));
                        int mnz = Mathf.Min(patchesCount.z, patches.GetLength(2));

                        for (int x = 0; x < patchesCount.x; x++)
                        {
                            for (int y = 0; y < patchesCount.y; y++)
                            {
                                for (int z = 0; z < patchesCount.z; z++)
                                {
                                    if (x < mnx && y < mny && z < mnz)
                                    {
                                        newPatches[x, y, z] = patches[x, y, z];
                                    }
                                    else
                                    {
                                        newPatches[x, y, z] = new TerrainPatch3DData(this, PatchResolution);
                                    }
                                }
                            }
                        }
                    }

                    patches = newPatches;
                }
            }
            get
            {
                return patchesCount;
            }
        }

        public Vector3 PatchScale
        {
            set
            {
                if (PatchScale != value)
                {
                    PatchScale = value;

                    //DataChanged(DataElement.PatchScale);
                }
            }
            get
            {
                return PatchScale;
            }
        }

        public Vector3Int PatchResolution
        {
            set
            {
                if (PatchResolution != value)
                {
                    PatchResolution = value;

                    patches = new TerrainPatch3DData[patchesCount.x, patchesCount.y, patchesCount.z];

                    for (int x = 0; x < patchesCount.x; x++)
                    {
                        for (int y = 0; y < patchesCount.y; y++)
                        {
                            for (int z = 0; z < patchesCount.z; z++)
                            {
                                patches[x, y, z] = new TerrainPatch3DData(this, PatchResolution);
                            }
                        }
                    }

                    //DataChanged(DataElement.PatchResolution);
                }
            }
            get
            {
                return PatchResolution;
            }
        }

        public float SolidnessThreshold
        {
            set
            {
                solidnessThreshold = value;
            }
            get
            {
                return solidnessThreshold;
            }
        }

        public TerrainPatch3DData GetPatch(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= patchesCount.x || y >= patchesCount.y || z >= patchesCount.z)
            {
                Debug.LogError("Patch index out of bounds");
            }

            return patches[x, y, z];
        }
    }*/
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelTerrain
{
    [System.Serializable]
    public class TerrainPatch3DData
    {
        public TerrainPatch3DData(Terrain3DData terrain, Vector3Int resolution)
        {
            if (terrain == null)
            {
                Debug.LogError("Patch must be a part of terrain");
            }

            this.TerrainData = terrain;
            this.resolution = resolution;

            data = new TerrainDataPoint[resolution.x, resolution.y, resolution.z];

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    for (int z = 0; z < resolution.z; z++)
                    {
                        data[x, y, z] = new TerrainDataPoint();
                    }
                }
            }
        }

        private Vector3Int resolution;
        private TerrainDataPoint[,,] data;

        public Terrain3DData TerrainData { get; }

        public TerrainDataPoint GetPoint(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= resolution.x || y >= resolution.y || z >= resolution.z)
            {
                Debug.LogError("Point index out of bounds");
            }

            return data[x, y, z];
        }

        public TerrainDataPoint[,,] GetDataRaw()
        {
            //TerrainDataPoint[] rawData = new TerrainDataPoint[data.Length];

            //System.Buffer.BlockCopy(data, 0, rawData, 0, data.Length);

            return data;
        }
    }

    [System.Serializable]
    public struct TerrainDataPoint
    {
        public TerrainDataPoint(float solidness = 0.0f)
        {
            this.solidness = solidness;
            textureIndices = new int[4];
            textureBlendCoefficents = new float[4];
        }

        [SerializeField, HideInInspector]
        private float solidness;
        [SerializeField, HideInInspector]
        private int[] textureIndices;
        [SerializeField, HideInInspector]
        private float[] textureBlendCoefficents;

        public float Solidness
        {
            set
            {
                solidness = value;
            }
            get
            {
                return solidness;
            }
        }

        public int[] TextureIndices
        {
            get
            {
                return textureIndices;
            }
        }

        public float[] TextureBlendCoefficents
        {
            get
            {
                return textureBlendCoefficents;
            }
        }
    }

    [RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider))]
    public class TerrainPatch3D : MonoBehaviour
    {
        private MeshFilter filter;
        private Terrain3D terrain;
        private TerrainPatch3DData patchData;

        private void Start()
        {
            filter = GetComponent<MeshFilter>();
        }

        public TerrainPatch3D(Terrain3D terrain, TerrainPatch3DData data)
        {
            if (terrain == null)
            {
                Debug.LogError("Terrain patch must be owned by a terrain");
            }

            patchData = data;
        }

        public void UpdatePatchMesh()
        {
            Mesh mesh = filter.sharedMesh;

            terrain.MeshGenerator.GenerateMesh(ref mesh, patchData);

            filter.sharedMesh = mesh;
        }

        public void UpdatePatchScale()
        {
            transform.localScale = new Vector3(
                1.0f / (patchData.TerrainData.PatchResolution.x - 1) * patchData.TerrainData.PatchScale.x,
                1.0f / (patchData.TerrainData.PatchResolution.y - 1) * patchData.TerrainData.PatchScale.y,
                1.0f / (patchData.TerrainData.PatchResolution.z - 1) * patchData.TerrainData.PatchScale.z
            );
        }
    }
        /*
        void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.2f);
            Gizmos.DrawSphere(brushMiddle, brushRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, PatchSize);

            Vector3Int l;
            Vector3Int h;

            CalculateBoundedPoints(brushMiddle, new Vector3(brushRadius, brushRadius, brushRadius), out l, out h);

            Gizmos.color = Color.green;

            for (int x = l.x; x <= h.x; x++)
            {
                for (int y = l.y; y <= h.y; y++)
                {
                    for (int z = l.z; z <= h.z; z++)
                    {
                        Vector3 pointPos = PointPosition(new Vector3Int(x, y, z));
                        if (Vector3.Distance(pointPos, brushMiddle) <= brushRadius)
                        {
                            Gizmos.DrawSphere(pointPos, 0.1f);
                        }
                    }
                }
            }
        }
    }*/

    /*
    [RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider))]
    public class TerrainPatch3D : MonoBehaviour
    {
        Vector3 brushMiddle = Vector3.zero;
        Vector2 snapPos;
        bool shiftPressed = false;
        float brushSnap = 1.0f;
        float brushRadius = 1.0f;

        int IndexFromCoord(int x, int y, int z)
        {
            return z * PatchMeshResolution.y * PatchMeshResolution.x + y * PatchMeshResolution.x + x;
        }

        void Update()
        {
            if(Application.isPlaying)
            {
                RaycastHit hit;

                Ray ray = player.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    brushMiddle = hit.point;
                }
                else
                {
                    Bounds bounds = new Bounds();
                    bounds.center = transform.position;
                    bounds.extents = PatchSize/2;

                    float distance = 0;

                    if (bounds.IntersectRay(ray, out distance))
                    {
                        brushMiddle = ray.origin + ray.direction * distance;
                    }
                }


                if(Input.GetKeyDown(KeyCode.LeftShift))
                {
                    shiftPressed = true;
                    brushSnap = brushRadius;
                    snapPos = Input.mousePosition;
                }

                if(Input.GetKeyUp(KeyCode.LeftShift))
                {
                    shiftPressed = false;
                }

                if(shiftPressed)
                {
                    brushRadius = Mathf.Clamp(brushSnap + (Input.mousePosition.y - snapPos.y) / 50.0f, 1.0f, 5.0f);
                }

                if (Input.GetButton("Fire1"))
                {
                    Vector3Int l;
                    Vector3Int h;

                    CalculateBoundedPoints(brushMiddle, new Vector3(brushRadius, brushRadius, brushRadius), out l, out h);

                    for (int x = l.x; x <= h.x; x++)
                    {
                        for (int y = l.y; y <= h.y; y++)
                        {
                            for (int z = l.z; z <= h.z; z++)
                            {
                                Vector3 pointPos = PointPosition(new Vector3Int(x, y, z));
                                if (Vector3.Distance(pointPos, brushMiddle) <= brushRadius)
                                {
                                    PatchData[IndexFromCoord(x, y, z)] = Mathf.Clamp(PatchData[IndexFromCoord(x, y, z)] + strength * Time.deltaTime, min, max);
                                    requiresMeshUpdate = true;
                                }
                            }
                        }
                    }
                }

                if (Input.GetButton("Fire2"))
                {
                    Vector3Int l;
                    Vector3Int h;

                    CalculateBoundedPoints(brushMiddle, new Vector3(brushRadius, brushRadius, brushRadius), out l, out h);

                    for (int x = l.x; x <= h.x; x++)
                    {
                        for (int y = l.y; y <= h.y; y++)
                        {
                            for (int z = l.z; z <= h.z; z++)
                            {
                                Vector3 pointPos = PointPosition(new Vector3Int(x, y, z));
                                if (Vector3.Distance(pointPos, brushMiddle) <= brushRadius)
                                {
                                    PatchData[IndexFromCoord(x, y, z)] = Mathf.Clamp(PatchData[IndexFromCoord(x, y, z)] - strength * Time.deltaTime, min, max);
                                    requiresMeshUpdate = true;
                                }
                            }
                        }
                    }
                }
            }

            if (requiresResizeResolution)
            {
                UpdatePatchResolution();
                requiresResizeResolution = false;
                requiresMeshUpdate = true;
            }

            if (requiresMeshUpdate)
            {
                UpdatePatchMesh();
                requiresMeshUpdate = false;
                requiresResizeScale = true;
            }

            if (requiresResizeScale)
            {
                UpdatePatchScale();
                requiresResizeScale = false;
            }
        }


        
    }*/

}

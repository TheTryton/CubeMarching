using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace VoxelTerrain
//{
namespace VoxelTerrain
{
    [System.Serializable]
    public class Terrain3DMeshGenerator
    {
        private ComputeShader cubeMarching;
        private ComputeShader triangleToMeshData;

        private ComputeBuffer pointsBuffer;

        private ComputeBuffer triangleBuffer;
        private ComputeBuffer triCountBuffer;

        private ComputeBuffer verticesBuffer;
        private ComputeBuffer indicesBuffer;
        private ComputeBuffer uvsBuffer;

        private Vector3Int patchResolution;

        public Vector3Int PatchResolution
        {
            get
            {
                return patchResolution;
            }
            set
            {
                if (value != patchResolution)
                {
                    patchResolution = value;

                    if (pointsBuffer != null)
                    {
                        DestroyBuffers();
                    }

                    CreateBuffers();
                }
            }
        }

        private int PointsCount()
        {
            return PatchResolution.x * PatchResolution.y * PatchResolution.z;
        }

        private int VoxelsCount()
        {
            return (PatchResolution.x - 1) * (PatchResolution.y - 1) * (PatchResolution.z - 1);
        }

        public Terrain3DMeshGenerator(ComputeShader cubeMarching, ComputeShader triangleToMeshData)
        {
            this.cubeMarching = cubeMarching;
            this.triangleToMeshData = triangleToMeshData;

            if (cubeMarching == null || triangleToMeshData == null)
            {
                Debug.LogError("Must provide marching compute shader in order to generate mesh");
            }
        }

        ~Terrain3DMeshGenerator()
        {
            if (pointsBuffer != null)
            {
                DestroyBuffers();
            }
        }

        private void DestroyBuffers()
        {
            pointsBuffer.Dispose();
            triangleBuffer.Dispose();
            triCountBuffer.Dispose();
            verticesBuffer.Dispose();
            indicesBuffer.Dispose();
            uvsBuffer.Dispose();
        }

        private void CreateBuffers()
        {
            int pointsCount = PointsCount();
            int voxelCount = VoxelsCount();

            pointsBuffer = new ComputeBuffer(pointsCount, sizeof(float));
            triangleBuffer = new ComputeBuffer(voxelCount * 5, sizeof(float) * 3 * 3, ComputeBufferType.Append);
            triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

            verticesBuffer = new ComputeBuffer(voxelCount * 5, sizeof(float) * 3);
            indicesBuffer = new ComputeBuffer(voxelCount * 5, sizeof(int));
            uvsBuffer = new ComputeBuffer(voxelCount * 5, sizeof(float) * 2);
        }

        public void GenerateMesh(ref Mesh mesh, TerrainPatch3DData patchData)
        {
            if (patchData.TerrainData.PatchResolution != PatchResolution)
            {
                Debug.LogError("Incorrect data passed!");
            }

            if (cubeMarching == null || triangleToMeshData == null)
            {
                Debug.LogError("One of the shaders is not set!");
            }

            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                mesh.MarkDynamic();
            }
            else
            {
                mesh.Clear();
            }

            int pointsCount = PointsCount();
            int voxelCount = VoxelsCount();

            TerrainDataPoint[,,] data = patchData.GetDataRaw();

            pointsBuffer.SetData(data, 0, 0, pointsCount);

            triangleBuffer.SetCounterValue(0);

            int kernel = cubeMarching.FindKernel("March3D");

            if (kernel == -1)
            {
                Debug.LogError("Couldn't find kernel March3D inside chubeMarching shader!");
            }

            cubeMarching.SetInt("PatchSizeX", PatchResolution.x);
            cubeMarching.SetInt("PatchSizeY", PatchResolution.y);
            cubeMarching.SetInt("PatchSizeZ", PatchResolution.z);

            cubeMarching.SetFloat("threshold", patchData.TerrainData.SolidnessThreshold);

            cubeMarching.SetBuffer(kernel, "outTriangles", triangleBuffer);
            cubeMarching.SetBuffer(kernel, "pointData", pointsBuffer);

            uint threadGroupX;
            uint threadGroupY;
            uint threadGroupZ;

            cubeMarching.GetKernelThreadGroupSizes(kernel, out threadGroupX, out threadGroupY, out threadGroupZ);

            cubeMarching.Dispatch(kernel,
                (int)Mathf.Ceil((PatchResolution.x - 1) / (float)threadGroupX),
                (int)Mathf.Ceil((PatchResolution.y - 1) / (float)threadGroupY),
                (int)Mathf.Ceil((PatchResolution.z - 1) / (float)threadGroupZ)
            );

            ComputeBuffer.CopyCount(triangleBuffer, triCountBuffer, 0);
            int[] triCountArray = { 0 };
            triCountBuffer.GetData(triCountArray);
            int triCount = triCountArray[0];

            kernel = triangleToMeshData.FindKernel("TrianglesToMeshData");

            if (kernel == -1)
            {
                Debug.LogError("Couldn't find kernel TrianglesToMeshData inside triangleToMeshData shader!");
            }

            triangleToMeshData.GetKernelThreadGroupSizes(kernel, out threadGroupX, out threadGroupY, out threadGroupZ);

            triangleToMeshData.SetInt("trianglesCount", triCount);
            triangleToMeshData.SetBuffer(kernel, "triangles", triangleBuffer);
            triangleToMeshData.SetBuffer(kernel, "vertices", verticesBuffer);
            triangleToMeshData.SetBuffer(kernel, "indices", indicesBuffer);
            triangleToMeshData.SetBuffer(kernel, "uvs", uvsBuffer);

            triangleToMeshData.Dispatch(kernel,
                (int)Mathf.Max(Mathf.Ceil(triCount / (float)threadGroupX), 1.0f),
                1,
                1
            );

            Vector3[] vertices = new Vector3[triCount * 3];
            Vector2[] uvs = new Vector2[triCount * 3];
            int[] triangles = new int[triCount * 3];

            verticesBuffer.GetData(vertices, 0, 0, triCount * 3);
            indicesBuffer.GetData(triangles, 0, 0, triCount * 3);
            uvsBuffer.GetData(uvs, 0, 0, triCount * 3);

            mesh.vertices = vertices;

            mesh.triangles = triangles;

            mesh.uv = uvs;

            mesh.RecalculateNormals();
        }
    }
}
    /*
    public class Terrain3D : MonoBehaviour
    {
        public ComputeShader marchingShader;
        public ComputeShader triangleToMeshDataShader;
        [SerializeField]
        private Terrain3DData terrainData;

        private TerrainPatch3D[,,] patches;

        public Camera player;

        public Terrain3DMeshGenerator MeshGenerator { get; private set; }

        /*private void OnTerrainDataChanged(Terrain3DData.DataElement element)
        {
            switch(element)
            {
                case Terrain3DData.DataElement.PatchesCount:
                    if(terrainData.PatchesCount != new Vector3Int(patches.GetLength(0), patches.GetLength(1), patches.GetLength(2)))
                    {
                        UpdatePatches();
                        UpdatePatchesMesh();
                        UpdatePatchesScale();
                    }
                    break;
                case Terrain3DData.DataElement.PatchScale:
                    UpdatePatchesScale();
                    break;
                case Terrain3DData.DataElement.PatchResolution:
                    DestroyPatches();

                    UpdatePatchesMesh();
                    UpdatePatchesScale();
                    break;
                default:
                    break;
            }
        }*/
    /*
        private void DestroyPatches()
        {
            if (patches != null)
            {
                for (int x = 0; x < patches.GetLength(0); x++)
                {
                    for (int y = 0; y < patches.GetLength(1); y++)
                    {
                        for (int z = 0; z < patches.GetLength(2); z++)
                        {
                            Destroy(patches[x, y, z]);
                        }
                    }
                }
            }
        }

        private void UpdatePatches()
        {
            if (terrainData != null)
            {
                if (patches != null)
                {
                    patches = new TerrainPatch3D[terrainData.PatchesCount.x, terrainData.PatchesCount.y, terrainData.PatchesCount.z];

                    for (int x = 0; x < patches.GetLength(0); x++)
                    {
                        for (int y = 0; y < patches.GetLength(1); y++)
                        {
                            for (int z = 0; z < patches.GetLength(2); z++)
                            {
                                patches[x, y, z] = new TerrainPatch3D(this, terrainData.GetPatch(x,y,z));
                            }
                        }
                    }
                }

                if (patches.GetLength(0) != terrainData.PatchesCount.x || patches.GetLength(1) != terrainData.PatchesCount.y || patches.GetLength(2) != terrainData.PatchesCount.z)
                {
                    TerrainPatch3D[,,] newPatchs = new TerrainPatch3D[terrainData.PatchesCount.x, terrainData.PatchesCount.y, terrainData.PatchesCount.z];

                    for (int x = 0; x < terrainData.PatchesCount.x; x++)
                    {
                        for (int y = 0; y < terrainData.PatchesCount.y; y++)
                        {
                            for (int z = 0; z < terrainData.PatchesCount.z; z++)
                            {
                                if(x < patches.GetLength(0) && y < patches.GetLength(1) && z < patches.GetLength(2))
                                {
                                    newPatchs[x, y, z] = patches[x, y, z];
                                }
                                else
                                {
                                    newPatchs[x, y, z] = new TerrainPatch3D(this, terrainData.GetPatch(x, y, z));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdatePatchesMesh()
        {
            if (patches != null)
            {
                for (int x = 0; x < patches.GetLength(0); x++)
                {
                    for (int y = 0; y < patches.GetLength(1); y++)
                    {
                        for (int z = 0; z < patches.GetLength(2); z++)
                        {
                            patches[x, y, z].UpdatePatchMesh();
                        }
                    }
                }
            }
        }

        private void UpdatePatchesScale()
        {
            if (patches != null)
            {
                for (int x = 0; x < patches.GetLength(0); x++)
                {
                    for (int y = 0; y < patches.GetLength(1); y++)
                    {
                        for (int z = 0; z < patches.GetLength(2); z++)
                        {
                            patches[x, y, z].UpdatePatchScale();
                        }
                    }
                }
            }
        }

        private void Start()
        {
            MeshGenerator = new Terrain3DMeshGenerator(marchingShader, triangleToMeshDataShader);

            UpdatePatches();
        }

        private void Update()
        {

        }

        [Header("Editor Properties")]
        [Range(0.0f,1.0f)]
        public float brushStrenght = 0.5f;

        private Vector3 brushMiddle = Vector3.zero;
        private Vector2 snapPos;
        private bool shiftPressed = false;
        private float brushSnap = 1.0f;
        private float brushRadius = 1.0f;

        private void CalculateBoundedPoints(Vector3 center, Vector3 halfExtents, out Vector3Int low, out Vector3Int high)
        {
            // center in terrain object space
            halfExtents = transform.worldToLocalMatrix * halfExtents;
            Vector3 localCenter = transform.worldToLocalMatrix * new Vector4(brushMiddle.x, brushMiddle.y, brushMiddle.z, 1.0f);

            Vector3 pointSeparation = new Vector3(
                terrainData.PatchScale.x / (terrainData.PatchResolution.x - 1),
                terrainData.PatchScale.y / (terrainData.PatchResolution.y - 1),
                terrainData.PatchScale.z / (terrainData.PatchResolution.z - 1)
            );

            Vector3 lf = localCenter - new Vector3(brushRadius, brushRadius, brushRadius);
            Vector3 hf = localCenter + new Vector3(brushRadius, brushRadius, brushRadius);

            lf = new Vector3(
                Mathf.Clamp(lf.x, 0, terrainData.PatchScale.x * terrainData.PatchesCount.x),
                Mathf.Clamp(lf.y, 0, terrainData.PatchScale.y * terrainData.PatchesCount.y),
                Mathf.Clamp(lf.z, 0, terrainData.PatchScale.z * terrainData.PatchesCount.z)
            );

            hf = new Vector3(
                Mathf.Clamp(hf.x, 0, terrainData.PatchScale.x * terrainData.PatchesCount.x),
                Mathf.Clamp(hf.y, 0, terrainData.PatchScale.y * terrainData.PatchesCount.y),
                Mathf.Clamp(hf.z, 0, terrainData.PatchScale.z * terrainData.PatchesCount.z)
            );

            low = new Vector3Int(
                (int)Mathf.Round(lf.x / pointSeparation.x),
                (int)Mathf.Round(lf.y / pointSeparation.y),
                (int)Mathf.Round(lf.z / pointSeparation.z)
                );

            high = new Vector3Int(
                (int)Mathf.Round(hf.x / pointSeparation.x),
                (int)Mathf.Round(hf.y / pointSeparation.y),
                (int)Mathf.Round(hf.z / pointSeparation.z)
                );
        }

        private Vector3 PointPositionLocal(Vector3Int index)
        {
            Vector3 pointSeparation = new Vector3(
                terrainData.PatchScale.x / (terrainData.PatchResolution.x - 1),
                terrainData.PatchScale.y / (terrainData.PatchResolution.y - 1),
                terrainData.PatchScale.z / (terrainData.PatchResolution.z - 1));

            return new Vector3(index.x * pointSeparation.x, index.y * pointSeparation.y, index.z * pointSeparation.z);
        }

        private void OnDrawGizmos()
        {
            Matrix4x4 world = Gizmos.matrix;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.2f);

            Gizmos.DrawSphere(transform.worldToLocalMatrix * new Vector4(brushMiddle.x, brushMiddle.y, brushMiddle.z, 1.0f), brushRadius);

            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(transform.position, 
                new Vector3(
                     terrainData.PatchScale.x * terrainData.PatchesCount.x,
                     terrainData.PatchScale.y * terrainData.PatchesCount.y,
                     terrainData.PatchScale.z * terrainData.PatchesCount.z
                )
            );

            Vector3Int l;
            Vector3Int h;

            CalculateBoundedPoints(brushMiddle, new Vector3(brushRadius, brushRadius, brushRadius), out l, out h);

            Gizmos.color = Color.green;

            Gizmos.matrix = world;

            for (int x = l.x; x <= h.x; x++)
            {
                for (int y = l.y; y <= h.y; y++)
                {
                    for (int z = l.z; z <= h.z; z++)
                    {
                        Vector3 pointPosWorld = transform.localToWorldMatrix * PointPositionLocal(new Vector3Int(x, y, z));
                        if (Vector3.Distance(pointPosWorld, brushMiddle) <= brushRadius)
                        {
                            Gizmos.DrawSphere(pointPosWorld, 0.1f);
                        }
                    }
                }
            }
        }
    }
}*/



namespace VoxelTerrain
{
    public class Terrain3D : MonoBehaviour
    {
        public int PatchSizeX = 16;
        public int PatchSizeY = 16;
        public int PatchSizeZ = 16;

        public Vector3 normalizedPatchSize = new Vector3(16, 16, 16);

        public float Threshold;

        public bool autoUpdateInEditor = true;

        private bool settingsUpdated = false;

        public GameObject observer;

        private List<TerrainPatch3D> Patchs;

        public Dictionary<string, Vector3> terrainObservers;

        public void UpdateTerrainObserverPosition(string name, Vector3 position)
        {
            terrainObservers[name] = position;
        }

        public void RemoveTerrainObserver(string name)
        {
            terrainObservers.Remove(name);
        }

        public ITerrainGenerator generator;

        private bool ShouldGeneratePatch(int x, int y, int z)
        {
            return true;
        }

        private void UpdateVisiblePatchs()
        {
            if(Patchs == null || Patchs.GetLength(0) != XPatchsCount || Patchs.GetLength(1) != YPatchsCount || Patchs.GetLength(2) != ZPatchsCount)
            {
                if (Patchs != null)
                {
                    for (int i = 0; i < Patchs.GetLength(0); i++)
                    {
                        for (int j = 0; j < Patchs.GetLength(1); j++)
                        {
                            for (int k = 0; k < Patchs.GetLength(2); k++)
                            {
                                if(Patchs[i, j, k])
                                    DestroyImmediate(Patchs[i, j, k].gameObject);
                            }
                        }
                    }
                }

                Patchs = new TerrainPatch3D[XPatchsCount, YPatchsCount, ZPatchsCount];

                for (int i = 0; i < Patchs.GetLength(0); i++)
                {
                    for (int j = 0; j < Patchs.GetLength(1); j++)
                    {
                        for (int k = 0; k < Patchs.GetLength(2); k++)
                        {
                            Vector3 PatchPos =
                                new Vector3(i * normalizedPatchSize.x, j * normalizedPatchSize.y, k * normalizedPatchSize.z) -
                                new Vector3(Patchs.GetLength(0) * normalizedPatchSize.x, Patchs.GetLength(1) * normalizedPatchSize.y, Patchs.GetLength(2) * normalizedPatchSize.z) / 2.0f;

                            GameObject newPatch = Instantiate(PatchPrefab, PatchPos, Quaternion.identity);
                            Patchs[i, j, k] = newPatch.GetComponent<TerrainPatch3D>();
                            Patchs[i, j, k].PatchIndex = new Vector3Int(i, j, k);

                            Patchs[i, j, k].PatchSize = normalizedPatchSize;

                            Patchs[i, j, k].generator.ModuleComputationTarget = GNoise.ModuleComputationTarget.GPU;
                            Patchs[i, j, k].generatorUV.ModuleComputationTarget = GNoise.ModuleComputationTarget.GPU;
                            Patchs[i, j, k].generator.GPUTarget = GNoise.NoiseModule.AvailableGPUs[0];
                            Patchs[i, j, k].generatorUV.GPUTarget = GNoise.NoiseModule.AvailableGPUs[0];
                        }
                    }
                }
            }

            List<TerrainPatch3D> PatchsToUnload = new List<TerrainPatch3D>();

            for (int i = 0; i < Patchs.GetLength(0); i++)
            {
                for (int j = 0; j < Patchs.GetLength(1); j++)
                {
                    for (int k = 0; k < Patchs.GetLength(2); k++)
                    {
                        TerrainPatch3D Patch = Patchs[i, j, k];

                        int lodLevel = terrainLODLevels.Count;

                        foreach (Vector3 observer in TerrainObservers.Values)
                        {
                            float distance = Vector3.Distance(observer, Patch.transform.position);

                            for (int l = terrainLODLevels.Count - 1; l >= 0; l--)
                            {
                                if (distance < terrainLODLevels[l].maxDistanceFromObserver)
                                {
                                    if (l < lodLevel)
                                    {
                                        lodLevel = l;
                                    }
                                }
                            }
                        }

                        Patch.Threshold = Threshold;

                        if (lodLevel == terrainLODLevels.Count)
                        {
                            PatchsToUnload.Add(Patch);
                        }
                        else
                        {
                            Patch.gameObject.SetActive(true);

                            Patch.PatchMeshResolution = terrainLODLevels[lodLevel].PatchDimensions;
                        }
                    }
                }
            }

            foreach (TerrainPatch3D PatchToUnload in PatchsToUnload)
            {
                PatchToUnload.gameObject.SetActive(false);
            }
        }

        public void Start()
        {
            TerrainLODLevel level = new TerrainLODLevel();
            level.PatchDimensions = new Vector3Int(100, 100, 100);
            level.maxDistanceFromObserver = float.PositiveInfinity;
            terrainLODLevels.Add(level);
        }

        private bool coll = false;
        private Vector3 currentPoint = Vector3.zero;
        private Vector3 currentNormal = Vector3.zero;

        public void Update()
        {
            UpdateTerrainObserverPosition("editor", GetComponent<Camera>().transform.position);
            UpdateVisiblePatchs();

            Ray ray = GetComponent<Camera>().GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                currentPoint = hitInfo.point;
                currentNormal = hitInfo.normal;

                coll = true;
            }
            else
            {
                coll = false;
            }
        }

        void OnDrawGizmos()
        {
            if(coll)
            {
                Gizmos.color = new Color(1, 0, 0, 1);

                Gizmos.DrawSphere(currentPoint, 0.1f);

                Gizmos.color = new Color(0, 0, 1, 1);

                Gizmos.DrawLine(currentPoint, currentPoint + currentNormal);
            }
        }

        public void Generate()
        {

        }

        public void RequestUpdate()
        {
        }

        void OnValidate()
        {
            settingsUpdated = true;
        }
    }
}

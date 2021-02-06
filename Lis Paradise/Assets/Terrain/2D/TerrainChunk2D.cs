using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class TerrainPatch2D : MonoBehaviour
{

    [SerializeField, Range(2, 512)]
    private int PatchSizeX = 32;
    [SerializeField, Range(2, 512)]
    private int PatchSizeY = 32;

    [SerializeField]
    private float[,] PatchData;

    [SerializeField]
    private float threshold = 0.5f;

    [SerializeField]
    private ComputeShader shader;

    private ComputeBuffer triangleBuffer;
    private ComputeBuffer triCountBuffer;
    private ComputeBuffer pointsBuffer;

    private static int v00 = 0;
    private static int v10 = 1;
    private static int v01 = 2;
    private static int v11 = 3;

    private static int v50 = 4;
    private static int v05 = 5;
    private static int v51 = 6;
    private static int v15 = 7;

    private static List<int>[] triangulationIndices = new List<int>[16]
    {
        new List<int>(),
        new List<int>{v00, v50, v05 },
        new List<int>{v10, v15, v50 },
        new List<int>{v00, v10, v15, v00, v15, v05},

        new List<int>{v51, v15, v11},
        new List<int>{v00, v50, v05, v05, v50, v51, v50, v15, v51, v51, v15, v11},
        new List<int>{v50, v10, v11, v50, v11, v51},
        new List<int>{v00, v10, v05, v05, v10, v51, v51, v10, v11},

        new List<int>{v01, v05, v51},
        new List<int>{v00, v51, v01, v00, v50, v51},
        new List<int>{v01, v05, v51, v05, v50, v51, v51, v50, v15, v15, v50, v10},
        new List<int>{v00, v51, v01, v00, v15, v51, v00, v10, v15},

        new List<int>{v01, v05, v11, v05, v15, v11},
        new List<int>{v00, v50, v01, v01, v50, v15, v01, v15, v11},
        new List<int>{v01, v05, v11, v05, v50, v11, v50, v10, v11},
        new List<int>{v00, v11, v01, v00, v10, v11}
    };

    private struct Triangle
    {
#pragma warning disable 649 // disable unassigned variable warning
        public Vector2 v1, v2, v3;

        public Vector2 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return v1;
                    case 1:
                        return v2;
                    default:
                        return v3;
                }
            }
        }
    }

    private Mesh GenerateMeshGPU()
    {
        Mesh mesh = new Mesh();

        pointsBuffer = new ComputeBuffer(PatchSizeX * PatchSizeY, sizeof(float));
        triangleBuffer = new ComputeBuffer((PatchSizeX - 1) * (PatchSizeY - 1) * 12, sizeof(float) * 2 * 3, ComputeBufferType.Append);
        triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

        float[] pointData = new float[PatchSizeX * PatchSizeY];
        for (int i = 0; i < PatchSizeX; i++)
        {
            for (int j = 0; j < PatchSizeY; j++)
            {
                pointData[i * PatchSizeY + j] = PatchData[i, j];
            }
        }

        pointsBuffer.SetData(pointData, 0, 0, PatchSizeX * PatchSizeY);

        triangleBuffer.SetCounterValue(0);
        shader.SetInt("PatchSizeX", PatchSizeX);
        shader.SetInt("PatchSizeY", PatchSizeY);
        shader.SetFloat("threshold", threshold);
        shader.SetBuffer(0, "triangles", triangleBuffer);
        shader.SetBuffer(0, "points", pointsBuffer);

        shader.Dispatch(shader.FindKernel("March2D"), (int)Mathf.Ceil(PatchSizeX / 8.0f), (int)Mathf.Ceil(PatchSizeY / 8.0f), 1);


        ComputeBuffer.CopyCount(triangleBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int triCount = triCountArray[0];

        Triangle[] tris = new Triangle[triCount];
        triangleBuffer.GetData(tris, 0, 0, triCount);
        Vector3[] vertices = new Vector3[triCount * 3];
        int[] triangles = new int[triCount * 3];

        for (int i = 0; i < triCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                triangles[i * 3 + j] = i * 3 + j;
                vertices[i * 3 + j] = tris[i][j];
            }
        }

        mesh.vertices = vertices;

        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        triangleBuffer.Dispose();
        pointsBuffer.Dispose();
        triCountBuffer.Dispose();

        return mesh;
    }

    private Mesh GenerateMeshCPU()
    {
        Mesh mesh = new Mesh();

        List<Triangle> tris = new List<Triangle>();

        for (int i = 1; i < PatchSizeX; i++)
        {
            for (int j = 1; j < PatchSizeY; j++)
            {
                int squareIndex = 0;
                if (PatchData[i - 1, j - 1] > threshold) squareIndex |= 1;
                if (PatchData[i, j - 1] > threshold) squareIndex |= 2;
                if (PatchData[i, j] > threshold) squareIndex |= 4;
                if (PatchData[i - 1, j] > threshold) squareIndex |= 8;

                Vector2[] pointPositions = new Vector2[8]
                {
                    new Vector2(i - 1,j - 1),
                    new Vector2(i,j - 1),
                    new Vector2(i - 1,j),
                    new Vector2(i,j),

                    new Vector2(i - 0.5f,j - 1),
                    new Vector2(i - 1,j - 0.5f),
                    new Vector2(i - 0.5f,j),
                    new Vector2(i, j - 0.5f),
                };

                for (int ti = 0; ti < triangulationIndices[squareIndex].Count; ti += 3)
                {
                    Triangle tri;

                    tri.v1 = pointPositions[triangulationIndices[squareIndex][ti]];
                    tri.v2 = pointPositions[triangulationIndices[squareIndex][ti + 1]];
                    tri.v3 = pointPositions[triangulationIndices[squareIndex][ti + 2]];

                    tris.Add(tri);
                }
            }
        }

        int triCount = tris.Count;

        Vector3[] vertices = new Vector3[triCount * 3];
        int[] triangles = new int[triCount * 3];

        for (int i = 0; i < triCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                triangles[i * 3 + j] = i * 3 + j;
                vertices[i * 3 + j] = tris[i][j];
            }
        }

        mesh.vertices = vertices;

        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    private void GenerateMesh()
    {
        PatchData = new float[PatchSizeX, PatchSizeY];

        int newNoise = Random.Range(0, 10000);

        for (int i = 0; i < PatchSizeX; i++)
        {
            for (int j = 0; j < PatchSizeY; j++)
            {
                PatchData[i, j] = Mathf.PerlinNoise(i/32.0f+ newNoise, j / 32.0f + newNoise);
            }
        }

        Mesh m1 = GenerateMeshGPU();
        //Mesh m2 = GenerateMeshRegular();

        //CombineInstance m1c = new CombineInstance();
        //CombineInstance m2c = new CombineInstance();

        //m1c.mesh = m1;
        //m2c.mesh = m2;

        //m1c.transform = Matrix4x4.Translate(new Vector3(0, 0, 1)) * transform.localToWorldMatrix;
        //m2c.transform = Matrix4x4.Translate(new Vector3(0, 0, -1)) * transform.localToWorldMatrix;

        //Mesh mesh = new Mesh();
        //mesh.CombineMeshes(new CombineInstance[2] { m1c, m2c });

        GetComponent<MeshFilter>().mesh = m1;
    }

    void Start()
    {
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            GenerateMesh();
        }
    }
}

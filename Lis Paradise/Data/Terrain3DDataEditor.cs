using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelTerrain
{/*
    [CustomEditor(typeof(Terrain3DData))]
    public class Terrain3DDataEditor : Editor
    {
        private Terrain3DData data;

        public void OnEnable()
        {
            data = (Terrain3DData)target;
        }

        public override void OnInspectorGUI()
        {
            if(data != null)
            {
                int patchesCountX = Mathf.Clamp(EditorGUILayout.IntField("Patches X", data.PatchesCount.x), 1, 32);
                int patchesCountY = Mathf.Clamp(EditorGUILayout.IntField("Patches Y", data.PatchesCount.y), 1, 32);
                int patchesCountZ = Mathf.Clamp(EditorGUILayout.IntField("Patches Z", data.PatchesCount.z), 1, 32);

                data.PatchesCount = new Vector3Int(patchesCountX, patchesCountY, patchesCountZ);
            }
            
        }
    }*/
}

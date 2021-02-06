using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VoxelTerrain
{
    /*
    [CustomEditor(typeof(Terrain3D))]
    public class Terrain3DEditor : Editor
    {
        public static int ToggleList(int selected, GUIContent[] items)
        {
            // Keep the selected index within the bounds of the items array
            selected = selected < 0 ? 0 : selected >= items.Length ? items.Length - 1 : selected;

            GUILayout.BeginVertical();
            for (int i = 0; i < items.Length; i++)
            {
                // Display toggle. Get if toggle changed.
                bool change = GUILayout.Toggle(selected == i, items[i]);
                // If changed, set selected to current index.
                if (change)
                    selected = i;
            }
            GUILayout.EndVertical();

            // Return the currently selected item's index
            return selected;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Terrain3D terrain = (Terrain3D)target;

            int selected = ToggleList(terrain.useCPUForMeshGeneration ? 0 : 1, new GUIContent[2] { new GUIContent("Use CPU for mesh generation"), new GUIContent("Use GPU for mesh generation") });
            if(selected == 0)
            {
                terrain.useCPUForMeshGeneration = true;
                terrain.useGPUForMeshGeneration = false;
            }
            if (selected == 1)
            {
                terrain.useCPUForMeshGeneration = false;
                terrain.useGPUForMeshGeneration = true;
            }

            EditorGUILayout.Space(5);

            terrain.autoUpdateInEditor = GUILayout.Toggle(terrain.autoUpdateInEditor, "Auto update in editor");

        }
    }*/
}

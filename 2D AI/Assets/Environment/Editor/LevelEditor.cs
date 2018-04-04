using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ProceduralTiles;

[CustomEditor(typeof(Level))]
public class GridEditor : Editor {
    public void OnSceneGUI () {
        Level level = target as Level;

        // Get level position and rotation
        Vector3 position = level.transform.position - (level.transform.right * (level.xSize - 1) + level.transform.up * (level.ySize - 1)) * .5f;
        position += Vector3.forward * (1 - (int)level.activeLayer);
        Quaternion rotation = level.transform.rotation;

        // Create buttons for each tile
        for (int i = 0; i < level.xSize; i++) {
            for (int j = 0; j < level.ySize; j++) {
                if(Handles.Button(position + Vector3.right * i + Vector3.up * j, rotation, .5f, .5f, Handles.RectangleHandleCap)) {
                    // Set actions for buttons based on the selected mode
                    switch (level.mode) {
                        case EditMode.Add:
                            level.SetTile(level.selectedTile, i, j);
                            break;
                        case EditMode.Pick:
                            level.selectedTile = level.GetTile(level.activeLayer, i, j);
                            break;
                        case EditMode.Remove:
                            level.RemoveTile(level.activeLayer, i, j);
                            break;
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI() {
        Level level = target as Level;

        // Set grid size
        EditorGUILayout.LabelField("Grid size");

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        int x = EditorGUILayout.IntField("X:", level.xSize);
        int y = EditorGUILayout.IntField("Y:", level.ySize);
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck()) {
            level.SetSize(x, y);
            SceneView.RepaintAll();
        }

        EditorGUILayout.Space();

        // Set selected tool
        string[] tools = Enum.GetNames(typeof(EditMode));

        EditorGUI.BeginChangeCheck();
        int tool = GUILayout.Toolbar((int)level.mode, tools);
        if (EditorGUI.EndChangeCheck()) {
            level.mode = (EditMode)tool;
        }

        EditorGUILayout.Space();

        // Set selected layer
        if(tool == (int)EditMode.Add && level.selectedTile) {
            // In add mode the selected layer is baed on the selected tile
            EditorGUILayout.EnumPopup("Layer:", level.selectedTile.layer);
        }
        else {
            EditorGUI.BeginChangeCheck();
            Layer layer = (Layer) EditorGUILayout.EnumPopup("Layer:", level.activeLayer);
            if (EditorGUI.EndChangeCheck()) {
                level.activeLayer = layer;
                SceneView.RepaintAll();
            }
        }

        // Set selected tile
        EditorGUI.BeginChangeCheck();
        Tile selectedTile = (Tile) EditorGUILayout.ObjectField(level.selectedTile, typeof(Tile), false);
        if (EditorGUI.EndChangeCheck()) {
            level.selectedTile = selectedTile;
        }
    }
}
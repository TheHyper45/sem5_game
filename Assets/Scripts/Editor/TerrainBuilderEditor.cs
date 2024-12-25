using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TerrainBuilder))]
public class TerrainBuilderEditor : Editor {
    private bool editingTerreinData = false;
    private TerrainVector2Int firstSelectedTileCoords = new() { x = -1,z = -1 };
    private readonly List<TerrainVector2Int> nextSelectedTileCoords = new();

    private void HandleTerrainResizing(TerrainBuilder terrainBuilder) {
        Vector3 endPos = new(terrainBuilder.terrainData.sizeX,0f,terrainBuilder.terrainData.sizeZ);
        EditorGUI.BeginChangeCheck();
        endPos = Handles.Slider(endPos,Vector3.forward);
        endPos = Handles.Slider(endPos,Vector3.right);
        if(EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(terrainBuilder,"Resize terrein");
            terrainBuilder.terrainData.Resize(Mathf.Max(0,(int)endPos.x),Mathf.Max(0,(int)endPos.z));
            terrainBuilder.RefreshTerreinMesh();
        }
        Handles.Label(endPos,$"x: {terrainBuilder.terrainData.sizeX},z: {terrainBuilder.terrainData.sizeZ}");
    }

    private void HandleTileSelection(TerrainBuilder terrainBuilder) {
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if(!Physics.Raycast(ray,out RaycastHit raycastHit)) return;
        var px = (int)raycastHit.point.x;
        var pz = (int)raycastHit.point.z;
        var py = terrainBuilder.terrainData.GetHeight(px,pz);
        if(firstSelectedTileCoords.x < 0 || firstSelectedTileCoords.z < 0) {
            Handles.DrawWireCube(new(px + 0.5f,py,pz + 0.5f),new(1f,0f,1f));
        }

        if(Event.current.type != EventType.MouseDown || Event.current.button != 0) return;
        Event.current.Use();

        if(!Event.current.control && !Event.current.shift) {
            nextSelectedTileCoords.Clear();
            firstSelectedTileCoords.x = px;
            firstSelectedTileCoords.z = pz;
        }
        else if(Event.current.control) {
            var index = nextSelectedTileCoords.FindIndex((point) => point.x == px && point.z == pz);
            if(index >= 0) {
                nextSelectedTileCoords.RemoveAt(index);
            }
            else {
                nextSelectedTileCoords.Add(new() { x = px,z = pz });
            }
        }
        else if(Event.current.shift) {
            nextSelectedTileCoords.Clear();
            var firstX = Mathf.Min(firstSelectedTileCoords.x,px);
            var lastX = Mathf.Max(firstSelectedTileCoords.x,px);
            var firstZ = Mathf.Min(firstSelectedTileCoords.z,pz);
            var lastZ = Mathf.Max(firstSelectedTileCoords.z,pz);
            for(int z = firstZ;z <= lastZ;z += 1) {
                for(int x = firstX;x <= lastX;x += 1) {
                    if(firstSelectedTileCoords.x == x && firstSelectedTileCoords.z == z) continue;
                    nextSelectedTileCoords.Add(new() { x = x,z = z });
                }
            }
        }
    }

    private void ChangeSelectedTilesHeight(TerrainBuilder terrainBuilder,int value) {
        if(value == 0) return;
        Undo.RecordObject(terrainBuilder,"Change height of terrein tiles.");
        terrainBuilder.terrainData.SetHeight(firstSelectedTileCoords,terrainBuilder.terrainData.GetHeight(firstSelectedTileCoords) + value);
        foreach(var tileCoord in nextSelectedTileCoords) {
            terrainBuilder.terrainData.SetHeight(tileCoord,terrainBuilder.terrainData.GetHeight(tileCoord) + value);
        }
        terrainBuilder.RefreshTerreinMesh();
    }

    private void OnSceneGUI() {
        if(!editingTerreinData) return;
        var terrainBuilder = (TerrainBuilder)target;
        EditorUtility.SetDirty(terrainBuilder);
        HandleTerrainResizing(terrainBuilder);

        if(firstSelectedTileCoords.x >= 0 && firstSelectedTileCoords.z >= 0) {
            var firstSelectedTileHeight = terrainBuilder.terrainData.GetHeight(firstSelectedTileCoords);
            Handles.DrawWireCube(new(firstSelectedTileCoords.x + 0.5f,firstSelectedTileHeight,firstSelectedTileCoords.z + 0.5f),new(1f,0f,1f));
            foreach(var tileCoord in nextSelectedTileCoords) {
                var tileHeight = terrainBuilder.terrainData.GetHeight(tileCoord);
                Handles.DrawWireCube(new(tileCoord.x + 0.5f,tileHeight,tileCoord.z + 0.5f),new(1f,0f,1f));
            }

            var firstHandlePosY = terrainBuilder.terrainData.GetHeight(firstSelectedTileCoords.x,firstSelectedTileCoords.z);
            Vector3 firstHandlePos = new(firstSelectedTileCoords.x + 0.5f,firstHandlePosY,firstSelectedTileCoords.z + 0.5f);
            EditorGUI.BeginChangeCheck();
            var newFirstHandlePos = Handles.Slider(firstHandlePos,Vector3.up);
            if(EditorGUI.EndChangeCheck()) {
                var diff = newFirstHandlePos - firstHandlePos;
                ChangeSelectedTilesHeight(terrainBuilder,(int)diff.y);
            }
        }

        HandleTileSelection(terrainBuilder);
        if(Event.current.type == EventType.MouseDown && Event.current.button == 1) {
            firstSelectedTileCoords = new() { x = -1,z = -1 };
            nextSelectedTileCoords.Clear();
        }
    }

    public override void OnInspectorGUI() {
        var terrainBuilder = (TerrainBuilder)target;
        EditorGUI.BeginChangeCheck();
        editingTerreinData = GUILayout.Toggle(editingTerreinData,"Edit Terrein","Button");
        if(EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(terrainBuilder);
        }

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Save Terrein Data",GUILayout.ExpandWidth(false))) {
            var filePath = EditorUtility.SaveFilePanel("Save Terrein Data","Assets/Resources/Levels","Default","json");
            if(!string.IsNullOrEmpty(filePath)) {
                terrainBuilder.terrainData.ToJsonFile(filePath);
            }
        }
        if(GUILayout.Button("Load Terrein Data",GUILayout.ExpandWidth(false))) {
            var filePath = EditorUtility.OpenFilePanel("Save Terrein Data","Assets/Resources/Levels","json");
            if(!string.IsNullOrEmpty(filePath)) {
                terrainBuilder.terrainData = TerrainData.FromJsonFile(filePath);
                terrainBuilder.RefreshTerreinMesh();
            }
        }
        GUILayout.EndHorizontal();
    }
}
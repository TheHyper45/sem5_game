using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TerrainBuilder))]
public class TerrainBuilderEditor : Editor {
    private bool terrainEditingEnabled = false;
    private TerrainData terrainData;
    private TerrainVector2Int firstSelectedTileCoords = new() { x = -1,z = -1 };

    private void HandleTerrainResizing(TerrainBuilder terrainBuilder) {
        Vector3 endPos = new(terrainData.sizeX,0f,terrainData.sizeZ);
        EditorGUI.BeginChangeCheck();
        endPos = Handles.Slider(endPos,Vector3.forward);
        endPos = Handles.Slider(endPos,Vector3.right);
        if(EditorGUI.EndChangeCheck()) {
            terrainData.Resize(Mathf.Max(0,(int)endPos.x),Mathf.Max(0,(int)endPos.z));
            terrainBuilder.SetTerrainData(terrainData);
        }
        Handles.Label(endPos,$"x: {terrainData.sizeX},z: {terrainData.sizeZ}");
    }

    private void HandleTileSelection() {
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit raycastHit)) {
            var px = (int)raycastHit.point.x;
            var pz = (int)raycastHit.point.z;
            var py = terrainData.GetHeight(px,pz);
            if(firstSelectedTileCoords.x < 0 || firstSelectedTileCoords.z < 0) {
                Handles.DrawWireCube(new(px + 0.5f,py,pz + 0.5f),new(1f,0f,1f));
            }
            if(Event.current.type == EventType.MouseDown && Event.current.button == 0) {
                firstSelectedTileCoords.x = px;
                firstSelectedTileCoords.z = pz;
                Event.current.Use();
            }
        }
    }

    private void ChangeSelectedTilesHeight(TerrainBuilder terrainBuilder,int value) {
        if(value == 0) return;
        terrainData.SetHeight(firstSelectedTileCoords,terrainData.GetHeight(firstSelectedTileCoords) + value);
        terrainBuilder.SetTerrainData(terrainData);
    }

    private void OnSceneGUI() {
        if(!terrainEditingEnabled) return;
        var terrainBuilder = (TerrainBuilder)target;
        HandleTerrainResizing(terrainBuilder);

        if(firstSelectedTileCoords.x >= 0 && firstSelectedTileCoords.z >= 0) {
            var height = terrainData.GetHeight(firstSelectedTileCoords);
            Handles.DrawWireCube(new(firstSelectedTileCoords.x + 0.5f,height,firstSelectedTileCoords.z + 0.5f),new(1f,0f,1f));

            Vector3 firstHandlePos = new(firstSelectedTileCoords.x + 0.5f,terrainData.GetHeight(firstSelectedTileCoords.x,firstSelectedTileCoords.z),firstSelectedTileCoords.z + 0.5f);
            EditorGUI.BeginChangeCheck();

            var newFirstHandlePos = Handles.Slider(firstHandlePos,Vector3.up);
            if(EditorGUI.EndChangeCheck()) {
                var diff = newFirstHandlePos - firstHandlePos;
                ChangeSelectedTilesHeight(terrainBuilder,(int)diff.y);
            }
        }
        HandleTileSelection();

        if(Event.current.type == EventType.MouseDown && Event.current.button == 1) {
            firstSelectedTileCoords = new() { x = -1,z = -1 };
        }
        EditorUtility.SetDirty(terrainBuilder);
    }

    public override void OnInspectorGUI() {
        var terrainBuilder = (TerrainBuilder)target;
        var path = $"Assets/Resources/Levels/{SceneManager.GetActiveScene().name}.json";

        GUILayout.Label("Terrain Editing",GUILayout.ExpandWidth(true));
        EditorGUI.BeginChangeCheck();
        terrainEditingEnabled = GUILayout.Toggle(terrainEditingEnabled,"Edit Terrain","Button");
        if(EditorGUI.EndChangeCheck()) {
            if(terrainEditingEnabled) {
                terrainData = TerrainData.FromJsonFile(path);
                terrainBuilder.SetTerrainData(terrainData);
                EditorUtility.SetDirty(terrainBuilder);
            }
            else {
                terrainData.ToJsonFile(path);
                terrainBuilder.LoadTerrainDataFrom(path);
            }
        }
        if(terrainEditingEnabled) {
            if(GUILayout.Button("Flatten Terrain")) {
                Array.Clear(terrainData.heights,0,terrainData.heights.Length);
                terrainBuilder.SetTerrainData(terrainData);
            }
            if(firstSelectedTileCoords.x >= 0 && firstSelectedTileCoords.z >= 0) {
                GUILayout.Label($"Selection: ({firstSelectedTileCoords.x},{firstSelectedTileCoords.z})");
            }
            GUILayout.Label("Terrain Data",GUILayout.ExpandWidth(true));
            if(GUILayout.Button("Load Terrain")) {
                terrainData = TerrainData.FromJsonFile(path);
                terrainBuilder.SetTerrainData(terrainData);
            }
            if(GUILayout.Button("Save Terrain")) {
                terrainData.ToJsonFile(path);
                terrainBuilder.LoadTerrainDataFrom(path);
            }
        }
    }
}
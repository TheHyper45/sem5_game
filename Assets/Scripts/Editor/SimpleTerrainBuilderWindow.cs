using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SimpleTerrainBuilderWindow : EditorWindow {
    private PreviewRenderUtility preview;
    private GameObject terrainGameObject;
    private SimpleTerrainData terrainData;
    private Mesh terrainMesh;
    private Material terrainMaterial;
    private SimpleTerrainVector2Int selectedTile;
    private List<SimpleTerrainVector2Int> nextSelectedTiles;
    private int editTerrainSizeX;
    private int editTerrainSizeZ;

    [MenuItem("Custom/TerrainBuilderWindow")]
    private static void Init() {
        var window = GetWindow<SimpleTerrainBuilderWindow>();
        window.titleContent.text = "Simple Terrain Builder";
        window.wantsMouseMove = true;
        window.Show();
    }

    private void RestoreCameraTransform() {
        preview.camera.transform.SetPositionAndRotation(new(0f,0.5f,-2f),Quaternion.identity);
    }

    private void UpdateTerrain() {
        SimpleTerrain.RebuildMesh(terrainData,terrainMesh);
        terrainGameObject.GetComponent<MeshFilter>().sharedMesh = terrainMesh;
        terrainGameObject.GetComponent<MeshRenderer>().material = terrainMaterial;
        terrainGameObject.GetComponent<MeshCollider>().sharedMesh = terrainMesh;
    }

    private void OnEnable() {
        selectedTile = new() { x = -1,z = -1 };
        terrainGameObject = new("Terrain");
        terrainGameObject.transform.position = Vector3.zero;
        terrainGameObject.AddComponent<MeshFilter>();
        terrainGameObject.AddComponent<MeshRenderer>();
        terrainGameObject.AddComponent<MeshCollider>();

        preview = new();
        preview.AddSingleGO(terrainGameObject);
        preview.camera.nearClipPlane = 0.01f;
        preview.camera.farClipPlane = 256f;
        preview.camera.fieldOfView = 60f;
        RestoreCameraTransform();

        preview.lights[0].intensity = 1.0f;
        preview.lights[0].type = LightType.Directional;
        preview.lights[0].transform.rotation = Quaternion.Euler(50f,-30f,0f);
        preview.lights[1].intensity = 0.0f;

        if(!terrainMaterial) {
            editTerrainSizeX = 16;
            editTerrainSizeZ = 16;
            nextSelectedTiles = new();
            terrainMaterial = Resources.Load<Material>("Images/Materials/Terrain");
            terrainMesh = new();
            terrainData = new() { sizeX = editTerrainSizeX,sizeZ = editTerrainSizeZ,heights = new int[editTerrainSizeX * editTerrainSizeZ],types = new int[editTerrainSizeX * editTerrainSizeZ] };
            UpdateTerrain();
        }
    }

    private void OnDisable() {
        preview.Cleanup();
    }

    private void OnGUI() {
        if(Event.current.button == 1) {
            var rot = preview.camera.transform.rotation.eulerAngles;
            preview.camera.transform.rotation = Quaternion.Euler(rot.x + Event.current.delta.y * 0.1f,rot.y + Event.current.delta.x * 0.1f,rot.z);
        }
        else if(Event.current.button == 2) {
            var vec = -0.01f * Event.current.delta.x * preview.camera.transform.right + 0.01f * Event.current.delta.y * preview.camera.transform.up;
            preview.camera.transform.Translate(vec,Space.World);
        }

        if(Event.current.type == EventType.ScrollWheel) {
            if(Event.current.delta.y < 0f) {
                preview.camera.transform.Translate(preview.camera.transform.forward * 0.2f,Space.World);
            }
            else if(Event.current.delta.y > 0f) {
                preview.camera.transform.Translate(preview.camera.transform.forward * -0.2f,Space.World);
            }
        }

        if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space) {
            nextSelectedTiles.Clear();
            selectedTile.x = -1;
            selectedTile.z = -1;
        }

        Rect pos = new(0,0,position.width,position.height);
        preview.BeginPreview(pos,GUIStyle.none);

        var useScriptableRenderPipeline = Unsupported.useScriptableRenderPipeline;
        Unsupported.useScriptableRenderPipeline = true;
        preview.camera.Render();
        Unsupported.useScriptableRenderPipeline = useScriptableRenderPipeline;

        using(new Handles.DrawingScope(Matrix4x4.identity)) {
            Handles.SetCamera(preview.camera);

            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if(preview.camera.scene.GetPhysicsScene().Raycast(ray.origin,ray.direction,out RaycastHit raycastHit)) {
                var px = (int)raycastHit.point.x;
                var pz = (int)raycastHit.point.z;

                bool leftMouseButtonPressed = Event.current.type == EventType.MouseDown && Event.current.button == 0;
                if(selectedTile.x < 0 || selectedTile.z < 0) {
                    var py = terrainData.GetHeight(px,pz);
                    Handles.DrawWireCube(new(px + 0.5f,py,pz + 0.5f),new(1f,0f,1f));
                    Handles.Label(new(px + 0.5f,py,pz + 0.5f),$"x: {px},z: {pz}");
                    if(leftMouseButtonPressed) selectedTile = new() { x = px,z = pz };
                }
                else if(!leftMouseButtonPressed) {
                    var py = terrainData.GetHeight(px,pz);
                    using(new Handles.DrawingScope(Matrix4x4.identity)) {
                        Handles.color = Color.yellow;
                        Handles.DrawWireCube(new(px + 0.5f,py,pz + 0.5f),new(1f,0f,1f));
                        Handles.Label(new(px + 0.5f,py,pz + 0.5f),$"x: {px},z: {pz}");
                    }
                }
                else if(leftMouseButtonPressed && Event.current.control) {
                    var index = nextSelectedTiles.FindIndex((point) => point.x == px && point.z == pz);
                    if(index >= 0) nextSelectedTiles.RemoveAt(index);
                    else nextSelectedTiles.Add(new() { x = px,z = pz });
                }
                else if(leftMouseButtonPressed && Event.current.shift) {
                    nextSelectedTiles.Clear();
                    var firstX = Mathf.Min(selectedTile.x,px);
                    var lastX = Mathf.Max(selectedTile.x,px);
                    var firstZ = Mathf.Min(selectedTile.z,pz);
                    var lastZ = Mathf.Max(selectedTile.z,pz);
                    for(int z = firstZ;z <= lastZ;z += 1) {
                        for(int x = firstX;x <= lastX;x += 1) {
                            if(selectedTile.x == x && selectedTile.z == z) continue;
                            nextSelectedTiles.Add(new() { x = x,z = z });
                        }
                    }
                }
            }

            if(selectedTile.x >= 0 && selectedTile.z >= 0) {
                var py = terrainData.GetHeight(selectedTile);
                Handles.DrawWireCube(new(selectedTile.x + 0.5f,py,selectedTile.z + 0.5f),new(1f,0f,1f));
                foreach(var tile in nextSelectedTiles) {
                    var nextPy = terrainData.GetHeight(tile);
                    Handles.DrawWireCube(new(tile.x + 0.5f,nextPy,tile.z + 0.5f),new(1f,0f,1f));
                }

                Vector3 sliderPosition = new(selectedTile.x + 0.5f,py,selectedTile.z + 0.5f);
                EditorGUI.BeginChangeCheck();
                var newSliderPosition = Handles.Slider(sliderPosition,Vector3.up);
                if(EditorGUI.EndChangeCheck()) {
                    var diff = newSliderPosition - sliderPosition;
                    terrainData.SetHeight(selectedTile,terrainData.GetHeight(selectedTile) + (int)diff.y);
                    foreach(var tile in nextSelectedTiles) {
                        terrainData.SetHeight(tile,terrainData.GetHeight(tile) + (int)diff.y);
                    }
                    UpdateTerrain();
                }
            }
            Handles.PositionHandle(new(-1f,0f,-1f),Quaternion.identity);
        }
        preview.EndAndDrawPreview(pos);

        if(GUILayout.Button("Reset Camera",GUILayout.ExpandWidth(false))) {
            RestoreCameraTransform();
        }
        if(GUILayout.Button("New Terrain",GUILayout.ExpandWidth(false))) {
            terrainData = new() { sizeX = editTerrainSizeX,sizeZ = editTerrainSizeZ,heights = new int[editTerrainSizeX * editTerrainSizeZ],types = new int[editTerrainSizeX * editTerrainSizeZ] };
            UpdateTerrain();
        }
        GUILayout.Label($"Terrain Size: ({terrainData.sizeX},{terrainData.sizeZ})");

        using(new GUILayout.HorizontalScope()) {
            if(GUILayout.Button("Resize Terrain",GUILayout.ExpandWidth(false))) {
                terrainData.Resize(editTerrainSizeX,editTerrainSizeZ);
                UpdateTerrain();
            }
            var stringEditTerrainSizeX = GUILayout.TextField(editTerrainSizeX.ToString(),GUILayout.ExpandWidth(false));
            int.TryParse(stringEditTerrainSizeX,out editTerrainSizeX);
            var stringEditTerrainSizeZ = GUILayout.TextField(editTerrainSizeZ.ToString(),GUILayout.ExpandWidth(false));
            int.TryParse(stringEditTerrainSizeZ,out editTerrainSizeZ);
        }

        if(GUILayout.Button("Save Terrain",GUILayout.ExpandWidth(false))) {
            var path = EditorUtility.SaveFilePanel("Save Terrain Asset","Assets/Resources/Levels","Default","asset");
            if(!string.IsNullOrEmpty(path)) {
                var fileName = Path.GetFileName(path);
                path = $"Assets/Resources/Levels/{fileName}";
                AssetDatabase.CreateAsset(terrainMesh.CloneData(),path);
                TextAsset jsonAsset = new(terrainData.ToJsonString()) { name = "TerrainData" };
                AssetDatabase.AddObjectToAsset(jsonAsset,path);
                AssetDatabase.SaveAssets();
            }
        }
        if(GUILayout.Button("Load Terrain",GUILayout.ExpandWidth(false))) {
            var path = EditorUtility.OpenFilePanel("Open Terrain Asset","Assets/Resources/Levels","asset");
            if(!string.IsNullOrEmpty(path)) {
                var fileName = Path.GetFileName(path);
                path = $"Assets/Resources/Levels/{fileName}";
                terrainData = SimpleTerrainData.FromJsonString(AssetDatabase.LoadAssetAtPath<TextAsset>(path).text);
                UpdateTerrain();
            }
        }
        Repaint();
    }
}
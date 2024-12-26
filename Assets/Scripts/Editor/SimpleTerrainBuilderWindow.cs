using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SimpleTerrainBuilderWindow : EditorWindow {
    private PreviewRenderUtility previewRenderUtility;

    [MenuItem("Custom Editor Tools/TerreinBuilderWindow")]
    private static void Init() {
        var window = GetWindow<SimpleTerrainBuilderWindow>();
        window.Show();
    }

    private void OnEnable() {
        previewRenderUtility = new();
        previewRenderUtility.AddSingleGO(GameObject.CreatePrimitive(PrimitiveType.Cube));
        previewRenderUtility.camera.transform.position = new(0f,0f,-10f);
    }

    private void OnDisable() {
        previewRenderUtility.Cleanup();
    }

    //private void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;

    //private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

    /*private void HandleFilePanel(string filePath) {
        if(!string.IsNullOrEmpty(filePath)) {
            terreinData = TerrainData.FromJsonFile(filePath);
            terreinDataFilePath = filePath;
        }
    }*/

    private void OnGUI() {
        Rect pos = new(0,0,position.width,position.height);
        previewRenderUtility.BeginPreview(pos,EditorStyles.helpBox);
        previewRenderUtility.Render();
        previewRenderUtility.EndAndDrawPreview(pos);

        //Handles.Slider(new(100f,100f,0f),Vector3.up);

        /*if(GUILayout.Button("Load Terrein Data")) {

        }*/

        //Handles.SetCamera(camera);
        //Handles.DrawCamera(new(0,0,position.width,position.height),camera);
        //SceneView.RepaintAll();

            /*GUILayout.BeginHorizontal();
            if(GUILayout.Button("Load Terrein Data")) {
                var filePath = EditorUtility.OpenFilePanel("Load Terrein Data","Assets/Resources/Levels","json");
                HandleFilePanel(filePath);
            }
            if(GUILayout.Button("New Terrein Data")) {
                var filePath = EditorUtility.SaveFilePanel("Load Terrein Data","Assets/Resources/Levels","Default","json");
                HandleFilePanel(filePath);
            }
            GUILayout.TextField(terreinDataFilePath);
            GUILayout.EndHorizontal();
            editingTerreinData = GUILayout.Toggle(editingTerreinData,"Edit Terrein","Button");
            if(editingTerreinData && string.IsNullOrEmpty(terreinDataFilePath)) {
                EditorUtility.DisplayDialog("Error!","Load or create terrain data first.","OK");
                editingTerreinData = false;
            }
            if(GUILayout.Button("Test")) {
                previewScene = EditorSceneManager.NewPreviewScene();
                SceneView.lastActiveSceneView.camera.scene = previewScene;
            }*/

            //Handles.DrawCamera(new(0,0,200,200),SceneView.lastActiveSceneView.camera);
    }

    /*private void OnSceneGUI(SceneView sceneView) {
        if(!editingTerreinData) return;
        //Handles.Slider(new(1f,2f,1f),Vector3.up);
    }*/
}
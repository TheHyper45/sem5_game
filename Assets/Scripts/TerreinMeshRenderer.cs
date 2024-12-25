using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TerreinMeshRenderer : MonoBehaviour {
    [SerializeField]
    private TextAsset terreinDataAsset;

    private void OnEnable() {
        var mesh = TerrainBuilder.BuildTerrainMesh(TerrainData.FromJsonString(terreinDataAsset.text));
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
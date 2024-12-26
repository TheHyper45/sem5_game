using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class SimpleTerrain : MonoBehaviour {
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material material;
    [SerializeField]
    private TextAsset dataAsset;

    [NonSerialized,HideInInspector]
    public SimpleTerrainData data;
    public static SimpleTerrain instance;

    private void Awake() {
        if(!Application.IsPlaying(gameObject)) return;
        instance = this;
        data = SimpleTerrainData.FromJsonString(dataAsset.text);
        gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void OnDestroy() {
        if(!Application.IsPlaying(gameObject)) return;
        instance = null;
    }

    private void Update() {
        RenderParams renderParams = new(material) { receiveShadows = true,shadowCastingMode = ShadowCastingMode.On };
        Graphics.RenderMesh(renderParams,mesh,0,Matrix4x4.TRS(transform.position,transform.rotation,transform.lossyScale));
    }
}
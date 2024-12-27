using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

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
        //@TODO: This should be cached.
        RenderParams renderParams = new(material) { receiveShadows = true,shadowCastingMode = ShadowCastingMode.On };
        Graphics.RenderMesh(renderParams,mesh,0,Matrix4x4.TRS(transform.position,transform.rotation,transform.lossyScale));
    }

    /*private class TileComparer : IComparer<SimpleTerrainVector2Int> {
        public readonly Dictionary<SimpleTerrainVector2Int,float> fScore = new();

        public int Compare(SimpleTerrainVector2Int a,SimpleTerrainVector2Int b) {
            float scoreA = float.PositiveInfinity;
            if(fScore.ContainsKey(a)) scoreA = fScore[a];
            float scoreB = float.PositiveInfinity;
            if(fScore.ContainsKey(b)) scoreB = fScore[b];
            return scoreA.CompareTo(scoreB);
        }
    }

    private readonly SimpleTerrainVector2Int[] tileNeighbourOffsets = new SimpleTerrainVector2Int[] {
        new() { x = 1,z = 0},
        new() { x = 1,z = 1},
        new() { x = 0,z = 1},
        new() { x = -1,z = 1},
        new() { x = -1,z = 0},
        new() { x = -1,z = -1},
        new() { x = 0,z = -1},
        new() { x = 1,z = -1},
    };

    private List<SimpleTerrainVector2Int> GetTileNeighbours(SimpleTerrainVector2Int point) {
        var height = data.GetHeight(point);
        List<SimpleTerrainVector2Int> result = new();
        foreach(var offset in tileNeighbourOffsets) {
            if(point.x + offset.x < 0 || point.x + offset.x >= data.sizeX) continue;
            if(point.z + offset.z < 0 || point.z + offset.z >= data.sizeZ) continue;
            SimpleTerrainVector2Int neighbour = point + offset;
            var neighbourHeight = data.GetHeight(neighbour);
            if(neighbourHeight == height) result.Add(neighbour);
        }
        return result;
    }

    //Implements A* algorithm from https://en.wikipedia.org/wiki/A*_search_algorithm.
    public List<SimpleTerrainVector2Int> FindPath(SimpleTerrainVector2Int start,SimpleTerrainVector2Int end) {
        Dictionary<SimpleTerrainVector2Int,SimpleTerrainVector2Int> cameFromDict = new();
        TileComparer tileComparer = new();
        SortedSet<SimpleTerrainVector2Int> openSet = new(tileComparer) { start };
        Dictionary<SimpleTerrainVector2Int,float> gScore = new() { [start] = 0f };

        while(openSet.Count > 0) {
            var current = openSet.First();
            if(current == end) {
                List<SimpleTerrainVector2Int> result = new() { current };
                while(cameFromDict.ContainsKey(current)) {
                    current = cameFromDict[current];
                    result.Prepend(current);
                }
                return result;
            }
            openSet.Remove(current);
            foreach(var neighbour in GetTileNeighbours(current)) {
                float gScoreCurrent = float.PositiveInfinity;
                if(gScore.ContainsKey(current)) gScoreCurrent = gScore[current];
                float gScoreNeighbour = float.PositiveInfinity;
                if(gScore.ContainsKey(neighbour)) gScoreNeighbour = gScore[neighbour];

                var newGScore = gScoreCurrent + SimpleTerrainVector2Int.EuclideanDistance(current,neighbour);
                if(newGScore < gScoreNeighbour) {
                    cameFromDict[neighbour] = current;
                    gScore[neighbour] = newGScore;
                    tileComparer.fScore[neighbour] = newGScore + SimpleTerrainVector2Int.EuclideanDistance(neighbour,end);
                    if(!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return new();
    }*/
}
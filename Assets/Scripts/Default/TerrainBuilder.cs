using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TerrainBuilder : MonoBehaviour {
    private void OnEnable() {
        LoadTerrainDataFrom($"Assets/Resources/Levels/{SceneManager.GetActiveScene().name}.json");
    }

    public void SetTerrainData(TerrainData terrainData) {
        var mesh = BuildTerrainMesh(terrainData);
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void LoadTerrainDataFrom(string path) {
        var data = TerrainData.FromJsonFile(path);
        var mesh = BuildTerrainMesh(data);
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private static readonly float TextureTileWidth = 32f / 256f;
    private static readonly float TextureTileHeight = 32f / 256f;

    private static readonly Vector2[,] QuadUVs = new Vector2[,] {
        {new(0f,1f),new(1f,1f),new(1f,0f),new(0f,0f)},
        {new(1f,1f),new(1f,0f),new(0f,0f),new(0f,1f)},
        {new(1f,0f),new(0f,0f),new(0f,1f),new(1f,1f)},
        {new(0f,1f),new(1f,1f),new(1f,0f),new(0f,0f)},
        {new(1f,0f),new(0f,0f),new(0f,1f),new(1f,1f)},
        {new(0f,0f),new(0f,1f),new(1f,1f),new(1f,0f)},
    };

    public static Mesh BuildTerrainMesh(TerrainData terrainData) {
        List<Vector3> vertexPositions = new();
        List<Vector3> vertexUVs = new();

        void AddUVs(int layer,bool randomUVs = false) {
            int index = randomUVs ? UnityEngine.Random.Range(0,QuadUVs.GetLength(1)) : 0;
            for(int i = 0;i < QuadUVs.GetLength(0);i += 1) {
                var uv = QuadUVs[i,index];
                vertexUVs.Add(new(uv.x,uv.y,layer));
            }
        }

        for(int z = 0;z < terrainData.sizeZ;z += 1) {
            for(int x = 0;x < terrainData.sizeX;x += 1) {
                int height = terrainData.GetHeight(x,z);

                vertexPositions.Add(new(x + 0f,height,z + 0f));
                vertexPositions.Add(new(x + 0f,height,z + 1f));
                vertexPositions.Add(new(x + 1f,height,z + 1f));
                vertexPositions.Add(new(x + 0f,height,z + 0f));
                vertexPositions.Add(new(x + 1f,height,z + 1f));
                vertexPositions.Add(new(x + 1f,height,z + 0f));
                AddUVs(0,true);

                for(int y = height;y > terrainData.GetHeight(x,z - 1);y -= 1) {
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    AddUVs(y == height ? 2 : 1,y < height);
                }
                for(int y = height;y > terrainData.GetHeight(x,z + 1);y -= 1) {
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    AddUVs(y == height ? 2 : 1,y < height);
                }
                for(int y = height;y > terrainData.GetHeight(x - 1,z);y -= 1) {
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    AddUVs(y == height ? 2 : 1,y < height);
                }
                for(int y = height;y > terrainData.GetHeight(x + 1,z);y -= 1) {
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    AddUVs(y == height ? 2 : 1,y < height);
                }
            }
        }

        List<int> triangleVertexIndicies = new();
        for(int i = 0;i < vertexPositions.Count;i += 1) {
            triangleVertexIndicies.Add(i);
        }

        Mesh mesh = new() { name = "Terrain" };
        mesh.SetVertices(vertexPositions);
        mesh.SetUVs(0,vertexUVs);
        mesh.SetTriangles(triangleVertexIndicies,0);
        mesh.UploadMeshData(false);
        return mesh;
    }
}

[Serializable]
public struct TerrainVector2Int {
    public int x;
    public int z;

    public static bool operator ==(TerrainVector2Int a,TerrainVector2Int b) {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(TerrainVector2Int a,TerrainVector2Int b) {
        return !(a == b);
    }

    public static TerrainVector2Int operator +(TerrainVector2Int a,TerrainVector2Int b) {
        return new() { x = a.x + b.x,z = a.z + b.z };
    }

    public static TerrainVector2Int operator-(TerrainVector2Int a,TerrainVector2Int b) {
        return new() { x = a.x - b.x,z = a.z - b.z };
    }

    public override readonly bool Equals(object obj) {
        return obj is TerrainVector2Int @int && x == @int.x && z == @int.z;
    }

    public override readonly int GetHashCode() {
        return HashCode.Combine(x,z);
    }
}

[Serializable]
public struct TerrainData {
    public int sizeX;
    public int sizeZ;
    public int[] heights;

    public readonly void SetHeight(TerrainVector2Int coords,int value) {
        SetHeight(coords.x,coords.z,value);
    }

    public readonly void SetHeight(int x,int z,int value) {
        heights[Mathf.Clamp(z,0,sizeZ - 1) * sizeX + Mathf.Clamp(x,0,sizeX - 1)] = value;
    }

    public readonly int GetHeight(TerrainVector2Int coords) {
        return GetHeight(coords.x,coords.z);
    }

    public readonly int GetHeight(int x,int z) {
        return heights[Mathf.Clamp(z,0,sizeZ - 1) * sizeX + Mathf.Clamp(x,0,sizeX - 1)];
    }

    public void Resize(int newSizeX,int newSizeZ) {
        newSizeX = Mathf.Max(0,newSizeX);
        newSizeZ = Mathf.Max(0,newSizeZ);
        if(sizeX == newSizeX && sizeZ == newSizeZ) return;

        int[] newHeights = new int[newSizeX * newSizeZ];
        for(int z = 0;z < newSizeZ;z += 1) {
            if(z >= sizeZ) continue;
            for(int x = 0;x < newSizeX;x += 1) {
                if(x >= sizeX) continue;
                newHeights[z * newSizeX + x] = heights[z * sizeX + x];
            }
        }
        sizeX = newSizeX;
        sizeZ = newSizeZ;
        heights = newHeights;
    }

    public readonly void ToJsonFile(string path) {
        File.WriteAllText(path,JsonUtility.ToJson(this));
    }

    public static TerrainData FromJsonFile(string path) {
        try {
            return JsonUtility.FromJson<TerrainData>(File.ReadAllText(path));
        }
        catch(FileNotFoundException) {
            return new() { sizeX = 1,sizeZ = 1,heights = new int[1] };
        }
    }
}
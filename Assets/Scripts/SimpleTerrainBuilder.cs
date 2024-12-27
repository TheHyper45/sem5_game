using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class SimpleTerrainBuilder : MonoBehaviour {
    public SimpleTerrainData terrainData;

    private void OnEnable() => RefreshTerreinMesh();

    public void RefreshTerreinMesh() {
        var mesh = BuildTerrainMesh(terrainData);
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private static readonly Vector2[,] QuadUVs = new Vector2[,] {
        {new(0f,1f),new(1f,1f),new(1f,0f),new(0f,0f)},
        {new(1f,1f),new(1f,0f),new(0f,0f),new(0f,1f)},
        {new(1f,0f),new(0f,0f),new(0f,1f),new(1f,1f)},
        {new(0f,1f),new(1f,1f),new(1f,0f),new(0f,0f)},
        {new(1f,0f),new(0f,0f),new(0f,1f),new(1f,1f)},
        {new(0f,0f),new(0f,1f),new(1f,1f),new(1f,0f)},
    };

    private readonly static int TextureGrassTopLayerIndex = 0;
    private readonly static int TextureStoneLayerIndex = 1;
    private readonly static int TextureGrassSideLayerindex = 2;


    //@TODO: Merge smaller quads into bigger ones.
    public static Mesh BuildTerrainMesh(SimpleTerrainData terrainData) {
        List<Vector3> vertexPositions = new();
        List<Vector3> vertexUVs = new();
        List<Vector3> normals = new();

        void AddUVs(int layer,bool randomUVs = false) {
            int index = randomUVs ? UnityEngine.Random.Range(0,QuadUVs.GetLength(1)) : 0;
            for(int i = 0;i < QuadUVs.GetLength(0);i += 1) {
                var uv = QuadUVs[i,index];
                vertexUVs.Add(new(uv.x,uv.y,layer));
            }
        }

        void AddNormals(Vector3 normal,int count = 6) {
            for(int i = 0;i < count;i += 1) {
                normals.Add(normal);
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
                AddUVs(TextureGrassTopLayerIndex,true);
                AddNormals(Vector3.up);

                for(int y = height;y > terrainData.GetHeight(x,z - 1);y -= 1) {
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    AddUVs(y == height ? TextureGrassSideLayerindex : TextureStoneLayerIndex,y < height);
                    AddNormals(Vector3.back);
                }
                for(int y = height;y > terrainData.GetHeight(x,z + 1);y -= 1) {
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    AddUVs(y == height ? TextureGrassSideLayerindex : TextureStoneLayerIndex,y < height);
                    AddNormals(Vector3.forward);
                }
                for(int y = height;y > terrainData.GetHeight(x - 1,z);y -= 1) {
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    AddUVs(y == height ? TextureGrassSideLayerindex : TextureStoneLayerIndex,y < height);
                    AddNormals(Vector3.left);
                }
                for(int y = height;y > terrainData.GetHeight(x + 1,z);y -= 1) {
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    AddUVs(y == height ? TextureGrassSideLayerindex : TextureStoneLayerIndex,y < height);
                    AddNormals(Vector3.right);
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
        mesh.SetNormals(normals);
        mesh.UploadMeshData(false);
        return mesh;
    }
}

[Serializable]
public struct SimpleTerrainVector2Int {
    public int x;
    public int z;

    public static bool operator ==(SimpleTerrainVector2Int a,SimpleTerrainVector2Int b) {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(SimpleTerrainVector2Int a,SimpleTerrainVector2Int b) {
        return !(a == b);
    }

    public static SimpleTerrainVector2Int operator +(SimpleTerrainVector2Int a,SimpleTerrainVector2Int b) {
        return new() { x = a.x + b.x,z = a.z + b.z };
    }

    public static SimpleTerrainVector2Int operator-(SimpleTerrainVector2Int a,SimpleTerrainVector2Int b) {
        return new() { x = a.x - b.x,z = a.z - b.z };
    }

    public static float EuclideanDistance(SimpleTerrainVector2Int a,SimpleTerrainVector2Int b) {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));
    }

    public override readonly bool Equals(object obj) {
        return obj is SimpleTerrainVector2Int @int && x == @int.x && z == @int.z;
    }

    public override readonly int GetHashCode() {
        return HashCode.Combine(x,z);
    }
}

[Serializable]
public struct SimpleTerrainData {
    public int sizeX;
    public int sizeZ;
    public int[] heights;

    public readonly void SetHeight(SimpleTerrainVector2Int coords,int value) {
        SetHeight(coords.x,coords.z,value);
    }

    public readonly void SetHeight(int x,int z,int value) {
        heights[Mathf.Clamp(z,0,sizeZ - 1) * sizeX + Mathf.Clamp(x,0,sizeX - 1)] = value;
    }

    public readonly int GetHeight(SimpleTerrainVector2Int coords) {
        return GetHeight(coords.x,coords.z);
    }

    public readonly int GetHeight(int x,int z) {
        return heights[Mathf.Clamp(z,0,sizeZ - 1) * sizeX + Mathf.Clamp(x,0,sizeX - 1)];
    }

    public void Resize(int newSizeX,int newSizeZ) {
        newSizeX = Mathf.Max(0,newSizeX);
        newSizeZ = Mathf.Max(0,newSizeZ);
        if(sizeX == newSizeX && sizeZ == newSizeZ) return;

        var newHeights = new int[newSizeX * newSizeZ];
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

    public static SimpleTerrainData FromJsonString(string text) {
        return JsonUtility.FromJson<SimpleTerrainData>(text);
    }

    public static SimpleTerrainData FromJsonFile(string path) {
        try {
            return JsonUtility.FromJson<SimpleTerrainData>(File.ReadAllText(path));
        }
        catch(FileNotFoundException) {
            File.WriteAllText(path,"{\"sizeX\": 1,\"sizeZ\": 1,\"heights\": [0]}");
            return new() { sizeX = 1,sizeZ = 1,heights = new int[1] };
        }
    }
}
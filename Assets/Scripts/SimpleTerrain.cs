using System;
using UnityEngine;
using System.Collections.Generic;

public static class SimpleTerrain {
    private static readonly int TextureGrassTopIndex = 0;
    private static readonly int TextureStoneIndex = 1;
    private static readonly int TextureGrassSideIndex = 2;

    //First is texture index, second are UV indicies.
    private static readonly Vector2[,] TextureUVs = new Vector2[,] {
        {new(0f,1f),new(0.125f,1f),new(0.125f,1f - 0.125f),new(0f,1f),new(0.125f,1f - 0.125f),new(0f,1f - 0.125f)},
        {new(0.125f,1f),new(0.25f,1f),new(0.25f,1f - 0.125f),new(0.125f,1f),new(0.25f,1f - 0.125f),new(0.125f,1f - 0.125f)},
        {new(0.25f,1f),new(0.375f,1f),new(0.375f,1f - 0.125f),new(0.25f,1f),new(0.375f,1f - 0.125f),new(0.25f,1f - 0.125f)},
    };

    private static void AddUVs(List<Vector2> vertexUVs,int textureIndex) {
        for(int i = 0;i < 6;i += 1) vertexUVs.Add(TextureUVs[textureIndex,i]);
    }

    public static void RebuildMesh(SimpleTerrainData terrainData,Mesh mesh) {
        List<Vector3> vertexPositions = new();
        List<Vector2> vertexUVs = new();
        List<Vector3> normals = new();

        for(int z = 0;z < terrainData.sizeZ;z += 1) {
            for(int x = 0;x < terrainData.sizeX;x += 1) {
                int height = terrainData.GetHeight(x,z);

                vertexPositions.Add(new(x + 0f,height,z + 0f));
                vertexPositions.Add(new(x + 0f,height,z + 1f));
                vertexPositions.Add(new(x + 1f,height,z + 1f));
                vertexPositions.Add(new(x + 0f,height,z + 0f));
                vertexPositions.Add(new(x + 1f,height,z + 1f));
                vertexPositions.Add(new(x + 1f,height,z + 0f));
                AddUVs(vertexUVs,TextureGrassTopIndex);
                normals.AddMultiple(Vector3.up,6);

                for(int y = height;y > terrainData.GetHeight(x,z - 1);y -= 1) {
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    AddUVs(vertexUVs,y == height ? TextureGrassSideIndex : TextureStoneIndex);
                    normals.AddMultiple(Vector3.back,6);
                }
                for(int y = height;y > terrainData.GetHeight(x,z + 1);y -= 1) {
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    AddUVs(vertexUVs,y == height ? TextureGrassSideIndex : TextureStoneIndex);
                    normals.AddMultiple(Vector3.forward,6);
                }
                for(int y = height;y > terrainData.GetHeight(x - 1,z);y -= 1) {
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 0f));
                    vertexPositions.Add(new(x + 0f,y - 1f,z + 1f));
                    AddUVs(vertexUVs,y == height ? TextureGrassSideIndex : TextureStoneIndex);
                    normals.AddMultiple(Vector3.left,6);
                }
                for(int y = height;y > terrainData.GetHeight(x + 1,z);y -= 1) {
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 0f,z + 0f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 1f));
                    vertexPositions.Add(new(x + 1f,y - 1f,z + 0f));
                    AddUVs(vertexUVs,y == height ? TextureGrassSideIndex : TextureStoneIndex);
                    normals.AddMultiple(Vector3.right,6);
                }
            }
        }

        List<int> triangleVertexIndicies = new();
        triangleVertexIndicies.AddIota(vertexPositions.Count);

        mesh.Clear();
        mesh.name = "Terrain";
        mesh.SetVertices(vertexPositions);
        mesh.SetUVs(0,vertexUVs);
        mesh.SetTriangles(triangleVertexIndicies,0);
        mesh.SetNormals(normals);
        mesh.UploadMeshData(false);
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
    public int[] types;

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

    public readonly string ToJsonString() {
        return JsonUtility.ToJson(this);
    }

    public static SimpleTerrainData FromJsonString(string text) {
        var result = JsonUtility.FromJson<SimpleTerrainData>(text);
        result.types ??= new int[result.sizeX * result.sizeZ];
        return result;
    }
}
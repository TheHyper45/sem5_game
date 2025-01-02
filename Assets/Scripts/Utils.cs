using UnityEngine;
using System.Collections.Generic;

public static class Utils {
    public static T RandomElement<T>(this List<T> list) {
        return list[Random.Range(0,list.Count)];
    }

    public static T RandomElement<T>(this T[] list) {
        return list[Random.Range(0,list.Length)];
    }

    public static void AddMultiple<T>(this List<T> list,T element,int count) {
        for(int i = 0;i < count;i += 1) {
            list.Add(element);
        }
    }

    public static void AddIota(this List<int> list,int count) {
        for(int i = 0;i < count;i += 1) {
            list.Add(i);
        }
    }

    public static Mesh CloneData(this Mesh mesh) {
        Mesh result = new() { name = mesh.name };
        result.SetVertices(mesh.vertices);
        List<Vector3> uvs = new();
        mesh.GetUVs(0,uvs);
        result.SetUVs(0,uvs);
        result.SetTriangles(mesh.triangles,0);
        result.SetNormals(mesh.normals);
        result.UploadMeshData(false);
        return result;
    }
}
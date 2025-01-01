using UnityEngine;
using System.Collections.Generic;

public static class Utils {
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
using System;
using UnityEngine;

public static class MathEx {
    public static Vector3 Lerp(Vector3 a,Vector3 b,float t) {
        return new(Mathf.Lerp(a.x,b.x,t),Mathf.Lerp(a.y,b.y,t),Mathf.Lerp(a.z,b.z,t));
    }

    public static Vector3 BezierQuadratic(Vector3 p0,Vector3 p1,Vector3 p2,float t) {
        return p1 + (1f - t) * (1f - t) * (p0 - p1) + t * t * (p2 - p1);
    }

    public static Vector3 Mul(Vector3 a,Vector3 b) {
        return new(a.x * b.x,a.y * b.y,a.z * b.z);
    }

    public static float MapRange(float value,float oldA,float oldB,float newA,float newB) {
        var per = (value - oldA) / (oldB - oldA);
        return newA * (1f - per) + newB * per;
    }
}

public static class Utils {

    public static void Noexcept(Action action) {
        try {
            action();
        }
        catch(Exception) {

        }
    }
}

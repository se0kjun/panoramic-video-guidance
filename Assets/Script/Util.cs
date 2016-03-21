using UnityEngine;

public class Util {
    public static readonly int MOVIE_FRAME = 10;
    public static readonly int PLANE_WIDTH = 10;
    public static readonly int PLANE_HEIGHT = 10;

    public static Vector2 GetUV(VideoXMLWrapper video, int pos_x, int pos_y)
    {
        return new Vector2((float)pos_x / (float)video.Width, (float)(video.Height - pos_y) / (float)video.Height);
    }

    public static Vector3 UvTo3D(Vector2 uv, GameObject gameObj)
    {
        Mesh mesh = gameObj.GetComponent<MeshFilter>().mesh;
        int[] tris = mesh.triangles;
        Vector2[] uvs = mesh.uv;
        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector2 u1 = uvs[tris[i]];
            Vector2 u2 = uvs[tris[i + 1]];
            Vector2 u3 = uvs[tris[i + 2]];

            float a = CalcArea(u1, u2, u3); if (a == 0) continue;
            float a1 = CalcArea(u2, u3, uv) / a; if (a1 < 0) continue;
            float a2 = CalcArea(u3, u1, uv) / a; if (a2 < 0) continue;
            float a3 = CalcArea(u1, u2, uv) / a; if (a3 < 0) continue;

            Vector3 p3D = a1 * verts[tris[i]] + a2 * verts[tris[i + 1]] + a3 * verts[tris[i + 2]];
            return gameObj.transform.TransformPoint(p3D);
        }

        return Vector3.zero;
    }

    public static float CalcArea(Vector2 p1, Vector2 p2, Vector3 p3)
    {
        Vector2 v1 = new Vector2(p1.x - p3.x, p1.y - p3.y);
        Vector2 v2 = new Vector2(p2.x - p3.x, p2.y - p3.y);
        return (v1.x * v2.y - v1.y * v2.x) / 2;
    }

}

using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class coneGenerator : MonoBehaviour
{
    public float height = 5f;       // Height of the cone
    public float radius = 2f;       // Radius of the cone's base
    public int segments = 20;       // Number of segments around the base

    void Start()
    {
        GenerateCone();
    }

    void GenerateCone()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        // Vertices
        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = new Vector3(0, height, 0); // Tip of the cone
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            vertices[i + 1] = new Vector3(x, 0, z);
        }
        vertices[segments + 1] = Vector3.zero; // Center of the base

        // Triangles
        int[] triangles = new int[segments * 3 + segments * 3];
        for (int i = 0; i < segments; i++)
        {
            // Side triangles
            triangles[i * 3] = 0; // Tip of the cone
            triangles[i * 3 + 1] = i + 1; // Current base vertex
            triangles[i * 3 + 2] = (i + 1) % segments + 1; // Next base vertex

            // Base triangles
            triangles[segments * 3 + i * 3] = segments + 1; // Center of the base
            triangles[segments * 3 + i * 3 + 1] = (i + 1) % segments + 1; // Next base vertex
            triangles[segments * 3 + i * 3 + 2] = i + 1; // Current base vertex
        }

        // UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius, vertices[i].z / radius);
        }

        // Assign to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}

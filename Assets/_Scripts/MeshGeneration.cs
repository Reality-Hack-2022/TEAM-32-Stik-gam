using System;
using UnityEngine;


public struct RawMesh
{
    public Vector3[] knotLoc;
    public Vector3[] knotTan;
    public int knotCount;
}

public class MeshGeneration : MonoBehaviour
{
    //DEFINING OBJECT
    public Vector3[] knotLocations;
    public Vector3[] knotTangents;
    public int knotCount = 0;

    //specifer for the object, probably passed in later
    //INPUT INTO MESH GEN
    public int x_size = 7; //cylinder division count
    public float radius;

    //mesh definitions
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Awake()
    {
        mesh = new Mesh();
    }
    private void CreateShape()
    {
        // initialize the array to the size
        int z_size_new = knotCount - 1;  //TODO: NEED TO ADJUST THIS IN FINAL  
        int size = (x_size + 1) * (z_size_new + 1) + 2; //plus 2 for the start & end
        vertices = new Vector3[size];
        triangles = new int[(x_size * (z_size_new + 1) + x_size * 2) * 6]; //?? works lol

        //create most of the vertices
        int i = 1; //start vert stored elsewhere

        Quaternion rot = new Quaternion();
        for (int z = 0; z < z_size_new + 1; z++)
        {
            for (int x = 0; x < x_size + 1; x++)
            {

                float angle = ((float)x) / x_size * Mathf.PI * 2;
                float x1 = Mathf.Sin(-angle) * radius;
                float y1 = Mathf.Cos(-angle) * radius;
                Vector3 vec = new Vector3(x1, 0, y1);
                rot.SetFromToRotation(Vector3.up, knotTangents[z]);
                vec = rot * vec;
                vec = vec + knotLocations[z];
                vertices[i] = vec;
                i++;
            }
        }

        //create the starting vertice
        Vector3 sum_start = new Vector3();
        Vector3 sum_end = new Vector3();

        for (int j = 0; j < x_size + 1; j++) //get the centroid
        {
            sum_start += vertices[j + 1];
            sum_end += vertices[i - 1 - j];
        }
        Vector3 firstvert = sum_start / (float)(x_size + 1);
        Vector3 lastvert = sum_end / (float)(x_size + 1);
        vertices[0] = firstvert;
        vertices[i] = lastvert;

        //we multiply the value by j as this is our offset to properly index into this shape
        int vertx = 1;
        int tris = 0;
        for (int z = 0; z < z_size_new; z++)
        {
            for (int x = 0; x < x_size; x++)
            {
                triangles[tris] = vertx + 0;
                triangles[tris + 1] = vertx + x_size + 1;
                triangles[tris + 2] = vertx + 1;
                triangles[tris + 3] = vertx + 1;
                triangles[tris + 4] = vertx + x_size + 1;
                triangles[tris + 5] = vertx + x_size + 2;

                vertx++;
                tris += 6;
            }
            vertx++;
        }
        vertx += x_size + 1;

        //create the cap
        for (int x = 0; x < x_size; x++) // go full circle?
        {
            //front cap
            triangles[tris] = 0;
            triangles[tris + 1] = x % x_size + 1;
            triangles[tris + 2] = (x + 1) % x_size + 1;
            tris += 3;

            //back cap
            triangles[tris] = vertx;
            triangles[tris + 1] = vertx - 1 - (x);
            triangles[tris + 2] = vertx - 1 - ((x + 1) % x_size);
            tris += 3;
        }

        UpdateMesh();

    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    //function called to return a mesh
    public Mesh CreateMesh()
    {
        //mesh = new Mesh();
        //mFilter.mesh = mesh;

        CreateShape();

        return mesh;
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSpawner : MonoBehaviour
{
    //----------------------
    //DEFINITIONAL ELEMENTS
    public List<RawMesh> storedRawMesh; 
    public int meshCount = 0;

    //GENERATOR OPERATORS
    MeshGeneration meshGenerator; //used to generate all objects
    CatmullRomSpline spline;
    List<SplineMesh> storedSplineMesh;

    //GENERATED ELEMENTS
    public List<Mesh> meshes;
    //private bunch of colliders? 
        
    //RENDERING & DISPLAY
    MeshCollider mCollider;
    Rigidbody mRigibody;
    // Start is called before the first frame update

    private void Awake()
    {
        //attach the spline to this object
        spline = gameObject.AddComponent<CatmullRomSpline>();

        //make the mesh generator
        meshGenerator = gameObject.AddComponent<MeshGeneration>();

        //init the Lists
        storedRawMesh = new List<RawMesh>();
        storedSplineMesh = new List<SplineMesh>();
        meshes = new List<Mesh>();

    }
    public void CreateSplineMesh(List<Vector3> vecs)
    {

        //Run it on test coordinates
        Vector3[] testtest = new Vector3[vecs.Count];
        for (int i = 0; i < vecs.Count; i++)
        {
            testtest[i] = vecs[i];
        }

        bool status = AddSplineMesh(testtest);
        print("did we succesfully create the spline mesh? " + status);

    }

    //return true if successfully added
    bool AddSplineMesh(Vector3[] points)
    {
        //Only add if we have more than 2 data points
        if (points.Length < 2)
        {
            return false;
        }

        meshCount++;
        //----------------------GENERATE THE RAWMESH------------------------
        //give the spline the list of transforms generated from the user
        spline.controlPointsList = points;
        //and process them into the spline
        RawMesh rawSplineMesh = spline.QueryResults();
        storedRawMesh.Add(rawSplineMesh);

        //---------------------GENERATE MESH FILTER-------------------------
        GameObject newSplineMesh = new GameObject("SplineMesh");
        newSplineMesh.AddComponent<SplineMesh>();
        SplineMesh sMesh = newSplineMesh.GetComponent(typeof(SplineMesh)) as SplineMesh;
        MeshGeneration meshGen = newSplineMesh.GetComponent(typeof(MeshGeneration)) as MeshGeneration;
        newSplineMesh.transform.SetParent(gameObject.transform, false);

        storedSplineMesh.Add(sMesh);
        //----------------------GENERATE THE MESH---------------------------
        //set the knot locations + tangenets so we can generate the mesh along the spline
        meshGen.knotLocations = rawSplineMesh.knotLoc;
        meshGen.knotTangents = rawSplineMesh.knotTan;
        meshGen.knotCount = rawSplineMesh.knotCount;
        //create the mesh
        Mesh generated = meshGen.CreateMesh();//(newSplineMesh.mFilter, newSplineMesh.mRenderer);
        sMesh.mFilter.mesh = generated;
        meshes.Add(generated);

        return true;
    }



}

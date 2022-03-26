using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSpawner : MonoBehaviour
{
    //REMOVE LATER AS THIS TAKES INPUT
    //public Transform[] transforms;
    //----------------------
    //DEFINITIONAL ELEMENTS
    public List<RawMesh> storedRawMesh; 
    public int meshCount = 0;
    public float radius = 10.0f;

    //GENERATOR OPERATORS
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

        //init the Lists
        storedRawMesh = new List<RawMesh>();
        storedSplineMesh = new List<SplineMesh>();
        meshes = new List<Mesh>();

    }
    void Start()
    {

        //Run it on test coordinates
        /*
        Vector3[] testtest = new Vector3[transforms.Length];
        Vector3[] tt = new Vector3[transforms.Length];
        Vector3 offset = new Vector3(0.0f, 40.0f, 0.0f);
        for (int i = 0; i < transforms.Length; i++)
        {
            testtest[i] = transforms[i].position + offset;
            tt[i] = transforms[i].position;
        }


        
        AddSplineMesh(testtest);
        AddSplineMesh(tt);
        */
        List<Vector3> vectors = new List<Vector3>();
        vectors.Add(new Vector3(-0.04f, 1.18f, -1.11f));
        vectors.Add(new Vector3(-0.00f, 1.15f, -1.10f));
        vectors.Add(new Vector3(0.06f, 1.15f, -1.08f));
        vectors.Add(new Vector3(0.14f, 1.15f, -1.08f));
        vectors.Add(new Vector3(0.14f, 1.14f, -1.08f));
        vectors.Add(new Vector3(0.13f, 1.14f, -1.07f));

        //convert to list 
        Vector3[] tt = new Vector3[vectors.Count];
        for (int i = 0; i < vectors.Count; i++)
        {
            tt[i] = vectors[i];
        }
        AddSplineMesh(tt);

        generateCollision();
    }
    
    //return true if successfully added
    bool AddSplineMesh(Vector3[] points)
    {
        //Only add if we have more than 2 data points
        if (points.Length < 2)
        {
            return false;
        }

        //----------------------GENERATE THE RAWMESH------------------------
        //give the spline the list of transforms generated from the user
        spline.controlPointsList = points;
        //and process them into the spline
        RawMesh rawSplineMesh = spline.QueryResults(); //up till here, stuff is correct
        storedRawMesh.Add(rawSplineMesh);

        //---------------------GENERATE MESH FILTER-------------------------
        GameObject newSplineMesh = new GameObject("SplineMesh");
        newSplineMesh.AddComponent<SplineMesh>();
        SplineMesh sMesh = newSplineMesh.GetComponent(typeof(SplineMesh)) as SplineMesh;
        MeshGeneration meshGen = newSplineMesh.GetComponent(typeof(MeshGeneration)) as MeshGeneration;
        newSplineMesh.transform.SetParent(gameObject.transform, true);

        storedSplineMesh.Add(sMesh);
        //----------------------GENERATE THE MESH---------------------------
        //set the knot locations + tangenets so we can generate the mesh along the spline
        meshGen.radius = radius;
        meshGen.knotLocations = rawSplineMesh.knotLoc;
        meshGen.knotTangents = rawSplineMesh.knotTan;
        meshGen.knotCount = rawSplineMesh.knotCount;
        //create the mesh
        Mesh generated = meshGen.CreateMesh();//(newSplineMesh.mFilter, newSplineMesh.mRendederer);
        sMesh.mFilter.mesh = generated;
        meshes.Add(generated);

        return true;
    }

    //generate collision for every RawMesh stored
    bool generateCollision()
    {
        //Quaternion rot = new Quaternion();
        List<BoxCollider> temp = new List<BoxCollider>();
        for (int i = 0; i < storedRawMesh.Count; i++) //for each Rawmesh
        {

            RawMesh rMesh = storedRawMesh[i];

            for (int j = 0; j < rMesh.knotCount - 2; j++) //for each knot
            {
                BoxCollider boxC = gameObject.AddComponent<BoxCollider>();
                boxC.center = rMesh.knotLoc[j];
                boxC.size = new Vector3(radius * 3.5f, radius * 3.5f, radius * 3.5f);
                temp.Add(boxC);
            }

        }

        //set that colliders ignores self collison
        for (int i = 0; i < temp.Count; i++)
        {
            BoxCollider b1 = temp[i];
            for (int j = 0; j < temp.Count; j++)
            {
                BoxCollider b2 = temp[j];
                Physics.IgnoreCollision(b1, b2, true);
            }
        }

        return true;
    }

}

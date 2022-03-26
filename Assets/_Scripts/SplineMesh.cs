using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMesh : MonoBehaviour
{
    public MeshFilter mFilter;
    public MeshRenderer mRenderer;
    MeshGeneration meshGenerator; //used to generate all objects


    // Start is called before the first frame update
    private void Awake()
    {
        mFilter = gameObject.AddComponent<MeshFilter>();
        //create mesh filter + renderer and attach it to this object
        mRenderer = gameObject.AddComponent<MeshRenderer>();
        //make the mesh generator
        meshGenerator = gameObject.AddComponent<MeshGeneration>();
    }
    
    void Start()
    {


    }
}

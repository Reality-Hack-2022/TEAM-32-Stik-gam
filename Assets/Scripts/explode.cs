using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    public GameObject explodePrefab;
    void OnCollisionEnter(Collision collision)
    {
        gameObject.transform.GetChild(0).GetComponent<MeshFilter>().mesh = null;
        Instantiate(gameObject, transform);
    }
}

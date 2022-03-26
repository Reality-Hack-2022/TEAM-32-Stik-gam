using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grabbed() {
        Transform papa = this.transform.parent;
        papa.SetParent(this.transform);
    }

    public void LetGo() {
        Transform realpapa = this.transform.GetChild(0);
        this.transform.SetParent(realpapa);
    }
}

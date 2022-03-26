using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth helth = other.gameObject.GetComponent<PlayerHealth>();
        if (helth != null) {
            helth.Death();
        }
    }
}

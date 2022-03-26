using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking.Pun2
{
    public class CalibrationManager : MonoBehaviourPunCallbacks
    {
        GameObject player1Left;
        GameObject player1Right;
        GameObject player1Head;

        GameObject player2Left;
        GameObject player2Right;
        GameObject player2Head;

        // Start is called before the first frame update
        void Start()
        {
            if(PhotonNetwork.PlayerListOthers.Length == 1)
            {
                Debug.Log(PhotonNetwork.PlayerListOthers[0].CustomProperties[0].ToString());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
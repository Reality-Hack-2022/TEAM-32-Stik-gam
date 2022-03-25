using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
//Sets the color of the first MeshRenderer/SkinnedMeshRenderer found with GetComponentInChildren
//
namespace Networking.Pun2
{
    public class SetColor : MonoBehaviourPun
    {
        Color playerColor;
        public void SetColorRPC(int n)
        {
            GetComponent<PhotonView>().RPC("RPC_SetColor", RpcTarget.AllBuffered, n);
        }

        [PunRPC]
        void RPC_SetColor(int n)
        {
            switch (n)
            {
                case 1:
                    playerColor = Color.red;
                    break;
                case 2:
                    playerColor = Color.cyan;
                    break;
                case 3:
                    playerColor = Color.green;
                    break;
                case 4:
                    playerColor = Color.yellow;
                    break;
                case 5:
                    playerColor = Color.magenta;
                    break;
                default:
                    playerColor = Color.black;
                    break;
            }
            playerColor = Color.Lerp(Color.white, playerColor, 0.5f);
            if(GetComponentInChildren<MeshRenderer>() != null)
                GetComponentInChildren<MeshRenderer>().material.color = playerColor;
            else if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
                GetComponentInChildren<SkinnedMeshRenderer>().material.color = playerColor;
        }
    }
}

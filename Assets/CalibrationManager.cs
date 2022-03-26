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
        GameObject player1Center;

        GameObject player2Left;
        GameObject player2Right;
        GameObject player2Head;
        GameObject player2Center;

        // Start is called before the first frame update
        void Start()
        {
            // There are 2 players
            if(PhotonNetwork.PlayerListOthers.Length == 1)
            {
                //Debug.Log(PhotonNetwork.PlayerListOthers[0].CustomProperties[0].ToString());
                // Master player is an owner
                // TODO: Change master person's color or something here to notify

                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Debug.Log("You are a master user");
                    // Get All Oculus Objects here
                    GameObject[] handsControllers = GameObject.FindGameObjectsWithTag("Hands");
                    for(var i = 0; i < handsControllers.Length; i++)
                    {
                        if (handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_L"))
                        {
                            player1Left = handsControllers[i];
                        }
                        else if (!handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_L"))
                        {
                            player2Left = handsControllers[i];

                        }
                        else if (handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_R"))
                        {
                            player1Right = handsControllers[i];
                        }
                        else if (!handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_R"))
                        {
                            player2Right = handsControllers[i];

                        }
                        else if (handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("Head"))
                        {
                            player1Head = handsControllers[i];
                        }
                        else if (!handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("Head"))
                        {
                            player2Head = handsControllers[i];
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient && gameObject.GetComponentInParent<PersonalManager>().IsCalibrating)
            {
                if(OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    //StartCalibration();
                }
            }
        }

        public void StartCalibration ()
        {
            // Get All Oculus Objects here
            GameObject[] handsControllers = GameObject.FindGameObjectsWithTag("Hands");
            for (var i = 0; i < handsControllers.Length; i++)
            {
                if (handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_L"))
                {
                    player1Left = handsControllers[i];
                }
                else if (!handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_L"))
                {
                    player2Left = handsControllers[i];

                }
                else if (handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_R"))
                {
                    player1Right = handsControllers[i];
                }
                else if (!handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRCustomHandPrefab_R"))
                {
                    player2Right = handsControllers[i];

                }
                else if (handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("Head"))
                {
                    player1Head = handsControllers[i];
                }
                else if (!handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("Head"))
                {
                    player2Head = handsControllers[i];
                } else if(handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRNetworkCameraRigHands"))
                {
                    player1Center = handsControllers[i];
                }
                else if (!handsControllers[i].GetComponentInParent<Photon.Pun.PhotonView>().IsMine && handsControllers[i].name.Contains("OVRNetworkCameraRigHands"))
                {
                    player2Center = handsControllers[i];
                }
            }
            // Get the ownership once
            player2Left.GetComponentInParent<Photon.Pun.PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            player2Right.GetComponentInParent<Photon.Pun.PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
            // Move controllers coordinate and rotation here
            // TODO: rotation
            player2Left.transform.position = player1Right.transform.position;
            player2Right.transform.position = player1Left.transform.position;
            //player2Center.transform.position = player1Center.transform.position;
            // Give the ownership back
            //player2Left.GetComponentInParent<Photon.Pun.PhotonView>().TransferOwnership(PhotonNetwork.PlayerListOthers[0].ActorNumber);
            //player2Right.GetComponentInParent<Photon.Pun.PhotonView>().TransferOwnership(PhotonNetwork.PlayerListOthers[0].ActorNumber);
        }
    }

}
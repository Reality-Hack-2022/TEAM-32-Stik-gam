using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//
//For handling local objects and sending data over the network
//
namespace Networking.Pun2
{
    public class PersonalManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [SerializeField] GameObject headPrefab;
        [SerializeField] GameObject bodyPrefab;
        [SerializeField] GameObject handRPrefab;
        [SerializeField] GameObject handLPrefab;
        [SerializeField] GameObject ovrCameraRig;
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] GameObject generatedSphere;
        [SerializeField] GameObject generatedParent;

        public bool IsCalibrating = false;

        //Tools
        List<GameObject> toolsR;
        List<GameObject> toolsL;
        List<GameObject> currentLine;
        int currentToolR;
        int currentToolL;

        //Colors
        public Color[] colors = { new Color(1.0f,0.35f, 0.79f), new Color(0.44f, 0.7f, 0.87f),new Color(0.2f, 0.98f, 0.76f) };
        public int currentColorIndex = 0;

        private Subscription<PlayerEvents.PlayerGripDown> playerGripDownSubscription;
        private Subscription<PlayerEvents.PlayerGripUp> playerGripUpSubscription;
        private Subscription<PlayerEvents.PlayerTriggerDown> playerTriggerDownSubscription;
        private Subscription<PlayerEvents.PlayerTriggerUp> playerTriggerUpSubscription;
        private Subscription<PlayerEvents.PlayerPrimaryDown> playerPrimaryDowSubscription;
        private Subscription<PlayerEvents.PlayerSecondaryDown> playerSecondaryDownSubscription;


        private void Awake()
        {
            /// If the game starts in Room scene, and is not connected, sends the player back to Lobby scene to connect first.
            if (!PhotonNetwork.NetworkingClient.IsConnected)
            {
                SceneManager.LoadScene("Lobby");
                return;
            }
            /////////////////////////////////

            toolsR = new List<GameObject>();
            toolsL = new List<GameObject>();

            if (PhotonNetwork.LocalPlayer.ActorNumber <= spawnPoints.Length)
            {
                ovrCameraRig.transform.position = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position;
                ovrCameraRig.transform.rotation = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.rotation;
            }
            generatedSphere.GetComponent<Renderer>().sharedMaterial.color = colors[currentColorIndex];
        }

        private void Start()
        {
            playerGripDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerGripDown>(Grip_performed);
            playerGripUpSubscription = EventBus.Subscribe<PlayerEvents.PlayerGripUp>(Grip_stopped);
            playerTriggerDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerTriggerDown>(Trigger_performed);
            playerTriggerUpSubscription = EventBus.Subscribe<PlayerEvents.PlayerTriggerUp>(Trigger_stopped);
            playerPrimaryDowSubscription = EventBus.Subscribe<PlayerEvents.PlayerPrimaryDown>(Primary_performed);
            playerSecondaryDownSubscription = EventBus.Subscribe<PlayerEvents.PlayerSecondaryDown>(Secondary_performed);
            //Instantiate Head
            GameObject obj = (PhotonNetwork.Instantiate(headPrefab.name, OculusPlayer.instance.head.transform.position, OculusPlayer.instance.head.transform.rotation, 0));
            //obj.GetComponent<SetColor>().SetColorRPC(PhotonNetwork.LocalPlayer.ActorNumber);

            //Instantiate Body
            obj = (PhotonNetwork.Instantiate(bodyPrefab.name, OculusPlayer.instance.body.transform.position, OculusPlayer.instance.body.transform.rotation, 0));

            //Instantiate right hand
            obj = (PhotonNetwork.Instantiate(handRPrefab.name, OculusPlayer.instance.rightHand.transform.position, OculusPlayer.instance.rightHand.transform.rotation, 0));
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                toolsR.Add(obj.transform.GetChild(i).gameObject);
                //obj.transform.GetComponentInChildren<SetColor>().SetColorRPC(PhotonNetwork.LocalPlayer.ActorNumber);
                //if (i > 0)
                    //toolsR[i].transform.parent.GetComponent<PhotonView>().RPC("DisableTool", RpcTarget.AllBuffered, 1);
            }

            //Instantiate left hand
            obj = (PhotonNetwork.Instantiate(handLPrefab.name, OculusPlayer.instance.leftHand.transform.position, OculusPlayer.instance.leftHand.transform.rotation, 0));
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                toolsL.Add(obj.transform.GetChild(i).gameObject);
                //obj.transform.GetComponentInChildren<SetColor>().SetColorRPC(PhotonNetwork.LocalPlayer.ActorNumber);
                //if (i > 0)
                    //toolsL[i].transform.parent.GetComponent<PhotonView>().RPC("DisableTool", RpcTarget.AllBuffered, 1);
            }

            currentLine = new List<GameObject>();
        }

        //Detects input from Thumbstick to switch "hand tools"
        private void Update()
        {
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                generatedSphere.GetComponent<Renderer>().sharedMaterial.color = colors[currentColorIndex];
                PhotonNetwork.Instantiate(generatedSphere.name, OculusPlayer.instance.rightHand.transform.position, OculusPlayer.instance.rightHand.transform.rotation, 0);
            }
            if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                GameObject father = PhotonNetwork.Instantiate(generatedParent.name, OculusPlayer.instance.rightHand.transform.position, OculusPlayer.instance.rightHand.transform.rotation, 0);
                
                for(var i = 0; i < currentLine.Count; i++)
                {
                    if(currentLine[i].GetComponent<Photon.Pun.PhotonView>().IsMine && currentLine[i].transform.parent == null)
                    {
                        Debug.Log("It's mine!");
                        currentLine[i].transform.parent = father.transform;
                    }
                }
                currentLine.Clear();
            }
            //not in scope/can't use :(
            //if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick))
            //    SwitchToolL();

            //if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick))
            //    SwitchToolR();

            //if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            //{
            //    GenerateCube(); // replace this with our mesh
            //}           

            // For Debug
            if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                IsCalibrating = true;
            }
        }

        void ChangeColor()
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tanks");

        }


        //disables current tool and enables next tool
        void SwitchToolR()
        {
            toolsR[currentToolR].transform.parent.GetComponent<PhotonView>().RPC("DisableTool", RpcTarget.AllBuffered, currentToolR);
            currentToolR++;
            if (currentToolR > toolsR.Count - 1)
                currentToolR = 0;
            toolsR[currentToolR].transform.parent.GetComponent<PhotonView>().RPC("EnableTool", RpcTarget.AllBuffered, currentToolR);
        }

        void SwitchToolL()
        {
            toolsL[currentToolL].transform.parent.GetComponent<PhotonView>().RPC("DisableTool", RpcTarget.AllBuffered, currentToolL);
            currentToolL++;
            if (currentToolL > toolsL.Count - 1)
                currentToolL = 0;
            toolsL[currentToolL].transform.parent.GetComponent<PhotonView>().RPC("EnableTool", RpcTarget.AllBuffered, currentToolL);
        }

        void GenerateCube()
        {
            PhotonNetwork.Instantiate(generatedSphere.name, OculusPlayer.instance.rightHand.transform.position, OculusPlayer.instance.rightHand.transform.rotation, 0);
        }

        //If disconnected from server, returns to Lobby to reconnect
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            SceneManager.LoadScene(0);
        }

        //So we stop loading scenes if we quit app
        private void OnApplicationQuit()
        {
            StopAllCoroutines();
        }

        private void Trigger_performed(PlayerEvents.PlayerTriggerDown e)
        {
            //start a corotine 

            if (e.isLeft)
            {

            }
            else
            {

            }


        }

        private void Trigger_stopped(PlayerEvents.PlayerTriggerUp e)
        {
            if (e.isLeft)
            {


            }
            else
            {

            }

        }


        private void Primary_performed(PlayerEvents.PlayerPrimaryDown e)
        {
            if (e.isLeft)
            {
                currentColorIndex--;
                if (currentColorIndex < 0) currentColorIndex = colors.Length - 1;
                ChangeColor();
            }
            else
            {
                currentColorIndex--;
                if (currentColorIndex < 0) currentColorIndex = colors.Length - 1;
                ChangeColor();
            }
        }

        private void Secondary_performed(PlayerEvents.PlayerSecondaryDown e)
        {
            if (e.isLeft)
            {
                currentColorIndex++;
                if (currentColorIndex > colors.Length - 1) currentColorIndex = 0;
                ChangeColor();
            }
            else
            {
                currentColorIndex++;
                if (currentColorIndex > colors.Length - 1) currentColorIndex = 0;
                ChangeColor();
            }
        }

        private void Grip_performed(PlayerEvents.PlayerGripDown e)
        {
            if (e.isLeft)
            {
            }
            else
            {
            }
        }

        private void Grip_stopped(PlayerEvents.PlayerGripUp e)
        {
            if (e.isLeft)
            {

            }
            else
            {

            }
        }

        // ネットワークオブジェクトが生成された時に呼ばれるコールバック
        void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (info.Sender.IsLocal)
            {
                Debug.Log("自身がネットワークオブジェクトを生成しました");
                Debug.Log(info.photonView);
            }
            else
            {
                Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
            }
        }
    }
}

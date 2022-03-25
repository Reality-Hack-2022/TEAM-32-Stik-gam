using Photon.Pun;
using UnityEngine;

//
//Creation example tool to instantiate a cube in the network using PhotonNetwork.Instantiate
//The cube ownership is set to actor number when created, and to its color using SetColor RPC
//
namespace Networking.Pun2
{
    public class CubeTool : MonoBehaviourPun
    {
        [SerializeField] enum Hand { Right, Left };
        [SerializeField] Hand hand;
        bool building;
        float t;

        void Update()
        {
            if (photonView.IsMine)
            {
                if(hand == Hand.Left)
                {
                    t = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
                }
                else if(hand == Hand.Right)
                {
                    t = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
                }

                if(t > 0.5f && !building)
                {
                    building = true;
                }
                if(building && t < 0.5f)
                {
                    building = false;
                    ReleaseCube();
                }
            }
        }

        void ReleaseCube()
        {
            GameObject obj = PhotonNetwork.Instantiate("GenericCube", transform.position, transform.rotation, 0);
            obj.GetComponent<SetColor>().SetColorRPC(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}

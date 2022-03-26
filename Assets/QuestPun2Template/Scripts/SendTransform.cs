using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking.Pun2
{
    public class SendTransform : MonoBehaviourPun
    {
        [SerializeField] int index = 1;

        // If this component is "mine", update its transform and rotation (used by head and hands to correctly follow anchors)
        void LateUpdate()
        {
            if (photonView.IsMine)
            {
                switch (index)
                {
                    case 1: //head
                        transform.position = OculusPlayer.instance.head.transform.position;
                        transform.rotation = OculusPlayer.instance.head.transform.rotation;
                        break;

                    case 2: //left
                        //Quaternion tmpRotation = new Quaternion(OculusPlayer.instance.leftHand.transform.rotation.x - 90.0f, OculusPlayer.instance.leftHand.transform.rotation.y - 90.0f, OculusPlayer.instance.leftHand.transform.rotation.z, 1);
                        transform.position = OculusPlayer.instance.leftHand.transform.position;
                        transform.rotation = OculusPlayer.instance.leftHand.transform.rotation;
                        break;

                    case 3: //right
                        transform.position = OculusPlayer.instance.rightHand.transform.position;
                        transform.rotation = OculusPlayer.instance.rightHand.transform.rotation;
                        break;

                    case 4: // body
                        transform.position = OculusPlayer.instance.body.transform.position;
                        transform.rotation = OculusPlayer.instance.body.transform.rotation;
                        break;
                }
            }
        }
    }
}

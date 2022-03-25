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
                        transform.position = OculusPlayer.instance.leftHand.transform.position;
                        transform.rotation = OculusPlayer.instance.leftHand.transform.rotation;
                        break;

                    case 3: //right
                        transform.position = OculusPlayer.instance.rightHand.transform.position;
                        transform.rotation = OculusPlayer.instance.rightHand.transform.rotation;
                        break;
                }
            }
        }
    }
}

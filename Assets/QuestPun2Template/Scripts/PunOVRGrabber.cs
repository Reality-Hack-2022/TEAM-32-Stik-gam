using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking.Pun2
{
    [RequireComponent (typeof (PhotonView))]
    public class PunOVRGrabber : OVRGrabber
    {
        PhotonView pv;

        //HandTracking related
        PunOVRHand trackingHand;
        private float pinchThreshold = 0.7f;

        protected override void Awake()
        {
            base.Awake();
            pv = GetComponent<PhotonView>();
            if(GetComponent<PunOVRHand>() != null)
            {
                trackingHand = GetComponent<PunOVRHand>();
            }
        }

        //Basically, if photonview is mine, update anchors from Grabber
        public override void Update()
        {
            if (pv.IsMine)
            {
                base.Update();
                //If hand tracking component detected, check pinch for grabbing mechanism.
                if(trackingHand != null)
                {
                    CheckPinch();
                }
            }
        }

        //If the pinch strenght is bigger than the threshold, call GrabBegin(), if smaller, call GrabEnd().
        private void CheckPinch()
        {
            float pinchStrenght = trackingHand.GetFingerPinchStrength(PunOVRHand.HandFinger.Index);

            if(pinchStrenght > pinchThreshold)
            {
                GrabBegin();
                Debug.Log("Grab begin");
            }
            else if (pinchStrenght < pinchThreshold)
            {
                GrabEnd();
            }
        }
    }
}

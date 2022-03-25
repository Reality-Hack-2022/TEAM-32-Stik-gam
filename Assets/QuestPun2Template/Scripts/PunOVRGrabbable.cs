using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

//
//Custom grabbable script which checks if the grabber "is mine" to actually grab
//

namespace Networking.Pun2
{
    [RequireComponent(typeof(PhotonView))]
    public class PunOVRGrabbable : OVRGrabbable
    {
        public UnityEvent onGrab;
        public UnityEvent onRelease;
        [SerializeField] bool hideHandOnGrab;
        PhotonView pv;
        Rigidbody rb;

        protected override void Start()
        {
            base.Start();
            pv = GetComponent<PhotonView>();
            rb = gameObject.GetComponent<Rigidbody>();
        }

        override public void GrabBegin(OVRGrabber hand, Collider grabPoint)
        {
            pv.RPC("ForceRelease", RpcTarget.Others);
            m_grabbedBy = hand;
            m_grabbedCollider = grabPoint;

            pv.TransferOwnership(PhotonNetwork.LocalPlayer);

            if (pv.IsMine)
            {
                pv.RPC("SetKinematicTrue", RpcTarget.All); //changes the kinematic state of the object to all players when its grabbed
                if (onGrab != null)
                    onGrab.Invoke();
                if (hideHandOnGrab)
                    m_grabbedBy.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
        }

        override public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
        {
            //If the grabbed object is mine, release it
            if (pv.IsMine)
            {
                rb.isKinematic = m_grabbedKinematic;
                pv.RPC("SetKinematicFalse", RpcTarget.All);
                rb.velocity = linearVelocity;
                rb.angularVelocity = angularVelocity;
                if (hideHandOnGrab)
                    m_grabbedBy.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                m_grabbedBy = null;
                m_grabbedCollider = null;
                if (onRelease != null)
                    onRelease.Invoke();
            }
        }

        public Collider[] grabPoints
        {
            get { return m_grabPoints; }
            set { grabPoints = value; }
        }

        virtual public void CustomGrabCollider(Collider[] collider)
        {
            m_grabPoints = collider;
        }

        [Photon.Pun.PunRPC]
        public void SetKinematicTrue()
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        [PunRPC]
        public void SetKinematicFalse()
        {
            rb.isKinematic = m_grabbedKinematic;
        }

        [PunRPC]
        public void ForceRelease() 
        {
            if(m_grabbedBy != null) 
            {
                m_grabbedBy.ForceRelease(this);
            }
        }
    }
}


using UnityEngine;
using UnityEngine.Events;

namespace Networking.Pun2
{
    public class OnTouchPun : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTouch;

        //On trigger enter used for buttons, it will only trigger if touching photonview "is mine"
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponentInParent<Photon.Pun.PhotonView>().IsMine)
                InvokeOnTouch();
        }

        public void InvokeOnTouch()
        {
            onTouch.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkingShin.Pun2
{
    public class OculusPlayer : MonoBehaviour
    {
        public GameObject head;
        public GameObject rightHand;
        public GameObject leftHand;

        public static OculusPlayer instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking.Pun2
{
    //This script can be used to smoothly return to game menu
    public class BackToMenuPun : MonoBehaviourPun
    {
        bool returningToMenu;
        OVRScreenFade screenFader;
        [SerializeField] string sceneToGo;

        private void Start()
        {
            screenFader = GameObject.FindObjectOfType<OVRScreenFade>();
        }
        private void Update()
        {
            if (photonView.IsMine)
            {
                if (OVRInput.GetDown(OVRInput.Button.Start))
                {
                    GoBackToMenu();
                }
            }
        }

        public void GoBackToMenu()
        {
            if (returningToMenu || !photonView.IsMine) return;
            returningToMenu = true;
            StartCoroutine("Load");
        }

        IEnumerator Load()
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneToGo);
            PhotonNetwork.Disconnect();
            async.allowSceneActivation = false;
            screenFader.FadeOut();
            yield return new WaitForSeconds(2);
            returningToMenu = false;
            async.allowSceneActivation = true;
        }
    }
}

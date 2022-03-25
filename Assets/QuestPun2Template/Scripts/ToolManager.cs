using Photon.Pun;
using UnityEngine;
//
//For managing different tools over the network
//
namespace Networking.Pun2
{
    public class ToolManager : MonoBehaviour
    {
        [PunRPC]
        public void DisableTool(int n)
        {
            transform.GetChild(n).gameObject.SetActive(false);
        }

        [PunRPC]
        public void EnableTool(int n)
        {
            transform.GetChild(n).gameObject.SetActive(true);
        }
    }
}

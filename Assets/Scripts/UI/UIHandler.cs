using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        public GameObject debugPanel;
        public Button hostButton;
        public Button clientButton;
        public GameObject timeCanvas;
        public void HostStart()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void ClientStart()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
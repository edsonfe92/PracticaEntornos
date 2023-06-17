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

        public void HostStart()
        {
            NetworkManager.Singleton.StartHost();
            GameManager.instance.gameConfig = transform.GetChild(1).gameObject.GetComponent<CreateMatchHandler>().gameConfig;
        }

        public void ClientStart()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
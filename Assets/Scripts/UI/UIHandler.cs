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

        private void Start()
        {
            hostButton.onClick.AddListener(OnHostButtonClicked);
            clientButton.onClick.AddListener(OnClientButtonClicked);
        }

        private void OnHostButtonClicked()
        {
            NetworkManager.Singleton.StartHost();
            debugPanel.SetActive(false);
            timeCanvas.SetActive(true);
        }

        private void OnClientButtonClicked()
        {
            NetworkManager.Singleton.StartClient();
            debugPanel.SetActive(false);
        }
    }
}
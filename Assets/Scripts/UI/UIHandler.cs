using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {        
        public GameObject debugPanel;
        public Button hostButton;
        public Button clientButton;


        public string clientPlayerName;
        public List<GameObject> playerCharacterPrefabs = new List<GameObject>();
        public GameObject playerCharacter;

        public static UIHandler instance;
        
        


        private void Awake()
        {
            playerCharacter = playerCharacterPrefabs[0];
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void HostStart()
        {
            NetworkManager.Singleton.StartHost();
            GameManager.instance.gameConfig = transform.GetChild(1).gameObject.GetComponent<CreateMatchHandler>().gameConfig;
        }

        public void ClientStart()
        {
            NetworkManager.Singleton.StartClient();
        }

        public void PlayerNameInputfield(string playerName) 
        {
            clientPlayerName = playerName;
        }

        public void DropdownPlayerSelected(int index)
        {
            switch (index)
            {
                case 0: playerCharacter = playerCharacterPrefabs[0]; break;
                case 1: playerCharacter = playerCharacterPrefabs[1]; break;
                case 2: playerCharacter = playerCharacterPrefabs[2]; break;
                default: playerCharacter = playerCharacterPrefabs[2]; break;
            }
        }
    }
}
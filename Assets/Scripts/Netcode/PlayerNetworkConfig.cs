using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Movement.Components;
using System.Collections;
using System.Linq;
using UI;
using Unity.Collections;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour { 


        private int nextIndex = 0;

        public GameObject characterPrefab;
        

        public GameObject nombrePrefab;
        public GameObject vidaPrefab;

                        
        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            characterPrefab = UIHandler.instance.playerCharacter;
            InstantiateCharacterServerRpc(OwnerClientId, UIHandler.instance.clientPlayerName);
        }


        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id, string name)

        {            
            GameObject vidaGameObject = Instantiate(vidaPrefab);
            vidaGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);

            var spawnPoint = SpawnSystemLobby.instance.spawnPointsLobby;
            GameObject characterGameObject = Instantiate(characterPrefab, spawnPoint[nextIndex].position, spawnPoint[nextIndex].rotation);
            characterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
            characterGameObject.transform.SetParent(transform, false);
            characterGameObject.GetComponent<FighterMovement>().playerNameScript.playerName.Value = name;

            characterGameObject.GetComponent<FighterMovement>().vidaUI = vidaGameObject.GetComponent<Vida>();

            nextIndex++;            
            
        }

    }
}
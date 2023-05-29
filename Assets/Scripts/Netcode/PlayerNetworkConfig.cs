using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Movement.Components;
using System.Collections.Generic;
using System.Linq;

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
            InstantiateCharacterServerRpc(OwnerClientId);
        }

        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id)

        {
            GameObject vidaGameObject = Instantiate(vidaPrefab);
            vidaGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);

            // Obtén una referencia al transform del Canvas "VidaUICanvas"
            //Transform vidaUICanvasTransform = GameObject.Find("VidaUICanvas").transform;

            //vidaGameObject.transform.SetParent(vidaUICanvasTransform, false);

            var spawnPoint = SpawnSystemLobby.instance.spawnPointsLobby;
            GameObject characterGameObject = Instantiate(characterPrefab, spawnPoint[nextIndex].position, spawnPoint[nextIndex].rotation);
            characterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
            characterGameObject.transform.SetParent(transform, false);

            nextIndex++;

            GameObject nombreGameObject = Instantiate(nombrePrefab);
            nombreGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            nombreGameObject.transform.SetParent(transform, false);
            nombreGameObject.GetComponent<PlayerName>().playerTransform = characterGameObject.transform;
            
            characterGameObject.GetComponent<FighterMovement>().vidaUI = vidaGameObject.GetComponent<Vida>();
        }

    }
}
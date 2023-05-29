using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {

        private int nextIndex = 0;

        public GameObject characterPrefab;
        public GameObject nombrePrefab;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            InstantiateCharacterServerRpc(OwnerClientId);
        }

        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id)
        {
            var spawnPoint = SpawnSystemLobby.instance.spawnPointsLobby;
            GameObject characterGameObject = Instantiate(characterPrefab, spawnPoint[nextIndex].position, spawnPoint[nextIndex].rotation);
            //characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
            characterGameObject.transform.SetParent(transform, false);


            nextIndex++;

            GameObject nombreGameObject = Instantiate(nombrePrefab);
            nombreGameObject.transform.SetParent(transform, false);
            //Vector3 pos = characterGameObject.transform.position;
            //nombreGameObject.transform.position = pos;
            nombreGameObject.GetComponent<PlayerName>().playerTransform = characterGameObject.transform;
        }
    }
}

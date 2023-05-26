using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {
        public GameObject characterPrefab;

        private int nextIndex = 0;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            InstantiateCharacterServerRpc(OwnerClientId);
        }
    
        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id)
        {
            List<Transform> spawnPoint = SpawnSystem.instance.spawnPoints;

            GameObject characterGameObject = Instantiate(characterPrefab, spawnPoint[nextIndex].position, spawnPoint[nextIndex].rotation);
            characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            //characterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
            characterGameObject.transform.SetParent(transform, false);

            nextIndex++;
        }
    }
}

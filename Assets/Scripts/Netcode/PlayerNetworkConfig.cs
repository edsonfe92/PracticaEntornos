using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {
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
            GameObject characterGameObject = Instantiate(characterPrefab);
            characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.transform.SetParent(transform, false);

            GameObject nombreGameObject = Instantiate(nombrePrefab);
            nombreGameObject.transform.SetParent(transform, false);
            //Vector3 pos = characterGameObject.transform.position;
            //nombreGameObject.transform.position = pos;
            nombreGameObject.GetComponent<PlayerName>().playerTransform = characterGameObject.transform;

        }
    }
}

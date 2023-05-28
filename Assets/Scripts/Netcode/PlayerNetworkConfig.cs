using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Movement.Components;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {

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
            GameObject characterGameObject = Instantiate(characterPrefab);
            characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.transform.SetParent(transform, false);

            GameObject nombreGameObject = Instantiate(nombrePrefab);
            nombreGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            nombreGameObject.transform.SetParent(transform, false);
            nombreGameObject.GetComponent<PlayerName>().playerTransform = characterGameObject.transform;

            GameObject vidaGameObject = Instantiate(vidaPrefab);
            vidaGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.GetComponent<FighterMovement>().vidaUI = vidaGameObject.GetComponent<Vida>();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode;
public class CharacterSelector : NetworkBehaviour
{
    public PlayerNetworkConfig playerNetworkConfig;
    public GameObject playerPrefab;

    private void Start()
    {
        Invoke("test",20);
    }
    public void test() 
    {
        playerNetworkConfig.characterPrefab = playerPrefab;
        playerNetworkConfig.InstantiateCharacterServerRpc(OwnerClientId);
    }
}

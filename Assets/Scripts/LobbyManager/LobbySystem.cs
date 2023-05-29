using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode;

public class LobbySystem : NetworkBehaviour
{
    public Timer timer;
    //int cont = 0;
    private int nextIndexUnstantiate = 0;
    private int nextIndextp = 0;
    Transform[] spawnPoints;
    bool tp = false;
    public GameObject characterPrefab;
    public GameObject nombrePrefab;
    public CameraBoundaries scriptCamera;

    void Start()
    {

        spawnPoints = SpawnSystemGame.instance.spawnPointsGame;
        Debug.Log(spawnPoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        print(SpawnSystemGame.instance.spawnPointsGame);
        /*if (timer.IsTimerFinished()&& tp == false)
        {
            print("A");
            //Teletransportar personajes
            //teletransporteServerRpc();
            tp = true;

        }*/
    }

    [ClientRpc]
    public void changeCameraClientRpc()
    {
        scriptCamera.changeToBattle();
    }

    [ServerRpc]
    public void teletransporteServerRpc()
    {
        //NetworkManager.ConnectedClientsIds
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            print($"Este es el id del cliente {clientId}");
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {
                if (client.PlayerObject != null && nextIndextp <= 3)
                {
                    print("1" + client.PlayerObject.transform.position);
                    foreach(Transform child in client.PlayerObject.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    // Teletransportar el jugador cambiando su transform
                    Debug.Log(spawnPoints[0].position);
                    GameObject characterGameObject = Instantiate(characterPrefab, spawnPoints[nextIndexUnstantiate].position, spawnPoints[nextIndexUnstantiate].rotation);
                    characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                    characterGameObject.transform.SetParent(client.PlayerObject.transform, false);

                    nextIndexUnstantiate++;

                    GameObject nombreGameObject = Instantiate(nombrePrefab);
                    nombreGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                    nombreGameObject.transform.SetParent(client.PlayerObject.transform, false);
                    nombreGameObject.GetComponent<PlayerName>().playerTransform = characterGameObject.transform;
                    print("2" + client.PlayerObject.transform.position);

                    nextIndextp++;
                }
            }
        }
        changeCameraClientRpc();
    }
}

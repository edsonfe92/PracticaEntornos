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

    void Start()
    {

        spawnPoints = SpawnSystemGame.instance.spawnPointsGame;
        Debug.Log(spawnPoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        print(SpawnSystemGame.instance.spawnPointsGame);
        if (timer.IsTimerFinished()&& tp == false)
        {
            print("A");
            //Teletransportar personajes
            teletransporteServerRpc();
            tp = true;
        }
    }

    [ServerRpc]
    public void InstantiateCharacterServerRpc(ulong id)
    {
        /*Debug.Log(spawnPoints[0].position);
        GameObject characterGameObject = Instantiate(characterPrefab, spawnPoints[nextIndexUnstantiate].position, spawnPoints[nextIndexUnstantiate].rotation);
        //characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
        characterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
        characterGameObject.transform.SetParent(transform, false);


        nextIndexUnstantiate++;

        GameObject nombreGameObject = Instantiate(nombrePrefab);
        nombreGameObject.transform.SetParent(transform, false);
        //Vector3 pos = characterGameObject.transform.position;
        //nombreGameObject.transform.position = pos;
        nombreGameObject.GetComponent<PlayerName>().playerTransform = characterGameObject.transform;*/
    }

    [ServerRpc]
    public void teletransporteServerRpc()
    {
        //NetworkManager.ConnectedClientsIds
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            /*if(cont == 4)
            {
                break;
            }*/

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
                    //Transform playerTransform = client.PlayerObject.transform;
                    //playerTransform.Translate(spawnPoint[nextIndex].localPosition, Space.Self);
                    //client.PlayerObject.transform.localPosition = spawnPoint[nextIndex].localPosition;
                    Debug.Log(spawnPoints[0].position);
                    GameObject characterGameObject = Instantiate(characterPrefab, spawnPoints[nextIndexUnstantiate].position, spawnPoints[nextIndexUnstantiate].rotation);
                    //characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
                    characterGameObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
                    characterGameObject.transform.SetParent(client.PlayerObject.transform, false);


                    nextIndexUnstantiate++;

                    GameObject nombreGameObject = Instantiate(nombrePrefab);
                    nombreGameObject.transform.SetParent(client.PlayerObject.transform, false);
                    //Vector3 pos = characterGameObject.transform.position;
                    //nombreGameObject.transform.position = pos;
                    nombreGameObject.GetComponent<PlayerName>().playerTransform = characterGameObject.transform;
                    print("2" + client.PlayerObject.transform.position);

                    nextIndextp++;
                    //cont++;
                }
            }
        }
    }
}

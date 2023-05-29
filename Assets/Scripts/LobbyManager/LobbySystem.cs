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


    private int i = 0;

    void Start()
    {

        spawnPoints = SpawnSystemGame.instance.spawnPointsGame;
        Debug.Log(spawnPoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {        
        if (timer.IsTimerFinished()&& tp == false)
        {
            Debug.Log("TimerTerminado");
            //Teletransportar personajes
            teletransporteServerRpc();
            tp = true;

        }
    }

    [ClientRpc]
    public void changeCameraClientRpc()
    {
        scriptCamera.changeToBattle();
    }

    [ClientRpc]
    public void DebugClientRpc(string s)
    {
        Debug.Log(s);
    }

    [ServerRpc]
    public void teletransporteServerRpc()
    {
        DebugClientRpc("Dentro teletransporteRpc");
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {            
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {
                client.PlayerObject.transform.position = SpawnSystemGame.instance.spawnPointsGame[i].position;                
                i++;                
            }
        }
        changeCameraClientRpc();
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode;

public class LobbySystem : NetworkBehaviour
{
    public Timer timer;
    public int maxTimeLobby;
    Transform[] spawnPoints;
    bool tp = false;
    public GameObject characterPrefab;
    public GameObject nombrePrefab;
    public CameraBoundaries scriptCamera;    


    private int i = 0;

    void Start()
    {
        spawnPoints = SpawnSystemGame.instance.spawnPointsGame;        
    }
    private void OnEnable()
    {
        timer.SetTimerServerRpc(maxTimeLobby);
        timer.StartTimerServerRpc();
    }
    void Update()

    {
        if (tp) return;

        ConnectedClientsServerRpc();
        
        if (timer.IsTimerFinished())
        {            
            teletransporteServerRpc();            
            tp = true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ConnectedClientsServerRpc() 
    {
        if (NetworkManager.Singleton.ConnectedClients.Count < 2)
        {
            timer.SetTimerServerRpc(maxTimeLobby);
            timer.StopTimerServerRpc();            
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

    [ServerRpc(RequireOwnership = false)]
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



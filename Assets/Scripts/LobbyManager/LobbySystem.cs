using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode;
using TMPro;
using Movement.Components;

public class LobbySystem : NetworkBehaviour
{
    public Timer timer;
    public int maxTimeLobby;
    Transform[] spawnPoints;
    public bool tp = false;
    public GameObject characterPrefab;
    public GameObject nombrePrefab;
    public CameraBoundaries scriptCamera;

    public TMP_Text timerText;
        


    private int i = 0;

    void Start()
    {
        spawnPoints = SpawnSystemGame.instance.spawnPointsGame;        
    }
    public override void OnNetworkSpawn() 
    {
        timer.tMax.OnValueChanged += OnTimerLobbyChanged;
    }
    public override void OnNetworkDespawn()
    {
        timer.tMax.OnValueChanged -= OnTimerLobbyChanged;
    }

    public void OnTimerLobbyChanged(int oldValue, int newValue) 
    {
        if (newValue<=0)
        {
            timerText.gameObject.SetActive(false);
        }
        timerText.text = newValue.ToString();
        
    }
    public void StartTimer() 
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
            TeletransporteServerRpc();            
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
    public void TeletransporteServerRpc()
    {
        Debug.Log("Dentro Teletransporte a Battle");
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {            
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {                                
                if (client.ClientId == 0)
                {
                    client.PlayerObject.transform.GetChild(0).position = SpawnSystemGame.instance.spawnPointsGame[i].position;
                    client.PlayerObject.transform.GetChild(0).gameObject.GetComponent<FighterMovement>().EneableHealthUIClientRpc(true);
                    client.PlayerObject.transform.GetChild(0).gameObject.GetComponent<FighterMovement>().inLobby = false;
                }
                else
                {
                    client.PlayerObject.transform.position = SpawnSystemGame.instance.spawnPointsGame[i].position;
                    client.PlayerObject.GetComponent<FighterMovement>().EneableHealthUIClientRpc(true);
                    client.PlayerObject.GetComponent<FighterMovement>().inLobby = false;
                }
                    
                i++;                
            }
        }

        changeCameraClientRpc();
        GameManager.instance.SetUpMatchTimer();
        GameManager.instance.startMatch = true;
        GameManager.instance.GenerateClienObjectNetworkList();
        GameManager.instance.SetAllHealthUIServerRpc();
    }
}



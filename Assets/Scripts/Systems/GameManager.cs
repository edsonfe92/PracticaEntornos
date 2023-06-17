using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Netcode;
using Movement.Components;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    
    //Timers & Coundtdowns
    private Timer matchTimer;
    private int roundCountdown;
    
    //Match Configurations
    public GameConfig gameConfig;

    private int currentRound;
    public int numPlayers = 0;
    public int currentPlayingPlayers;

    public bool roundEnded;
    public bool startMatch;

    public NetworkClient winner;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GetNumberPlayersConnectedServerRpc();
        roundCountdown = 3;
        currentRound = 0;
        currentPlayingPlayers = numPlayers;
        matchTimer = GetComponent<Timer>();
        matchTimer.SetTimerServerRpc(gameConfig.roundTime);
    }
    private void Update()
    {
        if (!startMatch) return;

        if (matchTimer.IsTimerFinished())
        {
            GetWinnerByTimeServerRpc();
        }

        if (!matchTimer.IsTimerFinished() || !matchTimer.IsTimerActive())
        {
            matchTimer.StartTimerServerRpc();
        }
        
        if (currentRound == gameConfig.numRounds)
        {            
            DisplayMatchWinner();
            CountDownNewLobby();
            MovePlayersToLobby();
            ResetMatch();
        }
        if (roundEnded)
        {
            ReSpawnPlayers();
            CountDownRoundStart();            
            currentRound++;
        }
    }
    [ServerRpc]
    private void GetNumberPlayersConnectedServerRpc() 
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {
                numPlayers++;
            }
        }
    }
    [ServerRpc]
    private void GetWinnerByTimeServerRpc()
    {        
        float maxHealth = float.MinValue;
        NetworkClient winnerPlayer = null;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {                
                if (client.PlayerObject.transform.GetChild(0).gameObject.GetComponent<FighterMovement>().currentLife.Value > maxHealth) 
                {
                    maxHealth = client.PlayerObject.transform.GetChild(0).gameObject.GetComponent<FighterMovement>().currentLife.Value;
                    winnerPlayer = client; 
                }            
            }
        }
        winner = winnerPlayer;
    }

    private void ResetMatch()
    {
        throw new NotImplementedException();
    }

    private void MovePlayersToLobby()
    {
        throw new NotImplementedException();
    }

    private void CountDownNewLobby()
    {
        throw new NotImplementedException();
    }

    private void DisplayMatchWinner()
    {
        throw new NotImplementedException();
    }

    private void CountDownRoundStart()
    {
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown() 
    {
        while ( roundCountdown > 0)
        {
            yield return new WaitForSeconds(1);
            roundCountdown--;
            if (roundCountdown <= 0)
            {
                ReConfigPlayersMovement();
            }
        }
    }

    private void ReConfigPlayersMovement()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {
                DisablePlayerMovement(client, false);
            }
        }
    }

    private void ReSpawnPlayers()
    {
        int i = 0;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {
                client.PlayerObject.transform.position = SpawnSystemGame.instance.spawnPointsGame[i].position;
                foreach (Transform child in client.PlayerObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
                DisablePlayerMovement(client,true);
                i++;
            }
        }
    }
    private void DisablePlayerMovement(NetworkClient client, bool b) 
    { 
        client.PlayerObject.transform.GetChild(0).gameObject.GetComponent<FighterMovement>().countDownActive = b;
    }
    public void PlayerDead() 
    {
        currentPlayingPlayers--;
        if (currentPlayingPlayers <= 1)
        {
            CheckLastOneStanding();
            roundEnded = true;
        }
    }
    public void PlayerDisconnected() 
    {
        currentPlayingPlayers--;
        numPlayers--;
        if (currentPlayingPlayers <= 1 || numPlayers <= 1)
        {
            CheckLastOneStanding();
            roundEnded = true;
        }
    }

    public void CheckLastOneStanding() 
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {
                Debug.Log("Cliente: " + client);
                Debug.Log("ClientObject: " + client.PlayerObject.name);
                Debug.Log("Nombre Hijo 0: " + client.PlayerObject.transform.GetChild(0).name);
                Debug.Log("Componente: " + client.PlayerObject.transform.GetChild(0).gameObject.GetComponent<FighterMovement>());

                if (client.PlayerObject.transform.GetChild(0).gameObject.GetComponent<FighterMovement>().currentLife.Value > 0)
                {
                    winner = client;
                }
            }
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Netcode;
using TMPro;
using Movement.Components;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public List<GameObject> networkClientGameObjectList = new List<GameObject>();
    
    //Timers & Coundtdowns
    public Timer matchTimer;
    public Timer toLobbyTimer;

    public TMP_Text timerMatchText;
    public TMP_Text timerToLobbyText;

    //Match Configurations
    public GameConfig gameConfig;

    private int currentRound;
    public int numPlayers = 0;
    public int currentPlayingPlayers;

    public bool roundEnded;
    public bool startMatch;

    public GameObject winner;

    //LobbyManager
    public LobbySystem lobbySystem;

    [Header("Camera")]
    public CameraBoundaries scriptCamera;

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
        currentRound = 0;
        currentPlayingPlayers = numPlayers;        

    }
    public override void OnNetworkSpawn()
    {
        matchTimer.tMax.OnValueChanged += OnTimerMatchChanged;
        toLobbyTimer.tMax.OnValueChanged += OnTimerToLobbyChanged;
    }
    public override void OnNetworkDespawn()
    {
        matchTimer.tMax.OnValueChanged -= OnTimerMatchChanged;
        toLobbyTimer.tMax.OnValueChanged -= OnTimerToLobbyChanged;
    }

    private void OnTimerToLobbyChanged(int previousValue, int newValue)
    {
        timerToLobbyText.text = newValue.ToString();
    }

    private void OnTimerMatchChanged(int previousValue, int newValue)
    {
        timerMatchText.text = newValue.ToString();
    }
    //Bucle del juego
    private void Update()
    {
        if (!startMatch) return;

        if (toLobbyTimer.IsTimerFinished())
        {
            ResetMatchServerRpc();
            MovePlayersToLobbyServerRpc();
            startMatch = false;
        }

        if (matchTimer.IsTimerFinished())
        {         
            GetWinnerByTimeServerRpc();
            roundEnded = true;
        }

        if (toLobbyTimer.IsTimerActive()) return;

        if (!matchTimer.IsTimerActive())
        {            
            matchTimer.StartTimerServerRpc();
        }        
        
        if (currentRound == gameConfig.numRounds)
        {            
            //FINAL TODAS LAS RONDAS
            roundEnded = false;
            GetMatchWinnerServerRpc();
            DisplayMatchWinnerServerRpc();            
            CountDownNewLobby();            
            
        }
        if (roundEnded)
        {
            //RESET DE LA RONDA
            Debug.Log("RONDA NUMERO " + currentRound + " TERMINADA");
            roundEnded = false;
            ReSpawnPlayersServerRpc();
            CountDownRoundStart(3);
            currentPlayingPlayers = numPlayers;
            currentRound++;
        }
    }

    public void SetUpMatchTimer() 
    {
        matchTimer.SetTimerServerRpc(gameConfig.roundTime);
    }
    
    [ServerRpc]
    private void GetWinnerByTimeServerRpc()
    {
        GameObject roundWinnerPlayer = null;
        float maxHealth = float.MinValue;        
        foreach (var player in networkClientGameObjectList)
        {
            if (player.GetComponent<FighterMovement>().vidaUI.currentHP.Value > maxHealth)
            {
                maxHealth = player.GetComponent<FighterMovement>().vidaUI.currentHP.Value;
                roundWinnerPlayer = player;
            }
        }
        roundWinnerPlayer.GetComponent<FighterMovement>().vidaUI.currentPoints.Value++;                
    }
    [ServerRpc]
    private void GetMatchWinnerServerRpc() 
    {
        int maxPoints = 0;
        foreach (var player in networkClientGameObjectList)
        {
            if (player.GetComponent<FighterMovement>().vidaUI.currentPoints.Value > maxPoints)
            {
                maxPoints = player.GetComponent<FighterMovement>().vidaUI.currentPoints.Value;
                winner = player;
            }
        }
    }
    [ServerRpc]
    private void ResetMatchServerRpc()
    {
        currentRound = 0;
        matchTimer.ResetTimerServerRpc();
        toLobbyTimer.ResetTimerServerRpc();
        currentPlayingPlayers = 0;
    }

    [ServerRpc]
    private void MovePlayersToLobbyServerRpc()
    {
        Debug.Log("ENDING MATCH");
        Debug.Log("TP LOBBY");
        int i = 0;
        
        foreach (GameObject player in networkClientGameObjectList)
        {
            Debug.Log("PLAYER POS INI: " + player.transform.position);
            Debug.Log("SpawnPoint: " + SpawnSystemGame.instance.spawnPointsGame[i+4].position);
            player.transform.position = SpawnSystemGame.instance.spawnPointsGame[i+4].position;
            Debug.Log("PLAYER POS fin: " + player.transform.position);
             
            player.GetComponent<FighterMovement>().EneableHealthUIClientRpc(false);
            player.GetComponent<FighterMovement>().DisableWinnerUIClientRpc();
            player.GetComponent<FighterMovement>().inLobby = true;
            i++;
        }
        
        lobbySystem.timer.SetTimerServerRpc(lobbySystem.maxTimeLobby+5);
        lobbySystem.timer.StartTimerServerRpc();
        lobbySystem.tp = false;

        changeCameraClientRpc();

        foreach (GameObject p in networkClientGameObjectList)
        {
            Debug.Log("PLAYER POS INI: " + p.transform.position);
        }


    }

    private void CountDownNewLobby()
    {
        toLobbyTimer.SetTimerServerRpc(15);
        toLobbyTimer.StartTimerServerRpc();
    }   
    [ServerRpc]
    private void DisplayMatchWinnerServerRpc()
    {        
        foreach (GameObject player in networkClientGameObjectList)
        {
            player.GetComponent<FighterMovement>().EneableWinnerUIClientRpc(winner.GetComponent<FighterMovement>().playerNameScript.playerName.Value.ToString());
        }
    }

    private void CountDownRoundStart(int roundCountdownTime)
    {
        StartCoroutine(Countdown(roundCountdownTime));
    }
    IEnumerator Countdown(int roundCountdown) 
    {
        while ( roundCountdown > 0)
        {
            yield return new WaitForSeconds(1);
            roundCountdown--;
            if (roundCountdown <= 0)
            {
                ReConfigPlayersMovementServerRpc();
                SetUpMatchTimer();
            }
        }
    }
    [ServerRpc]
    private void ReConfigPlayersMovementServerRpc()        
    {
        foreach (var player in networkClientGameObjectList)
        {
            DisablePlayerMovement(player, false);
        }


    }
    [ServerRpc]
    private void ReSpawnPlayersServerRpc()
    {
        Debug.Log("TP COMBATE");
        int i = 0;
        foreach (var player in networkClientGameObjectList)
        {
            player.transform.position = SpawnSystemGame.instance.spawnPointsGame[i].position;
            player.GetComponent<FighterMovement>().vidaUI.currentHP.Value = player.GetComponent<FighterMovement>().vidaUI.maxHP;
            DisablePlayerMovement(player, true);
            player.GetComponent<Animator>().Play("idle");
            i++;
        }
    }
    private void DisablePlayerMovement(GameObject player, bool b) 
    {        
        player.GetComponent<FighterMovement>().countDownActive = b;
        if (!b) 
        {
            player.GetComponent<BoxCollider2D>().enabled = true;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        } 
        
        
    }
    [ServerRpc]
    public void PlayerDeadServerRpc() 
    {
        Debug.Log("JUGADOR MUERTO");
        currentPlayingPlayers--;
        if (currentPlayingPlayers <= 1)
        {
            Debug.Log("Solo queda 1 jugador, check de victoria");
            CheckLastOneStandingServerRpc();
            roundEnded = true;
        }
    }
    [ServerRpc]
    public void PlayerDisconnectedServerRpc() 
    {
        currentPlayingPlayers--;
        numPlayers--;
        if (currentPlayingPlayers <= 1 || numPlayers <= 1)
        {
            CheckLastOneStandingServerRpc();
            roundEnded = true;
        }
    }
    [ServerRpc]
    public void CheckLastOneStandingServerRpc() 
    {
        foreach (var player in networkClientGameObjectList)
        {
            if (player.GetComponent<FighterMovement>().vidaUI.currentHP.Value > 0)
            {                
                player.GetComponent<FighterMovement>().vidaUI.currentPoints.Value++;
            }
        }
    }

    public void GenerateClienObjectNetworkList() 
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client))
            {
                if (client.ClientId == 0)
                {
                    networkClientGameObjectList.Add(client.PlayerObject.transform.GetChild(0).gameObject);
                }
                else 
                {
                    networkClientGameObjectList.Add(client.PlayerObject.gameObject);
                }                
            }
        }
        numPlayers = networkClientGameObjectList.Count;
        currentPlayingPlayers = numPlayers;
    }

    [ServerRpc]
    public void SetAllHealthUIServerRpc() 
    {        
        for (int i = 0; i < networkClientGameObjectList.Count; i++)
        {            
            networkClientGameObjectList[i].GetComponent<FighterMovement>().vidaUI.SetHealthUIPositionClientRpc(i);
        }
        
    }
    [ClientRpc]
    public void changeCameraClientRpc()
    {
        Debug.Log("CHANGE CAMERA");
        scriptCamera.changeToLobby();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using Movement.Components;

public class PlayerName : NetworkBehaviour
{    
    public TMP_Text playerNameText;

    public NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>("Player", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    public override void OnNetworkSpawn()
    {        
        playerNameText.text = playerName.Value.ToString();
        playerName.OnValueChanged += OnNameChanged;
    }

    private void OnNameChanged(FixedString64Bytes oldVlaue, FixedString64Bytes newValue) 
    {
        playerName.Value = newValue;
        playerNameText.text = playerName.Value.ToString();
    }
}
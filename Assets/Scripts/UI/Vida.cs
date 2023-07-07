using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Vida : NetworkBehaviour
{
    [Header("Health Variables")]
    public NetworkVariable<float> currentHP = new NetworkVariable<float>();
    public Image lifebar;
    public float maxHP;

    [Header("Points Variables")]
    public NetworkVariable<int> currentPoints = new NetworkVariable<int>();
    public TMP_Text pointsTextBox;

    [Header("Winner Display Variables")]
    public string winnerName;
    public TMP_Text winnerTextBox;
    public GameObject winnerDisplayObject;


    float auxValue;

    void Start()
    {
        maxHP = 200f;
        SetInitHealthServerRpc();
        lifebar.color = Color.green;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetInitHealthServerRpc() 
    {
        currentHP.Value = maxHP;
        currentPoints.Value = 0;
    }

    public override void OnNetworkSpawn()
    {
        currentHP.OnValueChanged += OnVidaChanged;

        currentPoints.OnValueChanged += OnPointsChanged;
    }
    public override void OnNetworkDespawn()
    {
        currentHP.OnValueChanged -= OnVidaChanged;
        currentPoints.OnValueChanged -= OnPointsChanged;
    }
    private void OnVidaChanged(float oldValue, float newValue)
    {

        lifebar.fillAmount = newValue / maxHP;
        if (lifebar.fillAmount >= 0.5)
        {
            auxValue = 2 * (1 - lifebar.fillAmount);
            lifebar.color = new Vector4(auxValue, 1, 0, 1);
        }
        else if (lifebar.fillAmount < 0.5)
        {
            auxValue = 2 * lifebar.fillAmount;
            lifebar.color = new Vector4(1, auxValue, 0, 1);
        }

    }
    private void OnPointsChanged(int oldValue, int newValue) 
    {
        pointsTextBox.text = newValue.ToString();
    }

    [ClientRpc]
    public void SetHealthUIPositionClientRpc(int id)
    {        
        transform.GetChild(0).position = HealthUIPivots.Instance.pivots[id].position;
    }

    public void SetWinnersDisplay(string winner) 
    {
        HealthUIPivots.Instance.winnerDisplayObject.SetActive(true);
        HealthUIPivots.Instance.winnerTextBox.text = winner;       
    }
    public void DisableWinnersDisplay() 
    {
        HealthUIPivots.Instance.winnerDisplayObject.SetActive(false);
    }
}

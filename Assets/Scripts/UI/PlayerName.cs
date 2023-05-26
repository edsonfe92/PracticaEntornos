using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public string name;
    public TMP_Text playerNameText;

    public Transform playerTransform;

    void Start()
    {

    }

    void Update()
    {
        playerNameText.text = name;
        transform.localPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y + 0.5f, playerTransform.localPosition.z);
    }
}
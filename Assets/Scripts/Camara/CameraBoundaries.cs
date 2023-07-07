using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundaries : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D limiteCamara;
    CinemachineConfiner confiner;
    public Collider2D colliderCombate;
    public Collider2D colliderLobby;
    void Start()
    {        
        confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = limiteCamara;        
    }
    
    public void changeToBattle()
    {
        confiner.m_BoundingShape2D = colliderCombate;
    }
    public void changeToLobby() 
    {
        Debug.Log("DENTRO DEL CHANGE");
        confiner.m_BoundingShape2D = limiteCamara;
    }

}

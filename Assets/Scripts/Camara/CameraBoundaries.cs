using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundaries : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D limiteCamara;
    CinemachineConfiner confiner;
    void Start()
    {
        confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = limiteCamara;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

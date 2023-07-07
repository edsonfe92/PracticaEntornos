using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaSystem : MonoBehaviour
{
    public static VidaSystem instance;

    public GameObject[] vidaCanvas;

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
}

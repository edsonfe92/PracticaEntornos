using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystemGame : MonoBehaviour
{
    public static SpawnSystemGame instance;

    public Transform[] spawnPointsGame;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystemLobby : MonoBehaviour
{
    public static SpawnSystemLobby instance;

    public Transform[] spawnPointsLobby;

    /*public  void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public  void RemoveSpawnPoint(Transform transform)
    {
        spawnPoints.Remove(transform);
    }*/

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

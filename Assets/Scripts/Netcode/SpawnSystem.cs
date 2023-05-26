using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem instance;

    public List<Transform> spawnPoints = new List<Transform>();

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
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}

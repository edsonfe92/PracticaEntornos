using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUIPivots : MonoBehaviour
{
    public List<Transform> pivots = new List<Transform>();
    public TMP_Text winnerTextBox;
    public GameObject winnerDisplayObject;

    public static HealthUIPivots Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

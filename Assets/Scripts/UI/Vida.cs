using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    public Image lifebar;
    public float maxHP = 200.0f;
    [Range(0.0f, 200.0f)] //TODO cambiar esto para hacerlo bien
    public float currentHP;
    float auxValue;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        lifebar.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        lifebar.fillAmount = currentHP / maxHP;
        if (lifebar.fillAmount >= 0.5)
        {
            auxValue = 2 * (1-lifebar.fillAmount);
            lifebar.color = new Vector4(auxValue, 1, 0, 1);
        }else if(lifebar.fillAmount < 0.5)
        {
            auxValue = 2 * lifebar.fillAmount;
            lifebar.color = new Vector4(1, auxValue, 0, 1);
        }
    }
}

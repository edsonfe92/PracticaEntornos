using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Timer : MonoBehaviour 
{ 
    public int tMax;
    /*private int contador = 0;
    private int timeValue = 1;  */  
    private bool isTimerFinished = false;

    Coroutine timer;

    /*private void OnEnable()
    {
            InvokeRepeating("Cronometro", 0f, 1f);
    }*/
    
    public void StartTimerServerRpc() 
    {
        timer = StartCoroutine(TimerCounter());
    }
    public void StopTimerServerRpc() 
    {
        StopCoroutine(TimerCounter());
    }
    public void SetTimerServerRpc(int a) 
    {
        tMax= a;
    }
    IEnumerator TimerCounter() 
    {
        while (tMax> 0)
        {
            yield return new WaitForSeconds(1);
            tMax--;
            if (tMax<= 0)
            {                
                FinishTimer();
            }
        }        
    }
   /* void Cronometro()
    {
        if (timeValue > 0)
        {
            contador++;
            timeValue = tMax - contador;
            UITimerText.text = timeValue.ToString();
        }
        else
        {
            FinishTimer();            
        }
    }*/

    void FinishTimer()
    {
        isTimerFinished = true;
    }

    public bool IsTimerFinished()
    {
        return isTimerFinished;
    }
}

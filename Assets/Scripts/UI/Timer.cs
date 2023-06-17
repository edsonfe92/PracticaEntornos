using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Timer : NetworkBehaviour
{
    public NetworkVariable<int> tMax = new NetworkVariable<int>();    
    /*private int contador = 0;
    private int timeValue = 1;  */  
    private bool isTimerFinished = false;
    private bool isTimerActive = false;

    Coroutine timer;

    /*private void OnEnable()
    {
            InvokeRepeating("Cronometro", 0f, 1f);
    }*/
    [ServerRpc]
    public void StartTimerServerRpc() 
    {
        isTimerActive = true;
        timer = StartCoroutine(TimerCounter());
    }
    [ServerRpc]
    public void StopTimerServerRpc() 
    {
        StopCoroutine(TimerCounter());
    }
    [ServerRpc]
    public void SetTimerServerRpc(int a) 
    {
        tMax.Value = a;
    }
    IEnumerator TimerCounter() 
    {        
        while (tMax.Value > 0)
        {
            yield return new WaitForSeconds(1);
            tMax.Value--;
            if (tMax.Value <= 0)
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
        isTimerActive = false;
    }

    public bool IsTimerFinished()
    {
        return isTimerFinished;
    }
    public bool IsTimerActive() 
    {
        return isTimerActive;
    }
}

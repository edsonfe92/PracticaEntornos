using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public Text UITimerText;
    public int Tmax;
    private int contador = 0;
    private int timeValue = 1;
    // Update is called once per frame
    private bool isTimerFinished = false;

    private void OnEnable()
    {
            InvokeRepeating("Cronometro", 0f, 1f);
    }

    void Cronometro()
    {
        if (timeValue > 0)
        {
            contador++;
            timeValue = Tmax - contador;
            UITimerText.text = timeValue.ToString();
        }
        else
        {
            FinishTimer();
            print("C");
        }
    }

    void FinishTimer()
    {
        isTimerFinished = true;
    }

    public bool IsTimerFinished()
    {
        return isTimerFinished;
    }
}

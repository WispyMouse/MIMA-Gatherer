using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManagement : MonoBehaviour
{
    private static TimerManagement Instance { get; set; }

    private static Queue<Timer> InactiveTimersInPool = new Queue<Timer>();

    [SerializeReference]
    private Timer TimerPF;

    private void Awake()
    {
        Instance = this;
    }

    public static void ReturnTimerToPool(Timer toReturn)
    {
        toReturn.gameObject.SetActive(false);
        InactiveTimersInPool.Enqueue(toReturn);
    }

    public static Timer GetTimer(float duration, Vector3 position, float currentValue = 0)
    {
        Timer newTimer;
        
        if (InactiveTimersInPool.Count > 0)
        {
            newTimer = InactiveTimersInPool.Dequeue();
            newTimer.transform.position = position;
            newTimer.gameObject.SetActive(true);
        }
        else
        {
            newTimer = GameObject.Instantiate(Instance.TimerPF, position, Quaternion.identity);
        }
        
        newTimer.Duration = duration;
        newTimer.CurrentValue = currentValue;

        return newTimer;
    }
}

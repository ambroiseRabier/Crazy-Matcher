using System;
using System.Collections;
using UnityEngine;

public delegate void TimerUpdatedEventHandler(Timer sender, float remainingSeconds);
public delegate void TimerEventHandler(Timer sender);

public class Timer : MonoBehaviour
{
    public event TimerUpdatedEventHandler OnTimeUpdated;
    public event TimerEventHandler OnEnded;

    public float InitialRemainingSeconds { get; private set; }
    public float RemainingSeconds { get; private set; }
    public bool IsPlaying { get; private set; }
    public bool IsStopped { get { return !IsPlaying; } }
    public float MinIntervalForTimeUpdate { get; set; }
    public bool IsDestroyed { get; private set; }
    public bool IsEnded {
        get
        {
            return RemainingSeconds <= 0;
        }
    }

    private float m_LastEmitUpdateRemainingSeconds;


    private void Start()
    {
        StartCoroutine(CustomUpdate());
    }


    public static Timer CreateInstance(float remainingSeconds)
    {
        Timer timer = new GameObject("Timer").AddComponent<Timer>();
        timer.InitialRemainingSeconds = remainingSeconds;

        return timer;
    }





    public void AddSeconds(float seconds)
    {
        RemainingSeconds += seconds;
        EmitTimeUpdate();
    }

    public void RemoveSeconds(float seconds)
    {
        AddSeconds(-Mathf.Abs(seconds));
    }

    public void ResetAndPlay()
    {
        Reset();
        Resume();
    }

    public void ResetAndStop()
    {
        Reset();
        Stop();
    }

    private void Reset()
    {
        RemainingSeconds = InitialRemainingSeconds;
        EmitTimeUpdate();
    }

    public void Stop()
    {
        IsPlaying = false;
    }

    public void Resume()
    {
        IsPlaying = true;
    }

    public void Destroy()
    {
        Destroy(gameObject);
        IsDestroyed = true;
    }

    private void Update()
    {
    }

    IEnumerator CustomUpdate()
    {
        while(true)
        {
            if (IsPlaying)
            {
                RemainingSeconds -= Time.unscaledDeltaTime;


                if (m_LastEmitUpdateRemainingSeconds - RemainingSeconds >= MinIntervalForTimeUpdate)
                {
                    EmitTimeUpdate();
                }

                if (IsEnded)
                {
                    Stop();

                    if (OnEnded != null)
                    {
                        OnEnded(this);
                    }
                }
            }

            yield return null;
        }

    }

    private void EmitTimeUpdate()
    {
        m_LastEmitUpdateRemainingSeconds = RemainingSeconds;

        if (OnTimeUpdated != null)
            OnTimeUpdated(this, RemainingSeconds);
    }

    public static void DelayThenPerform(float delay, Action actionToPerform)
    {
        Timer timer = CreateInstance(delay);
        TimerEventHandler delegateAction = null;

        delegateAction = (Timer timerSender) => {
            actionToPerform();
            timer.OnEnded -= delegateAction;
            timer.Destroy();
        };

        timer.OnEnded += delegateAction;
        timer.ResetAndPlay();
    }
}

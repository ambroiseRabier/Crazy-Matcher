using System;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Animator))]
public abstract class CanvasScreen<T> : Singleton<T> where T : Component
{
    private Animator m_animator;

    public delegate void CanvasScreenEventHandler(T sender);
    public delegate void CanvasScreenRatioProgressEventHandler(T sender, float ratio);

    private static readonly int DESIRED_STATE_PARAM = Animator.StringToHash("DesiredState");
    private static readonly int OPENED_STATE        = Animator.StringToHash("Opened");
    private static readonly int OPENING_STATE       = Animator.StringToHash("Opening");
    private static readonly int CLOSED_STATE        = Animator.StringToHash("Closed");
    private static readonly int CLOSING_STATE       = Animator.StringToHash("Closing");

    public event CanvasScreenRatioProgressEventHandler OnOpening;
    public event CanvasScreenEventHandler OnOpened;
    public event CanvasScreenEventHandler OnClosed;
    public event CanvasScreenRatioProgressEventHandler OnClosing;

    public bool IsOpened { get { return State == CanvasScreenState.OPENED; } }
    public bool IsClosed { get { return State == CanvasScreenState.CLOSED; } }

    private CanvasScreenState DesiredState
    {
        get
        {
            return (CanvasScreenState)m_animator.GetInteger(DESIRED_STATE_PARAM);
        }
        set
        {
            m_animator.SetInteger(DESIRED_STATE_PARAM, (int)value);
        }
    }

    public CanvasScreenState State
    {
        get
        {
            int currentHashedStateName = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

            if (currentHashedStateName == OPENED_STATE)
            {
                return CanvasScreenState.OPENED;
            }
            else if (currentHashedStateName == OPENING_STATE)
            {
                return CanvasScreenState.OPENING;
            }
            else if (currentHashedStateName == CLOSED_STATE)
            {
                return CanvasScreenState.CLOSED;
            }
            else if (currentHashedStateName == CLOSING_STATE)
            {
                return CanvasScreenState.CLOSING;
            }

            throw new System.Exception("State on animator does not match on " + typeof(CanvasScreenState).Name);
        }
    }

    private CanvasScreenMachineState[] m_canvasScreenMachineStates;

    protected override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();
        m_animator.Play("Closing", 0, 1f);
        DesiredState = CanvasScreenState.CLOSED;
        m_canvasScreenMachineStates = m_animator.GetBehaviours<CanvasScreenMachineState>();

        CanvasScreenMachineState machineState;

        for (int i = m_canvasScreenMachineStates.Length-1; i>-1; i--)
        {
            machineState = m_canvasScreenMachineStates[i];
            machineState.OnEnter += CanvasScreenMachineStates_OnEnter;

            if (machineState.CanvasScreenState == CanvasScreenState.CLOSING || machineState.CanvasScreenState == CanvasScreenState.OPENING) {
                machineState.OnUpdate += CanvasScreenMachineStates_OnUpdates;
            } 
        }
    }

    private void CanvasScreenMachineStates_OnUpdates(CanvasScreenState state, float normalizedTime)
    {
        switch (state)
        {
            case CanvasScreenState.CLOSING:
                if (OnClosing != null)
                    OnClosing(this as T, normalizedTime);
                break;
            case CanvasScreenState.OPENING:
                if (OnOpening != null)
                    OnOpening(this as T, normalizedTime);
                break;
        }
    }

    private void CanvasScreenMachineStates_OnEnter(CanvasScreenState state)
    {
        switch (state)
        {
            case CanvasScreenState.CLOSED:
                if (OnClosed != null)
                    OnClosed(this as T);
                break;
            case CanvasScreenState.CLOSING:
                if (OnClosing != null)
                    OnClosing(this as T, 0f);
                break;
            case CanvasScreenState.OPENED:
                if (OnOpened != null)
                    OnOpened(this as T);
                break;
            case CanvasScreenState.OPENING:
                if (OnOpening != null)
                    OnOpening(this as T, 0f);
                break;
        }
    }

    /// <summary>
    /// Start transition opening if not opened.
    /// Invoke actionOnOpened when opening transition is ended or invoke actionOnClosed directely if the state is already opened
    /// </summary>
    /// <param name="actionOnOpened"></param>
    public void Open(Action actionOnOpened = null)
    {
        ChangeStateThenPerformCallBackOnCompleted(CanvasScreenState.OPENED, OnOpened, actionOnOpened);
    }

    /// <summary>
    /// Start transition closing if not closed.
    /// Invoke actionOnClosed when closing transition is ended or invoke actionOnClosed directely if the state is already closed
    /// </summary>
    /// <param name="actionOnClosed"></param>
    public void Close(Action actionOnClosed = null)
    {
        ChangeStateThenPerformCallBackOnCompleted(CanvasScreenState.CLOSED, OnClosed, actionOnClosed);
    }

    private void ChangeStateThenPerformCallBackOnCompleted(CanvasScreenState targetedState, CanvasScreenEventHandler eventHandler, Action callBack)
    {
        if (callBack != null)
        {
            if (State == targetedState)
            {
                callBack();
                return;
            }
            else
            {
                CanvasScreenEventHandler onStatedCompletedCallback = null;

                onStatedCompletedCallback = (T canvasScreen) => {
                    callBack();
                    eventHandler -= onStatedCompletedCallback;
                };

                eventHandler += onStatedCompletedCallback;
            }
        }

        DesiredState = targetedState;
    }

    private void OnDestroy()
    {
        CanvasScreenMachineState machineState;

        for (int i = m_canvasScreenMachineStates.Length - 1; i > -1; i--)
        {
            machineState = m_canvasScreenMachineStates[i];

            machineState.OnEnter  -= CanvasScreenMachineStates_OnEnter;
            machineState.OnUpdate -= CanvasScreenMachineStates_OnUpdates;
        }
    }
}
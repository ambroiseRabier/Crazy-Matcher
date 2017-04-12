using System;
using UnityEngine;

public class CanvasScreenMachineState : StateMachineBehaviour
{
    [SerializeField] private CanvasScreenState m_canvasScreenState;
    public CanvasScreenState CanvasScreenState { get { return m_canvasScreenState; } }

    public event Action<CanvasScreenState> OnEnter;
    public event Action<CanvasScreenState, float> OnUpdate;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetBehaviours<CanvasScreenMachineState>();
        if (OnEnter != null)
            OnEnter(m_canvasScreenState);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnUpdate != null)
            OnUpdate(m_canvasScreenState, stateInfo.normalizedTime);
    }
}
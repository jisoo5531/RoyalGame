using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_Sender;

    public IState<T> currrentState { get; set; }

    public StateMachine(T sender, IState<T> state, float speed, bool isPrince)
    {
        m_Sender = sender;
        SetState(state, speed, isPrince);
    }
    public void SetState(IState<T> state, float speed, bool isPrince)
    {
        if (m_Sender == null)
        {
            return;
        }

        if (currrentState == state)
        {
            return;
        }
        if (currrentState != null)
        {
            currrentState.OperateExit(m_Sender);
        }

        currrentState = state;

        if (currrentState != null)
        {
            currrentState.OperateEnter(m_Sender, speed, isPrince);
        }

    }
    public void DoOperateUpdate(bool isRun)
    {
        if (m_Sender == null)
        {
            return;
        }
        currrentState.OperateUpdate(m_Sender, isRun);
    }
}

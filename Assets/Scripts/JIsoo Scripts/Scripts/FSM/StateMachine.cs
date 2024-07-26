using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_Sender;

    public IState<T> currrentState { get; set; }

    public StateMachine(T sender, IState<T> state, float speed)
    {
        m_Sender = sender;
        SetState(state, speed);
    }
    public void SetState(IState<T> state, float speed)
    {
       // Debug.Log("SetState : " + state);

        if (m_Sender == null)
        {
           // Debug.LogError("m_sender ERROR");
            return;
        }

        if (currrentState == state)
        {
            //Debug.LogWarningFormat("Same State : ", state);
            return;
        }
        if (currrentState != null)
        {
            currrentState.OperateExit(m_Sender);
        }

        currrentState = state;

        if (currrentState != null)
        {
            currrentState.OperateEnter(m_Sender, speed);
        }

    }
    public void DoOperateUpdate()
    {
        if (m_Sender == null)
        {
            return;
        }
        currrentState.OperateUpdate(m_Sender);
    }
}

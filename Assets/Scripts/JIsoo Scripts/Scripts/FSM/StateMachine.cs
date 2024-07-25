using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_Sender;

    public IState<T> currrentState { get; set; }

    public StateMachine(T sender, IState<T> state)
    {
        m_Sender = sender;
        SetState(state);
    }
    public void SetState(IState<T> state)
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
            currrentState.OperateEnter(m_Sender);
        }

        Debug.Log("SetNextState : " + state);
    }
    public void DoOperateUpdate()
    {
        if (m_Sender == null)
        {
            Debug.LogError("Invalid m_Sender");
            return;
        }
        currrentState.OperateUpdate(m_Sender);
    }
}

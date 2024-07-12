using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit : Unit
{
    protected override void Start()
    {
        base.Start();

        IState<Unit> move = new UnitMove();
        dicState.Add(UnitState.Move, move);
    }

    protected override void StateTransition()
    {
        if (targetTransform)
        {
            float distance = Vector3.Distance(targetTransform.position, transform.position);
            if (distance < detectionRange)
            {
                SetState(UnitState.Attack);
            }
            else
            {
                SetState(UnitState.Move);
            }
            //else if (stateMachine.currrentState != dicState[UnitState.Attack]) // 공격 중이 아닐 때만 이동 상태로 전이
            //{
                
            //}
        }
        else
        {
            SetState(UnitState.Move);
            //SetState(UnitState.Idle);
        }
    }
}

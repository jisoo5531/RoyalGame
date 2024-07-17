using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit : Unit
{
    public float moveSpeed;

    protected override void InitializeUnitData(UnitData_SO unit)
    {
        base.InitializeUnitData(unit);
        Debug.Log("자식 초기화");
        moveSpeed = unit.moveSpeed;
    }
    protected override void Start()
    {
        base.Start();

        IState<Unit> move = new UnitMove();
        dicState.Add(UnitState.Move, move);
    }

    protected override void StateTransition()
    {
        Debug.Log("자식 상태 변화");
        if (targetTransform)
        {
            float distance = Vector3.Distance(targetTransform.position, transform.position);
            if (distance < range)
            {
                SetState(UnitState.Attack);
            }
            else
            {
                SetState(UnitState.Move);
            }            
        }
        else
        {
            SetState(UnitState.Move);            
        }
    }
}

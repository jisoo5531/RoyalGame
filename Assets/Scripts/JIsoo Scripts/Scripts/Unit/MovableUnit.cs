using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit : Unit
{
    public float moveSpeed;
    TargetFollowUnit targetFollowUnit;
    

    private void Awake()
    {
        targetFollowUnit = GetComponent<TargetFollowUnit>();

    }

    protected override void InitializeUnitData(UnitData_SO unit)
    {
        base.InitializeUnitData(unit);

        moveSpeed = unit.moveSpeed;
    }

    protected override void Start()
    {
        base.Start();

        IState<Unit> move = new UnitMove();
        dicState.Add(UnitState.Move, move);
        if (targetFollowUnit != null)
        {
            targetFollowUnit.speed = moveSpeed;
            targetFollowUnit.range = range;
        }
    }

    protected override void StateTransition()
    {
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

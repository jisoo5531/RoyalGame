using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismovableUnit : Unit
{
    private int lifeTime;


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
                SetState(UnitState.Idle);
            }
        }
        else
        {
            SetState(UnitState.Idle);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffenseTower : Unit
{
    private float lifeTime;

    protected override void InitializeUnitData(UnitData_SO unit)
    {
        base.InitializeUnitData(unit);

        lifeTime = unit.lifeTime;
    }
    private void Update()
    {
        UpdateLifeTime();
    }

    //protected override void StateTransition()
    //{
    //    if (targetTransform)
    //    {
    //        float distance = Vector3.Distance(targetTransform.position, transform.position);
    //        if (distance < range)
    //        {
    //            SetState(UnitState.Attack);
    //        }
    //        else
    //        {
    //            SetState(UnitState.Idle);
    //        }
    //    }
    //    else
    //    {
    //        SetState(UnitState.Idle);
    //    }
    //}

    private void UpdateLifeTime()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeffenseTower : Unit
{
    private float lifeTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        InitializeUnitData(UnitSpawner.instance.selectedUnit);
    }
    private void Start()
    {
        InitStateMachine();
    }

    protected override void InitializeUnitData(UnitData_SO unit)
    {
        base.InitializeUnitData(unit);

        lifeTime = unit.lifeTime;
    }

    protected override void InitStateMachine()
    {
        base.InitStateMachine();
    }
    private void Update()
    {
        UpdateLifeTime();

        //stateMachine.DoOperateUpdate();

        //if (this.attackTarget == AttackTarget.All)
        //{
        //    DetectEnemyManager.instance.CheckDetectAllEnemy(detectionRange, this.transform, targetFollowUnit, isMove);
        //}
        //else
        //{
        //    DetectEnemyManager.instance.CheckDetectEnemyTower(detectionRange, this.transform, targetFollowUnit, isMove);
        //}
        //StateTransition();
    }

    //private void StateTransition()
    //{
    //    if (DetectEnemyManager.instance == null)
    //        return;

    //    int index = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.enemyUnitArr);
    //    if (DetectEnemyManager.instance.enemyUnitArr[index] != null)
    //    {
    //        Transform enemy = DetectEnemyManager.instance.enemyUnitArr[index].transform;
    //        float distance = Vector3.Distance(enemy.position, transform.position);
    //        if (distance < range)
    //        {
    //            targetFollowUnit.isAttack = true;
    //            SetState(UnitState.Attack);
    //        }
    //        else
    //        {
    //            SetState(UnitState.Move);
    //        }
    //    }
    //    else
    //    {
    //        SetState(UnitState.Move);
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

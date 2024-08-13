using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovableTest : Unit
{
    public float moveSpeed;
    TargetFollowUnit targetFollowUnit;
    public bool isMove = false;
    public UnitData_SO unitData;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        targetFollowUnit = GetComponent<TargetFollowUnit>();


        range = 20;
        // InitializeUnitData(UnitSpawner.instance.selectedUnit);

       // InitializeUnitData(unitData);
    }
    private void Start()
    {
        InitStateMachine();
    }

    protected override void InitStateMachine()
    {
        base.InitStateMachine();
        IState<Unit> move = new UnitMove();
        dicState.Add(UnitState.Move, move);
        if (targetFollowUnit != null)
        {
            targetFollowUnit.speed = this.moveSpeed;
        }
    }

    //protected override void InitializeUnitData(UnitData_SO unit)
    //{
    //    base.InitializeUnitData(unit);
    //    moveSpeed = unit.moveSpeed;
    //    SendDamage(unit.damage);
    //}

    private void Update()
    {
        //stateMachine.DoOperateUpdate();

        if (!targetFollowUnit.isAttack)
        {
            //DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, this.attackTarget);
            StateTransition(targetFollowUnit.target);
        }

        // TODO : target을 바라볼 떄
        transform.LookAt(targetFollowUnit.target);
    }

    private void StateTransition(Transform target)
    {
        if (DetectEnemyManager.instance == null || target == null)
            return;

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= range)
        {
            targetFollowUnit.isAttack = true;
            SetState(UnitState.Attack, 1);
        }
        else
        {
            SetState(UnitState.Move, 1);
        }
    }
}

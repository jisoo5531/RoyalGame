using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovableUnit : Unit
{
    public float moveSpeed;
    TargetFollowUnit targetFollowUnit;
    public bool isMove = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        targetFollowUnit = GetComponent<TargetFollowUnit>();

        InitializeUnitData(UnitSpawner.instance.selectedUnit);


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
            targetFollowUnit.range = range;
        }
    }

    protected override void InitializeUnitData(UnitData unit)
    {
        base.InitializeUnitData(unit);
        moveSpeed = unit.unitInfo.unitStat.moveSpeed;
        SendDamage(unit.unitInfo.unitStat.damage);
    }

    private void Update()
    {
        stateMachine.DoOperateUpdate();

        if (!targetFollowUnit.isAttack)
        {
            DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, this.attackTarget);
            StateTransition(targetFollowUnit.target);
        }
    }

    private void StateTransition(Transform target)
    {
        if (DetectEnemyManager.instance == null || target == null)
            return;

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= range)
        {
            targetFollowUnit.isAttack = true;
            SetState(UnitState.Attack);
        }
        else
        {
            SetState(UnitState.Move);
        }
    }
}

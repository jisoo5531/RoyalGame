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

    protected override void InitializeUnitData(UnitData_SO unit)
    {
        base.InitializeUnitData(unit);
        moveSpeed = unit.moveSpeed;
    }

    private void Update()
    {
        stateMachine.DoOperateUpdate();

        if(this.attackTarget == AttackTarget.All)
        {
            DetectEnemyManager.instance.CheckDetectAllEnemy(detectionRange, this.transform, targetFollowUnit, isMove);
        }
        else
        {
            DetectEnemyManager.instance.CheckDetectEnemyTower(detectionRange, this.transform, targetFollowUnit, isMove);
        }
        StateTransition();
    }

    private void StateTransition()
    {
        if (DetectEnemyManager.instance == null)
            return;

        int index = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.enemyUnitArr);
        if (DetectEnemyManager.instance.enemyUnitArr[index] != null)
        {
            Transform enemy = DetectEnemyManager.instance.enemyUnitArr[index].transform;
            float distance = Vector3.Distance(enemy.position, transform.position);
            if (distance < range)
            {
                targetFollowUnit.isAttack = true;
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

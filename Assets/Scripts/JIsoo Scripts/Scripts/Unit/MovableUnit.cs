using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovableUnit : Unit
{
    public float moveSpeed;
    TargetFollowUnit targetFollowUnit;
    Transform enemyUnit;
    Transform enemyTower;

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
        CheckDetectEnemy();
        StateTransition();
    }

    public void CheckDetectEnemy()
    {
        if (DetectEnemyManager.instance != null)
        {
            int unitIndex = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.enemyUnit);
            int towerIndex = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.towerArr);
            if (DetectEnemyManager.instance.enemyUnit[unitIndex] != null || DetectEnemyManager.instance.towerArr[towerIndex] != null)
            {
                enemyUnit = DetectEnemyManager.instance.enemyUnit[unitIndex].transform;
                enemyTower = DetectEnemyManager.instance.towerArr[towerIndex].transform;

                if (targetFollowUnit.target == enemyUnit || targetFollowUnit.target == enemyTower) return;

                float distance = Vector3.Distance(enemyUnit.position, transform.position);

                if (distance <= detectionRange)
                {
                    targetFollowUnit.target = enemyUnit;
                    return;
                }
                else
                {
                    targetFollowUnit.target = enemyTower;
                }
                targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<Collider>();
                PathRequestManager.RequestPath(transform.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
                return;
            }
        }
    }

    private void StateTransition()
    {
        if (DetectEnemyManager.instance != null)
        {
            int index = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.enemyUnit);
            if (DetectEnemyManager.instance.enemyUnit[index] != null)
            {
                Transform enemy = DetectEnemyManager.instance.enemyUnit[index].transform;
                float distance = Vector3.Distance(enemy.position, transform.position);
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
}

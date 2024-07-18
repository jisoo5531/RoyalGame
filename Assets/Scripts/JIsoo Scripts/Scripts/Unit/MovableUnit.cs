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
        SendDamage(unit.damage);
    }

    private void Update()
    {
        stateMachine.DoOperateUpdate();

        CheckDetectEnemy();
        StateTransition();
    }

    public void CheckDetectEnemy()
    {
        if (DetectEnemyManager.instance == null)
            return;

        int unitIndex = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.enemyUnit);
        int towerIndex = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.towerArr);

        if (DetectEnemyManager.instance.enemyUnit[unitIndex] == null && DetectEnemyManager.instance.towerArr[towerIndex] == null)
            return;

        enemyUnit = DetectEnemyManager.instance.enemyUnit[unitIndex]?.transform;
        enemyTower = DetectEnemyManager.instance.towerArr[towerIndex]?.transform;

        float distance = Vector3.Distance(enemyUnit.position, transform.position);

        if (distance <= detectionRange && targetFollowUnit.target != enemyUnit)
        {
            targetFollowUnit.target = enemyUnit;
        }
        else if (distance > detectionRange && targetFollowUnit.target != enemyTower)
        {
            targetFollowUnit.target = enemyTower;
        }
        else
        {
            return;
        }
        targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<CharacterController>();

        print("Å½Áö");
        if (isMove)
        {
            print("ÀÌµ¿ Áß");
            PathRequestManager.RequestPath(transform.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
            targetFollowUnit.isMove = true;
        }
    }

    private void StateTransition()
    {
        if (DetectEnemyManager.instance == null)
            return;

        int index = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.enemyUnit);
        if (DetectEnemyManager.instance.enemyUnit[index] != null)
        {
            Transform enemy = DetectEnemyManager.instance.enemyUnit[index].transform;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovableUnit : Unit
{
    public float moveSpeed;
    TargetFollowUnit targetFollowUnit;


    private void Awake()
    {
        targetFollowUnit = GetComponent<TargetFollowUnit>();
        anim = GetComponent<Animator>();
    }

    protected override void InitializeUnitData(UnitData_SO unit)
    {
        base.InitializeUnitData(unit);
        print("자식 클래스 호출");
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

    private void Update()
    {
        CheckDetectEnemy();
        StateTransition();
    }

    public void CheckDetectEnemy()
    {
        if (DetectEnemyManager.instance != null)
        {
            int index = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.enemyUnit);
            if (DetectEnemyManager.instance.enemyUnit[index] != null)
            {
                Transform enemy = DetectEnemyManager.instance.enemyUnit[index].transform;
                float distance = Vector3.Distance(enemy.position, transform.position);
                if (distance <= detectionRange)
                {
                    targetFollowUnit.target = enemy;
                }
                else
                {
                    int towerIndex = DetectEnemyManager.instance.CheckEnemyDistance(this.transform, DetectEnemyManager.instance.towerArr);
                    print(towerIndex);
                    print(targetFollowUnit.target == null);
                    targetFollowUnit.target = DetectEnemyManager.instance.towerArr[towerIndex].transform;
                }
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

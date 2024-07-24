using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovableUnit : Unit
{
    public float moveSpeed;
    public float detectionRange;
    TargetFollowUnit targetFollowUnit;
    public bool initialWaitDone = false;
    public bool isMove = false;
    public double moveDelay;
    public bool isSpawn = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        targetFollowUnit = GetComponent<TargetFollowUnit>();

        InitializeUnitData(UnitSpawner.instance.selectedUnit);


    }
    private void Start()
    {
        spawnTime = (float)PhotonNetwork.Time;
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

    protected override void InitializeUnitData(AllCardData unit)
    {
        base.InitializeUnitData(unit);
        if (unit is UnitInfoData unitInfo)
        {
            moveSpeed = unitInfo.moveSpeed;
            detectionRange = unitInfo.detectRange;
        }

        SendDamage(unit.damage);
    }

    private void Update()
    {
        if (isSpawn)
        {
            if (!initialWaitDone && (PhotonNetwork.Time - spawnTime) >= moveDelay)
            {
                isMove = true;
                DetectEnemyManager.instance.FirstMovePath(this.transform, targetFollowUnit);
                initialWaitDone = true;
            }
            stateMachine.DoOperateUpdate();

            if (!targetFollowUnit.isAttack)
            {
                DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, attackTarget);

                if (isMove)
                {
                    StateTransition(targetFollowUnit.target);
                }
            }
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

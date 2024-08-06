using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovableUnit : Unit
{
    public float moveSpeed;
    public float detectionRange;
    TargetFollowUnit targetFollowUnit;
    public bool isWait = true;
    public bool isMove = false;
    public double moveDelay;
    public bool isSpawn = false;

    RangedUnit rangedUnit;
    Damaging damaging;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        targetFollowUnit = GetComponent<TargetFollowUnit>();
        rangedUnit = GetComponent<RangedUnit>();
        damaging = GetComponentInChildren<Damaging>();
        canvasInfo = GetComponent<UnitCanvasInfo>();

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
            attackSpeed = unitInfo.attackSpeed;
        }
        if (rangedUnit != null)
        {
            rangedUnit.damage = this.damage;
        }

        if (damaging != null)
        {
            damaging.damage = this.damage;
        }
        canvasInfo.maxHP = this.HP;
        canvasInfo.HP = this.HP;
        SendDamage(unit.damage);
    }

    private void Update()
    {
        if (isSpawn && photonView.IsMine)
        {
            if (!isWait)
            {
                isMove = true;
                DetectEnemyManager.instance.FirstMovePath(this.transform, targetFollowUnit);
                isWait = true;
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
            else
            {
                if (targetFollowUnit.target != null)
                {
                    Vector3 direction = (targetFollowUnit.target.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                }
                else
                {
                    targetFollowUnit.isAttack = false;
                    isMove = true;
                    
                    DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, attackTarget);

                    if (isMove)
                    {
                        StateTransition(targetFollowUnit.target);
                    }
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
            isMove = false;
            targetFollowUnit.isAttack = true;
            SetState(UnitState.Attack, attackSpeed);
            // UpdateAnimationSpeed(attackSpeed);
        }
        else
        {
            SetState(UnitState.Move, 0.8f);
            // UpdateAnimationSpeed(0.8f);
        }
    }
    //public void UpdateAnimationSpeed(float speed)
    //{
    //    anim.speed = speed;
    //    photonView.RPC("SyncAnimationSpeed", RpcTarget.Others, speed);
    //}

    //[PunRPC]
    //public void SyncAnimationSpeed(float speed)
    //{
    //    anim.speed = speed;
    //}
}

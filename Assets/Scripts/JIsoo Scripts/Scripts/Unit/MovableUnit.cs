using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MovableUnit : Unit
{
    #region public 변수
    public float moveSpeed;
    public float detectionRange;
    public bool isWait = true;
    public bool isMove = false;
    public double moveDelay;
    public bool isSpawn = false;
    #endregion

    #region private 변수
    RangedUnit rangedUnit;
    Damaging damaging;
    TargetFollowUnit targetFollowUnit;

    private bool isRunning = false;
    private bool isRun = false;
    private bool isRange = false;
    #endregion

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
            stateMachine.DoOperateUpdate(isRunning);

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

                    if (PhotonNetwork.IsMasterClient)
                    {
                        photonView.RPC("CanvasRotate", RpcTarget.Others, transform.localEulerAngles.y);
                        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -transform.localEulerAngles.y, 0);
                    }
                    else
                    {
                        photonView.RPC("CanvasRotate", RpcTarget.Others, transform.localEulerAngles.y - 180);
                        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -transform.localEulerAngles.y + 180, 0);
                    }
                    StateTransition(targetFollowUnit.target);
                    if (isRange)
                    {
                        DetectEnemyManager.instance.FirstMovePath(this.transform, targetFollowUnit);
                    }
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


    [PunRPC]
    private void CanvasRotate(float rotateY)
    {
        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -rotateY + 180, 0);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == 11)
    //    {
    //        //DetectEnemyManager.instance.FirstMovePath(this.transform, targetFollowUnit);
    //    }
    //}

    private void StateTransition(Transform target)
    {
        if (DetectEnemyManager.instance == null || target == null)
            return;

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= range)
        {
            isMove = false;
            isRun = false;
            isRunning = false;
            isRange = false;
            targetFollowUnit.isAttack = true;
            targetFollowUnit.speed = moveSpeed;
            SetState(UnitState.Attack, attackSpeed);
            if (damaging != null)
            {
                damaging.isWait = false;
            }
            // UpdateAnimationSpeed(attackSpeed);
        }
        else
        {
            isRange = true;
            SetState(UnitState.Move, 0.8f);
            if (isPrince && !isRun)
            {
                isRun = true;
                StartCoroutine(RunDelay());
            }
        }
    }

    IEnumerator RunDelay()
    {
        yield return new WaitForSeconds(2f);
        isRunning = true;
        targetFollowUnit.speed *= 2f;
        yield break;
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

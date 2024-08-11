using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    bool isTimer = true;

    Vector3 frontPoint, backPoint, leftPoint, rightPoint;
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
        if (!isSpawn || !photonView.IsMine) return;

        if (damaging != null) damaging.target = targetFollowUnit.target;

        if (!isWait)
        {
            DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, attackTarget);
            isMove = true;
            isTimer = false;
            isWait = true;
        }

        if (!isTimer)
        {
            stateMachine.DoOperateUpdate(isRunning);

            if (targetFollowUnit.isAttack)
            {
                if (targetFollowUnit.target != null)
                {
                    RotateTowardsTarget(targetFollowUnit.target);
                    StateTransition(targetFollowUnit.target);
                }
                else
                {
                    targetFollowUnit.isAttack = false;
                    StateTransition(targetFollowUnit.target);
                }
            }
            else if (isMove)
            {
                StateTransition(targetFollowUnit.target);
            }
        }
    }

    [PunRPC]
    private void CanvasRotate(float rotateY)
    {
        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -rotateY + 180, 0);
    }

    private void StateTransition(Transform target)
    {
        if (target == null || DetectEnemyManager.instance == null) return;

        if (CheckDis())
        {
            SetAttackState();
        }
        else
        {
            SetMoveState();
        }

        DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, attackTarget);
    }

    private bool CheckDis()
    {
        Collider collider = targetFollowUnit?.targetCollider;
        if (collider == null) return false;

        UpdateColliderPoints(collider);

        return CheckDistance();
    }

    private void UpdateColliderPoints(Collider collider)
    {
        if (collider is BoxCollider boxCollider)
        {
            Bounds bounds = boxCollider.bounds;
            frontPoint = bounds.max;
            backPoint = bounds.min;
            leftPoint = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
            rightPoint = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);
        }
        else if (collider is CapsuleCollider capsuleCollider)
        {
            Vector3 capsuleCenter = capsuleCollider.bounds.center;
            float radius = capsuleCollider.radius;
            frontPoint = capsuleCenter + Vector3.forward * radius;
            backPoint = capsuleCenter + Vector3.back * radius;
            leftPoint = capsuleCenter + Vector3.left * radius;
            rightPoint = capsuleCenter + Vector3.right * radius;
        }
    }

    private bool CheckDistance()
    {
        float[] distances = {
            Vector3.Distance(frontPoint, transform.position),
            Vector3.Distance(leftPoint, transform.position),
            Vector3.Distance(backPoint, transform.position),
            Vector3.Distance(rightPoint, transform.position)
        };

        for(int i = 0; i< distances.Length; i++)
        {
            if (distances[i] <= range) return true;
        }
        return false;
    }

    private void SetAttackState()
    {
        isMove = isRun = isRunning = isRange = false;
        targetFollowUnit.isAttack = true;
        targetFollowUnit.speed = moveSpeed;
        SetState(UnitState.Attack, attackSpeed);
        if (damaging != null) damaging.isWait = false;
    }

    private void SetMoveState()
    {
        isMove = isRange = true;
        targetFollowUnit.isAttack = false;
        SetState(UnitState.Move, 0.8f);
        if (isPrince && !isRun)
        {
            isRun = true;
            StartCoroutine(RunDelay());
        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        float rotationY = transform.localEulerAngles.y;
        photonView.RPC("CanvasRotate", RpcTarget.Others, PhotonNetwork.IsMasterClient ? rotationY : rotationY - 180);
        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -rotationY + (PhotonNetwork.IsMasterClient ? 0 : 180), 0);
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

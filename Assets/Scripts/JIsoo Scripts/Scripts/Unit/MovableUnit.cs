using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MovableUnit : Unit, IAttackable
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

    private int layer;
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
        layer = (1 << LayerMask.NameToLayer("EnemyTower") | (1 << LayerMask.NameToLayer("EnemyUnit")));
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

        if (damaging != null && targetFollowUnit.target != null) damaging.target = targetFollowUnit.target;

        if (!isWait)
        {
            isMove = true;
            isTimer = false;
            DetectEnemyManager.instance.FirstMovePath(this.transform, targetFollowUnit);
            isWait = true;
        }

        stateMachine.DoOperateUpdate(isRunning);

        if (targetFollowUnit.isAttack)
        {
            if (targetFollowUnit.target != null)
            {
                RotateTowardsTarget(targetFollowUnit.target);
            }
            else
            {
                isMove = true;
                targetFollowUnit.isAttack = false;
                DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, attackTarget);
            }
        }
        else
        {
            targetFollowUnit.isAttack = false;
            DetectEnemyManager.instance.CheckDetectEnemy(detectionRange, this.transform, targetFollowUnit, isMove, attackTarget);
        }
        StateTransition(targetFollowUnit.target);
    }

    [PunRPC]
    private void CanvasRotate(float rotateY)
    {
        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -rotateY + 180, 0);
    }

    private void StateTransition(Transform target)
    {
        if (target == null || isTimer || DetectEnemyManager.instance == null) return;

        if (CheckDis())
        {
            SetAttackState();
        }
        else
        {
            SetMoveState();
        }
    }

    private bool CheckDis()
    {
        if (targetFollowUnit.target == null) return false;

        Collider[] colliders = Physics.OverlapSphere(transform.position + (Vector3.up * 3f), range, layer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.Equals(targetFollowUnit.target.gameObject))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position + (Vector3.up * 3f), range);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);

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

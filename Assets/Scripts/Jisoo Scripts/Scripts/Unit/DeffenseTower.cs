using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeffenseTower : Unit, IAttackable
{
    protected float lifeTime;
    RangedUnit rangedUnit;
    TargetFollowUnit targetFollowUnit;
    private int result;
    public bool isSpawn = false;
    public bool isWait = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rangedUnit = GetComponent<RangedUnit>();
        canvasInfo = GetComponent<UnitCanvasInfo>();
        targetFollowUnit = GetComponent<TargetFollowUnit>();

        InitializeUnitData(UnitSpawner.instance.selectedUnit);
    }
    private void Start()
    {
        InitStateMachine();

        int numberOfIntervals = (int)(lifeTime / 0.1f);

        float damagePerInterval = maxHP / numberOfIntervals;

        result = (int)damagePerInterval;

        StartCoroutine(MinusLifeTime());

    }

    protected override void InitializeUnitData(AllCardData unit)
    {
        base.InitializeUnitData(unit);
        if (unit is DEFENSETOWERInfoData defenseTowerInfo)
        {
            lifeTime = defenseTowerInfo.lifeTime;
            attackSpeed = defenseTowerInfo.attackSpeed;
        }

        if (rangedUnit != null)
        {
            rangedUnit.damage = this.damage;
        }
        canvasInfo.maxHP = this.HP;
        canvasInfo.HP = this.HP;
    }

    protected override void InitStateMachine()
    {
        base.InitStateMachine();
    }

    IEnumerator MinusLifeTime()
    {
        while (true)
        {
            yield return null;
            if (!isWait)
            {
                yield return new WaitForSeconds(0.1f);
                canvasInfo.GetDamage(result);
            }
        }
    }

    private void Update()
    {
        if (!isSpawn || !photonView.IsMine) return;

        stateMachine.DoOperateUpdate(false);
        if (!targetFollowUnit.isAttack)
        {
            DetectEnemyManager.instance.CheckDetectEnemy(range, this.transform, targetFollowUnit, attackTarget);

            StateTransition(targetFollowUnit.target);
        }
        else
        {
            if (targetFollowUnit.target != null && !isWait)
            {
                RotateTowardsTarget(targetFollowUnit.target);
            }
            else
            {
                targetFollowUnit.isAttack = false;

                DetectEnemyManager.instance.CheckDetectEnemy(range, this.transform, targetFollowUnit, attackTarget);

                StateTransition(targetFollowUnit.target);
            }
        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);

        float rotationY = transform.localEulerAngles.y;

        if (photonView != null)
        {
            photonView.RPC("CanvasRotate", RpcTarget.Others, PhotonNetwork.IsMasterClient ? rotationY : rotationY - 180);
        }
        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -rotationY + (PhotonNetwork.IsMasterClient ? 0 : 180), 0);
    }

    [PunRPC]
    private void CanvasRotate(float rotateY)
    {
        canvasInfo.unitCanvas.transform.localEulerAngles = new Vector3(0, -rotateY + 180, 0);
    }

    private void StateTransition(Transform target)
    {
        if (DetectEnemyManager.instance == null || target == null)
            return;

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= range)
        {
            SetState(UnitState.Attack, attackSpeed);
            targetFollowUnit.isAttack = true;
            // UpdateAnimationSpeed(attackSpeed);
        }
        else
        {
            SetState(UnitState.Idle, 1f);
            targetFollowUnit.isAttack = false;
        }
    }
}

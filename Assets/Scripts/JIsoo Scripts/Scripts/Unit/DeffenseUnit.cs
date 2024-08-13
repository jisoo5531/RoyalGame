using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeffenseUnit : Unit, IAttackable
{
    protected float lifeTime;
    RangedUnit rangedUnit;
    TargetFollowUnit targetFollowUnit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rangedUnit = GetComponent<RangedUnit>();
        canvasInfo = GetComponent<UnitCanvasInfo>();
        targetFollowUnit = GetComponent<TargetFollowUnit>();
    }

    private void Start()
    {
        InitStateMachine();
    }

    public void InitData(float range, int damage)
    {
        this.range = range;
        this.damage = damage;

        if(rangedUnit != null)
        {
            rangedUnit.damage = this.damage;
        }
    }

    protected override void InitStateMachine()
    {
        base.InitStateMachine();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (targetFollowUnit.target != null)
        {
            stateMachine.DoOperateUpdate(false);
        }

        if (!targetFollowUnit.isAttack)
        {
            DetectEnemyManager.instance.CheckEnemyUnit(range, this.transform, targetFollowUnit);

            StateTransition(targetFollowUnit.target);
        }
        else
        {
            if (targetFollowUnit.target != null)
            {
                RotateTowardsTarget(targetFollowUnit.target);
            }
            else
            {
                targetFollowUnit.isAttack = false;

                DetectEnemyManager.instance.CheckEnemyUnit(range, this.transform, targetFollowUnit);

                StateTransition(targetFollowUnit.target);

                if(targetFollowUnit.target != null)
                {
                    StateIdle();
                }
            }
        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
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
            SetState(UnitState.Attack, 1f);
            targetFollowUnit.isAttack = true;
        }
        else
        {
            StateIdle();
        }
    }

    private void StateIdle()
    {
        SetState(UnitState.Idle, 1f);
        targetFollowUnit.isAttack = false;
    }
}

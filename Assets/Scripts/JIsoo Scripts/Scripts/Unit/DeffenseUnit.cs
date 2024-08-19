using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeffenseUnit : Unit, IAttackable
{
    RangedUnit rangedUnit;
    TargetFollowUnit targetFollowUnit;
    public bool isTower;


    private void Awake()
    {
        if (photonView.IsMine)
        {
            anim = GetComponent<Animator>();
            rangedUnit = GetComponent<RangedUnit>();
            targetFollowUnit = GetComponent<TargetFollowUnit>();
        }
    }

    private void Start()
    {
        if (!isTower)
        {
            InitStateMachine();
        }
    }

    public void InitData(float range, int damage)
    {
        this.range = range;
        this.damage = damage;

        if (rangedUnit != null)
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

        if (stateMachine != null && targetFollowUnit.target != null)
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
                photonView.RPC("RotateTowardsTarget", RpcTarget.All, targetFollowUnit.target.position);
            }
            else
            {
                targetFollowUnit.isAttack = false;

                StateIdle();
            }
        }
    }

    [PunRPC]
    private void RotateTowardsTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
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

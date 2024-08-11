using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviourPunCallbacks, ICard, IAttackable
{
    [HideInInspector] public Animator anim;

    #region public º¯¼ö
    public int cardLevel { get; set; }
    public int currentCardCount { get; set; }
    public int maxCardCount { get; set; }
    public int damage { get; set; }
    public float range { get; set; }
    public int HP { get; set; }
    public int maxHP { get; set; }
    public int cost { get; set; }
    public float spawnTime { get; set; }
    public float attackSpeed { get; set; }

    public string attackTarget { get; set; }

    protected bool isPrince;

    #endregion


    protected enum UnitState
    {
        Idle,
        Move,
        Attack,
    }


    protected Dictionary<UnitState, IState<Unit>> dicState = new Dictionary<UnitState, IState<Unit>>();
    protected StateMachine<Unit> stateMachine;

    public UnitCanvasInfo canvasInfo;

    protected virtual void InitStateMachine()
    {
        isPrince = canvasInfo.isPrince;
        IState<Unit> idle = new UnitIdle();
        IState<Unit> attack = new UnitAttack();

        dicState.Add(UnitState.Idle, idle);
        dicState.Add(UnitState.Attack, attack);

        stateMachine = new StateMachine<Unit>(this, dicState[UnitState.Idle], attackSpeed, isPrince);
    }

    protected virtual void InitializeUnitData(AllCardData unit)
    {
        attackSpeed = 1f;
        if (unit is UnitInfoData unitInfo)
        {
            maxHP = unitInfo.hp;
            attackTarget = unitInfo.target;
            attackSpeed = unitInfo.attackSpeed;
        }
        else if (unit is DEFENSETOWERInfoData defenseTowerInfo)
        {
            maxHP = defenseTowerInfo.hp;
            attackTarget = defenseTowerInfo.target;
        }
        HP = maxHP;
        damage = unit.damage;
        range = unit.range;
    }


    protected void SetState(UnitState state, float speed)
    {
        if (dicState.ContainsKey(state))
        {
            stateMachine.SetState(dicState[state], speed, isPrince);
        }
    }

    public void Attack()
    {
        if(range >= 14)
        {
            GetComponent<RangedUnit>().Attack();
        }
    }

    public void SendDamage(int damage)
    {
        if (range >= 14)
        {
            GetComponent<RangedUnit>().damage = damage;
            return;
        }
        GetComponentInChildren<Damaging>().damage = damage;
    }
}

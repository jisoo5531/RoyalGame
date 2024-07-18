using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ICard, IAttackable, IDamagable
{

    public int unit_ID;
    [HideInInspector] public Animator anim;

    #region º¯¼ö

    public float detectionRange;
    //public float coolTime;    

    public string name { get; set; }
    public int cardLevel { get; set; }
    public int currentCardCount { get; set; }
    public int maxCardCount { get; set; }
    public int damage { get; set; }
    public float range { get; set; }
    public int HP { get; set; }
    public int maxHP { get; set; }
    public int cost { get; set; }
    public float spawnTime { get; set; }
    public float attackSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public AttackTarget attackTarget { get; set; }
        
    #endregion

    protected enum UnitState
    {
        Idle,
        Move,
        Attack,
    }


    protected Dictionary<UnitState, IState<Unit>> dicState = new Dictionary<UnitState, IState<Unit>>();
    protected StateMachine<Unit> stateMachine;

    protected virtual void InitStateMachine()
    {
        IState<Unit> idle = new UnitIdle();
        IState<Unit> attack = new UnitAttack();

        dicState.Add(UnitState.Idle, idle);
        dicState.Add(UnitState.Attack, attack);

        stateMachine = new StateMachine<Unit>(this, dicState[UnitState.Idle]);
    }

    protected virtual void InitializeUnitData(UnitData_SO unit)
    {
        name = unit.unitName;
        HP = unit.HP;
        maxHP = unit.maxHp;
        damage = unit.damage;
        range = unit.range;
        detectionRange = unit.detectionRange;
        attackTarget = unit.attackTarget;
    }


    protected void SetState(UnitState state)
    {
        if (dicState.ContainsKey(state))
        {
            stateMachine.SetState(dicState[state]);
        }
    }

    public void SendDamage(int damage)
    {
        GetComponentInChildren<Damaging>().damage = damage;
    }

    public void GetDamage(int damage)
    {
        HP -= damage;

        // TODO : À¯´ÖÀÌ Á×À» ¶§
        if (HP <= 0)
        {
            Death();
        }
    }    
    private void Death()
    {
        Destroy(gameObject);
    }
}

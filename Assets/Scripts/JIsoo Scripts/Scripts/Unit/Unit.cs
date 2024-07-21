using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ICard, IAttackable, IDamagable
{

    public int unit_ID;
    [HideInInspector] public Animator anim;

    #region 변수

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

    protected virtual void InitializeUnitData(UnitData unit)
    {                
        name = unit.unitInfo.unitName;
        HP = unit.unitInfo.unitStat.HP;
        maxHP = unit.unitInfo.unitStat.maxHp;
        damage = unit.unitInfo.unitStat.damage;
        range = unit.unitInfo.unitRange.range;
        detectionRange = unit.unitInfo.unitRange.detectionRange;
        attackTarget = unit.unitInfo.attackTarget;
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
        //GetComponentInChildren<Damaging>().damage = damage;
    }

    public void GetDamage(int damage)
    {
        Debug.Log($"{gameObject.name} 맞았다");
        HP -= damage;

        // 유닛이 죽을 때
        if (HP <= 0)
        {            
            Death();
        }
    }    
    private void Death()
    {        
        GameObject effect = Instantiate(EffectManager.m_Instance.deathEffect, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
        effect.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}

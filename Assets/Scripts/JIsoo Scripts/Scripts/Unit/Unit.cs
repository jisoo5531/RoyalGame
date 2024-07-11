using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Damagable
{
    public int unit_ID;

    protected enum UnitState
    {
        Idle,
        Move,
        Attack,
    }

    protected Dictionary<UnitState, IState<Unit>> dicState = new Dictionary<UnitState, IState<Unit>>();
    protected StateMachine<Unit> stateMachine;

    [HideInInspector] public Animator anim;

    private void Awake()
    {        
        anim = GetComponent<Animator>();

        // TODO : 테스트용 Enemy 태그 
        //targetTransform = GameObject.FindWithTag("Enemy").transform;

        InitializeUnitData(UnitSpawner.instance.selectedUnit);
    }

    protected virtual void Start()
    {
        Debug.Log("베이스 스타트");
        IState<Unit> idle = new UnitIdle();
        IState<Unit> attack = new UnitAttack();

        dicState.Add(UnitState.Idle, idle);
        dicState.Add(UnitState.Attack, attack);

        stateMachine = new StateMachine<Unit>(this, dicState[UnitState.Idle]);
        //GetComponentInChildren<Weapon>().damage = damage;
    }
    private void InitializeUnitData(UnitData_SO unit)
    {        
        name = unit.unitName;
        HP = unit.HP;
        maxHp = unit.maxHp;
        damage = unit.damage;
        moveSpeed = unit.moveSpeed;
        detectionRange = unit.range;
        //coolTime = unit.spawnTime;
    }

    private void Update()
    {
        StateTransition();
        stateMachine.DoOperateUpdate();
    }
    protected virtual void StateTransition()
    {
        // 파생 클래스에서 상태 전이를 정의
    }

    protected void SetState(UnitState state)
    {
        if (dicState.ContainsKey(state))
        {
            stateMachine.SetState(dicState[state]);
        }
    }
}

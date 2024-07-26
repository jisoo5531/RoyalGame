using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviourPunCallbacks, ICard, IAttackable, IDamagable
{


    public int unit_ID;
    [HideInInspector] public Animator anim;
    GameObject effect;

    #region 변수

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
    public float attackSpeed { get; set; }

    public string attackTarget { get; set; }

    public Range rangeType;


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

        stateMachine = new StateMachine<Unit>(this, dicState[UnitState.Idle], attackSpeed);
    }

    protected virtual void InitializeUnitData(AllCardData unit)
    {
        name = unit.cardName;
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
            stateMachine.SetState(dicState[state], speed);
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
            Debug.Log("원거리?");
            print(GetComponent<RangedUnit>() == null);
            GetComponent<RangedUnit>().damage = damage;
            return;
        }
        GetComponentInChildren<Damaging>().damage = damage;
    }

    [PunRPC]
    private void RPC_damage(int damage)
    {
        this.HP -= damage;
    }

    public void GetDamage(int damage)
    {
        print(damage+",  "+HP);
        Debug.Log($"{gameObject.name} 맞았다");
        Debug.Log("hp: " + damage + ",  " + HP);
        photonView.RPC("RPC_damage", RpcTarget.All, damage);
        // 유닛이 죽을 때
        if (HP <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        string effectStr = EffectManager.instance.deathEffect.name;
        effect = PhotonNetwork.Instantiate(effectStr, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
        effect.transform.localScale = transform.localScale;
        //photonView.RPC("UnitDestroy", RpcTarget.All, transform.localScale);
        Destroy(effect, 1f);
        Destroy(gameObject);
        IsMineManager isMIne = GameManager.instance.isMineManager;
        isMIne.RemoveUnit(GetComponent<PhotonView>().ViewID);
    }

    //[PunRPC]
    //private void UnitDestroy(Vector3 trans)
    //{
    //    effect.transform.localScale = trans;
    //    Destroy(effect, 1f);
    //    Destroy(gameObject);
    //}
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviourPunCallbacks
{
    public int HP { get; set; }
    public int maxHP { get; set; }

    public float range { get; set; }

    public int damage { get; set; }

    public bool isNotOnCannon { get; set; }

    public TargetFollowUnit targetFollowUnit;
    public UnitCanvasInfo canvasInfo;
    [HideInInspector] public Animator anim;


    protected enum TowerState
    {
        Idle,
        Attack
    }


    protected Dictionary<TowerState, IState<Tower>> dicState = new Dictionary<TowerState, IState<Tower>>();
    protected StateMachine<Tower> stateMachine;

    public GameObject onTopUnit;

    protected virtual void InitData() {}

    //protected virtual void InitStateMachine()
    //{
    //    IState<TowerState> idle = new UnitIdle();
    //    IState<TowerState> attack = new UnitAttack();

    //    dicState.Add(TowerState.Idle, idle);
    //    dicState.Add(TowerState.Attack, attack);

    //    stateMachine = new StateMachine<TowerState>(this, dicState[TowerState.Idle], attackSpeed, isPrince);
    //}


    //protected void SetState(TowerState state, float speed)
    //{
    //    if (dicState.ContainsKey(state))
    //    {
    //        stateMachine.SetState(dicState[state], speed, isPrince);
    //    }
    //}
}

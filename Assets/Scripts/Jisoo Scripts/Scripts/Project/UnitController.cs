//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////[System.Serializable]
////public class UnitInfo
////{
////    public string name;
////    public int HP;
////    public int maxHp;
////    public float damage;
////    public float moveSpeed;
////    public float detectionRange;
////    public float coolTime;
////    public Transform targetTransform;
////}

//public class UnitController : MonoBehaviour
//{
//    public enum UnitState
//    {
//        Move,
//        Attack,
//    }

//    //public UnitInfo unitInfo;

//    private Dictionary<UnitState, IState<UnitController>> dicState = new Dictionary<UnitState, IState<UnitController>>();
//    private StateMachine<UnitController> stateMachine;

//    [HideInInspector] public Animator anim;

//    protected virtual void Awake()
//    {
//        anim = GetComponent<Animator>();
//    }

//    private void Start()
//    {
//        //IState<UnitController> move = new UnitMove();
//        //IState<UnitController> attack = new UnitAttack();
        
//        //dicState.Add(UnitState.Move, move);
//        //dicState.Add(UnitState.Attack, attack);

//        stateMachine = new StateMachine<UnitController>(this, dicState[UnitState.Move]);

//        GetComponentInChildren<Weapon>().damage = unitInfo.damage;
//    }

//    private void Update()
//    {        
//        if (unitInfo.targetTransform)
//        {
//            float distance = Vector3.Distance(unitInfo.targetTransform.position, transform.position);
//            if (distance < unitInfo.detectionRange)
//            {
//                stateMachine.SetState(dicState[UnitState.Attack]);
//            }
//            else
//            {
//                stateMachine.SetState(dicState[UnitState.Move]);
//            }
//        }
//        else
//        {
//            stateMachine.SetState(dicState[UnitState.Move]);
//        }

        
//        stateMachine.DoOperateUpdate();
//    }
//}

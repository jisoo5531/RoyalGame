using Org.BouncyCastle.Asn1;
using Photon.Pun;
using UnityEngine;

public class UnitAttack : IState<Unit>
{
    private Unit unit;
    private bool isAttack = false;
    private bool isCoolTime = false;
    private float attackSpeed = 0f;

    public void OperateEnter(Unit sender, float speed)
    {
        unit = sender;
        unit.anim.SetBool("isAttack", true);
        //attackSpeed = speed;
        //unit.anim.speed = speed;
        //UpdateAnimationSpeed(speed);
        unit.Attack();
    }
    public void OperateExit(Unit sender)
    {
       // unit.anim.speed = 1;
        unit.anim.SetBool("isAttack", false);
        //if(unit is MovableUnit moveUnit)
        //{
        //    moveUnit.UpdateAnimationSpeed(1);
        //}
        //unit.UpdateAnimationSpeed(1); // 기본 속도로 리셋
        //UpdateAnimationSpeed(1);
    }
    public void OperateUpdate(Unit sender)
    {
        if (unit.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if(!isAttack)
            {
                unit.Attack();
                isAttack = true;
            }
           // isCoolTime = false;
        }
        else
        {
            //if(!isCoolTime)
            //{
            //    if (unit is MovableUnit moveUnit)
            //    {
            //        moveUnit.UpdateAnimationSpeed(1);
            //    }
            //    //unit.UpdateAnimationSpeed(attackSpeed);
            //    isCoolTime = true;
            //}
            isAttack = false;
        }
    }

    //private void UpdateAnimationSpeed(float speed)
    //{
    //    unit.anim.speed = speed;
    //    unit.photonView.RPC("SyncAnimationSpeed", RpcTarget.Others, speed);
    //}

    //[PunRPC]
    //public void SyncAnimationSpeed(float speed)
    //{
    //    unit.anim.speed = speed;
    //}
}

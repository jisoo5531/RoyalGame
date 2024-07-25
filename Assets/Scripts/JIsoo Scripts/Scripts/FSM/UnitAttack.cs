using Org.BouncyCastle.Asn1;
using UnityEngine;

public class UnitAttack : IState<Unit>
{
    private Unit unit;
    private bool isAttack = false;

    public void OperateEnter(Unit sender, float speed)
    {
        unit = sender;
        unit.anim.SetBool("isAttack", true);
        unit.anim.speed = speed;
        unit.Attack();
    }
    public void OperateExit(Unit sender, float speed)
    {
        unit.anim.speed = speed;
        unit.anim.SetBool("isAttack", false);
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
        }
        else
        {
            isAttack = false;
        }
    }
}

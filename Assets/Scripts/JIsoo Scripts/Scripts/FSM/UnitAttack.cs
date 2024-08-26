using Org.BouncyCastle.Asn1;
using Photon.Pun;
using UnityEngine;

public class UnitAttack : IState<Unit>
{
    private Unit unit;
    private bool isAttack = false;
    private bool isCoolTime = false;
    private float attackSpeed = 0f;
    private float attackDelay = 0.2f;

    public void OperateEnter(Unit sender, float speed, bool isPrince)
    {
        unit = sender;
        unit.anim.SetBool("isAttack", true);
    }
    public void OperateExit(Unit sender)
    {
        unit.anim.SetBool("isAttack", false);
    }
    public void OperateUpdate(Unit sender, bool isRun)
    {
        AnimatorStateInfo stateInfo = unit.anim.GetCurrentAnimatorStateInfo(0);

        if (unit.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if(stateInfo.normalizedTime >= attackDelay && !isAttack)
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

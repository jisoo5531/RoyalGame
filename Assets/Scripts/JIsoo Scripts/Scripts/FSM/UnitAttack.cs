using UnityEngine;

public class UnitAttack : IState<Unit>
{
    private Unit unit;

    public void OperateEnter(Unit sender)
    {
        Debug.Log("공격");
        unit = sender;
        unit.anim.SetTrigger("DoAttack");
    }
    public void OperateExit(Unit sender)
    {
    }
    public void OperateUpdate(Unit sender)
    {
        Debug.Log("공격 중");
        if (false == unit.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            unit.anim.SetTrigger("DoAttack");
        }
    }
}

using UnityEngine;

public class UnitAttack : IState<Unit>
{
    private Unit unit;

    public void OperateEnter(Unit sender)
    {
        unit = sender;
        unit.anim.SetTrigger("DoAttack");
    }
    public void OperateExit(Unit sender)
    {
    }
    public void OperateUpdate(Unit sender)
    {
        if (false == unit.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            unit.anim.SetTrigger("DoAttack");
        }
    }
}

using UnityEngine;

public class UnitMove : IState<Unit>
{
    private Unit unit;    

    public void OperateEnter(Unit sender, float speed)
    {
        unit = sender;
        unit.anim.speed = speed;
        unit.anim.SetBool("isMove", true);

    }
    public void OperateExit(Unit sender)
    {
        unit.anim.speed = 1;
        unit.anim.SetBool("isMove", false);
        
    }
    public void OperateUpdate(Unit sender)
    {
    }
}

using UnityEngine;

public class UnitMove : IState<Unit>
{
    private Unit unit;
    private MovableUnit movableUnit;

    public void OperateEnter(Unit sender)
    {
        unit = sender;
        unit.anim.SetBool("isMove", true);

        if (sender is MovableUnit)
        {
            movableUnit = sender as MovableUnit;
        }
    }
    public void OperateExit(Unit sender)
    {
        unit.anim.SetBool("isMove", false);
        
    }
    public void OperateUpdate(Unit sender)
    {
    }
}

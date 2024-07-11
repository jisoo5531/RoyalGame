using UnityEngine;

public class UnitMove : IState<Unit>
{
    private Unit unit;
    public void OperateEnter(Unit sender)
    {
        unit = sender;
        unit.anim.SetBool("isMove", true);
    }
    public void OperateExit(Unit sender)
    {
        unit.anim.SetBool("isMove", false);
    }
    public void OperateUpdate(Unit sender)
    {
        if (unit)
        {            
            unit.transform.Translate(Vector3.forward * unit.moveSpeed * Time.deltaTime);
        }
    }
}

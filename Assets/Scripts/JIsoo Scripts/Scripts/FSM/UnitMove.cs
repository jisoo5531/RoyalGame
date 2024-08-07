using UnityEngine;

public class UnitMove : IState<Unit>
{
    private Unit unit;
    private float runDelay = 2f;
    private float currentTime = 0f;
    private bool isPrince;

    public void OperateEnter(Unit sender, float speed, bool isPrince)
    {
        unit = sender;
        unit.anim.speed = speed;
        unit.anim.SetBool("isMove", true);
        this.isPrince = isPrince;

    }
    public void OperateExit(Unit sender)
    {
        unit.anim.speed = 1;
        unit.anim.SetBool("isMove", false);

        if (isPrince)
        {
            unit.anim.SetBool("isRun", false);
        }

    }
    public void OperateUpdate(Unit sender, bool isRun)
    {
        if (isPrince)
        {
            unit.anim.speed = 1;
            if (isRun)
            {
                unit.anim.SetBool("isRun", true);
            }
            else
            {
                unit.anim.SetBool("isRun", false);
            }
        }
    }
}

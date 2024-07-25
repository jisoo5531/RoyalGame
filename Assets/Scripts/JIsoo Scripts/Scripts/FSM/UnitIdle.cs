using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : IState<Unit>
{
    private Unit unit;

    public void OperateEnter(Unit sender)
    {
        unit = sender;
    }
    public void OperateExit(Unit sender)
    {        
    }
    public void OperateUpdate(Unit sender)
    {
        Debug.Log("Idle ป๓ลย");
    }
}

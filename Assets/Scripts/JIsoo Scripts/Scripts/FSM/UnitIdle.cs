using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : IState<Unit>
{
    private Unit unit;

    public void OperateEnter(Unit sender, float speed)
    {
        unit = sender;
    }
    public void OperateExit(Unit sender, float speed)
    {        
    }
    public void OperateUpdate(Unit sender)
    {

    }
}

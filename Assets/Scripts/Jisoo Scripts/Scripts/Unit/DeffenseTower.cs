using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDefenseTower : Unit
{
    public DeffenseTower defense = new DeffenseTower();

    private void Awake()
    {        
    }
}
public class DeffenseTower : Unit /*IAttackable, IDamagable*/
{
    protected float lifeTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        //InitializeUnitData(UnitSpawner.instance.selectedUnit);
    }
    private void Start()
    {
        InitStateMachine();
    }

    protected override void InitializeUnitData(AllCardData unit)
    {
        base.InitializeUnitData(unit);
        if (unit is DEFENSETOWERInfoData defenseTowerInfo)
        {
            lifeTime = defenseTowerInfo.lifeTime;
        }
    }

    protected override void InitStateMachine()
    {
        base.InitStateMachine();
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            UpdateLifeTime();
        }
    }


    public void UpdateLifeTime()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }    
}

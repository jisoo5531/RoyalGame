using UnityEngine;

[System.Serializable]
public class CardInfo
{
    public int unit_ID;
    public int unit_Level;
    public int unit_CurrentCardCount;
    public int unit_MaxCardCount;
    public int user_ID;
}

[System.Serializable]
public class UnitInfo
{    
    public string unitName;
    public int unitCount;
    public Type type;
    
    public Stat unitStat;
    public Range unitRange;

    public string unit_Desc;

    public UnitInfo()
    {
        unitStat = new Stat();
        unitRange = new Range();
    }
}

[System.Serializable]
public class Stat
{
    public int maxHp;
    public int HP;
    public int damage;
    public float attackSpeed;
    public float moveSpeed;

    [Space(20)]
    public int cost;
    public int lifeTime;
    public float spawnTime;
}

[System.Serializable]
public class Range
{
    public float range;
    public float detectionRange;
}

[System.Serializable]
public class UnitData
{
    public CardInfo cardInfo;
    public UnitInfo unitInfo;
    
    public AttackTarget attackTarget;
    public Grade grade;
    
    public Sprite iconSprite;
    public GameObject prefab;

    // 복제 생성자
    public UnitData(UnitData_SO unitDataSO)
    {
        cardInfo = new CardInfo();
        unitInfo = new UnitInfo();

        cardInfo.unit_ID = unitDataSO.unit_ID;
        cardInfo.unit_Level = unitDataSO.unit_Level;
        cardInfo.unit_CurrentCardCount = unitDataSO.unit_CurrentCardCount;
        cardInfo.unit_MaxCardCount = unitDataSO.unit_MaxCardCount;
        cardInfo.user_ID = unitDataSO.user_ID;

        unitInfo.unitName = unitDataSO.unitName;
        unitInfo.type = unitDataSO.type;
        unitInfo.unitCount = unitDataSO.unitCount;

        unitInfo.unitStat.damage = unitDataSO.damage;
        unitInfo.unitStat.HP = unitDataSO.HP;
        unitInfo.unitStat.maxHp = unitDataSO.maxHp;
        unitInfo.unitStat.attackSpeed = unitDataSO.attackSpeed;
        unitInfo.unitStat.moveSpeed = unitDataSO.moveSpeed;
        unitInfo.unitStat.cost = unitDataSO.cost;
        unitInfo.unitStat.spawnTime = unitDataSO.spawnTime;
        unitInfo.unitStat.lifeTime = unitDataSO.lifeTime;

        unitInfo.unitRange.detectionRange = unitDataSO.detectionRange;
        unitInfo.unitRange.range = unitDataSO.range;
        unitInfo.unit_Desc = unitDataSO.unit_Desc;
        attackTarget = unitDataSO.attackTarget;
        grade = unitDataSO.grade;
        iconSprite = unitDataSO.iconSprite;
        prefab = unitDataSO.prefab;
    }

    public int Get_Upgrade_HP()
    {
        return Mathf.RoundToInt((float)unitInfo.unitStat.maxHp * 0.2f);
    }

    public int Get_Upgrade_Damage()
    {
        return Mathf.RoundToInt((float)unitInfo.unitStat.damage * 0.2f);
    }

    public void UpgradeToStat()
    {
        unitInfo.unitStat.maxHp += Mathf.RoundToInt((float)unitInfo.unitStat.maxHp * 0.2f);
        unitInfo.unitStat.damage += Mathf.RoundToInt((float)unitInfo.unitStat.damage * 0.3f);
    }
}

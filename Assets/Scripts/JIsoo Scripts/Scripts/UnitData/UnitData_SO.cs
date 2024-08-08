using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData_SO : ScriptableObject
{
    public int unit_ID;
    public int unit_Level;
    public int unit_CurrentCardCount;
    public int unit_MaxCardCount;
    public int user_ID;    

    [Space(20)]

    public string unitName;
    public int damage;
    public int HP;
    public int maxHp;
    public float attackSpeed;
    public float moveSpeed;

    /// <summary>
    /// 몇 마리 생성되는지
    /// </summary>
    [Tooltip("몇 마리 생성할건지")] public int unitCount;

    public Type type;
    
    [Space(20)]
    public Range rangeType;
    public int rangedSpeed;
    [Tooltip("사정거리")]
    public float range;
    [Tooltip("탐지거리")]
    public float detectionRange;

    [Space(20)]

    /// <summary>
    /// 엘릭서 비용
    /// </summary>
    public int cost;

    /// <summary>
    /// 생성 소요 시간
    /// </summary>
    public float spawnTime;

    /// <summary>
    /// 유닛 설명
    /// </summary>
    [Tooltip("유닛 설명")] public string unit_Desc;

    /// <summary>
    /// 방어타워 수명
    /// </summary>
    [Tooltip("방어 타워 수명")] public int lifeTime;

    public AttackTarget attackTarget;

    public Grade grade;

    public Sprite iconSprite;
    public GameObject prefab;

    

    public int Get_Upgrade_HP()
    {
        return Mathf.RoundToInt((float)maxHp * 0.2f);
    }    
    
    public int Get_Upgrade_Damage()
    {
        return Mathf.RoundToInt((float)damage * 0.2f);
    }

    /// <summary>
    /// 레벨 올라갈 때마다 스탯 증가
    /// </summary>
    public void UpgradeToStat()
    {
        maxHp += Mathf.RoundToInt((float)maxHp * 0.2f);
        damage += Mathf.RoundToInt((float)damage * 0.3f);
    }
}

public enum Type
{
    Unit,
    Deffense,
    Magic
}

public enum Grade
{
    Normal,
    Rare,
    Epic
}

public enum AttackTarget
{
    All,
    Tower
}
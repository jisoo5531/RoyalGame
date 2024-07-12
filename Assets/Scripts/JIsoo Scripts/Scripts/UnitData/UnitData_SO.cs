using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData_SO : ScriptableObject
{
    public int unit_ID;
    public int unit_Level;
    public int unit_CurrentCardCount;
    public int unit_MaxCardCount;
    public int user_ID;

    public string unitName;
    public int damage;
    public int HP;
    public int maxHp;
    public int attackSpeed;
    public int moveSpeed;

    // TODO : 공격 대상 추가

    public int range;

    /// <summary>
    /// 엘릭서 비용
    /// </summary>
    public int cost;

    /// <summary>
    /// 생성 소요 시간
    /// </summary>
    public int spawnTime;

    /// <summary>
    /// 유닛 설명
    /// </summary>
    public string unit_Desc;

    public Grade grade;

    public Sprite iconSprite;
    public GameObject prefab;
}

public enum Grade
{
    Normal,
    Rare,
    Epic
}
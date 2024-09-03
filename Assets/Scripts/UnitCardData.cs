using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCardData
{
    public int cardId;
    public string cardName;
    public int cost;
    public int level;
    public int damage;
    public string grade;
    public float range;
    public Sprite img;
    public string type;
    public int currentCardCount;
    public int maxCardCount;
    public string desc;
    public GameObject prefab;
}

public class UnitInfoData : AllCardData
{
    public float attackSpeed;
    public int moveSpeed;
    public int hp;
    public float spawnTime;
    public float detectRange;
    public string target;

    public UnitInfoData(int cardId, string cardName, int cost, string grade, float range, float detectRange,
                        int level, int damage, string type, float attackSpeed, int moveSpeed,
                        int hp, int spawnTime, string target, Sprite img, GameObject prefab)
    {
        this.cardId = cardId;
        this.cardName = cardName;
        this.cost = cost;
        this.grade = grade;
        this.range = range;
        this.detectRange = detectRange;
        this.level = level;
        this.damage = damage;
        this.type = type;
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
        this.hp = hp;
        this.spawnTime = spawnTime;
        this.target = target;
        this.img = img;
        this.prefab = prefab;
    }

    public UnitInfoData(int cardId, string cardName, int cost, string grade, float range, float detectRange,
                        int level, int damage, string type, float attackSpeed, int moveSpeed,
                        int hp, int spawnTime, string desc, string target, int currentCardCount, int maxCardCount, Sprite img)
    {
        this.cardId = cardId;
        this.cardName = cardName;
        this.cost = cost;
        this.grade = grade;
        this.range = range;
        this.detectRange = detectRange;
        this.level = level;
        this.damage = damage;
        this.type = type;
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
        this.hp = hp;
        this.spawnTime = spawnTime;
        this.desc = desc;
        this.target = target;
        this.currentCardCount = currentCardCount;
        this.maxCardCount = maxCardCount;
        this.img = img;
    }
}

public class DEFENSETOWERInfoData : AllCardData
{
    public float attackSpeed;
    public int hp;
    public float spawnTime;
    public int lifeTime;
    public string target;
}

public class MAGICInfoData : AllCardData
{
    public int tower_Damage;
}

public enum Range
{
    Melee,
    Ranged
}   
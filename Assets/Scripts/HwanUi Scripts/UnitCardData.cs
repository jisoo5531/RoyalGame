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
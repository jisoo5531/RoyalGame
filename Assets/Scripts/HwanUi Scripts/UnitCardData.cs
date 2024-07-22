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
    public GameObject prefab;
}

public class UnitInfoData : AllCardData
{
    public int moveSpeed;
    public int hp;
    public float spawnTime;
    public float detectRange;
}

public class DEFENSETOWERInfoData : AllCardData
{
    public float attackSpeed;
    public int hp;
    public float spawnTime;
    public float detectRange;
    public int lifeTime;
}

public class MAGICInfoData : AllCardData
{
    public int tower_Damage;
}
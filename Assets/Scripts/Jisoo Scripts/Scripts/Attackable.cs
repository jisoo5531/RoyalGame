using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attackable : MonoBehaviour, ICard
{
    public string name { get; set; }
    public int cardLevel { get; set; }
    public int currentCardCount { get; set; }
    public int maxCardCount { get; set; }
    public int damage { get; set; }
    public float range { get; set; }

    /// <summary>
    /// 유닛마다 각자의 무기 또는 스킬의 데미지 저장
    /// </summary>
    /// <param name="damage"></param>
    public abstract void Damage(int damage);
}

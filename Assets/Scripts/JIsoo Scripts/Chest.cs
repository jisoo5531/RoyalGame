using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    private enum Grade
    {
        Normal,
        Rare,
        Epic
    }

    private enum Normal
    {
        Knight = 1,
        Archer = 2,
        Fireball = 3
    }

    private enum Rare
    {
        Giant = 4,
        Mage = 5,
        Crossbow = 6,
    }

    private enum Epic
    {
        Prince = 7,
        Balista = 8
    }

    List<int> normalCards;
    List<int> rareCards;
    List<int> epicCards;

    public Sprite characterSprite { get; set; }
    public string CharacterName { get; set; }
    public string CharacterGrade { get; set; }
    public int CharacterLevel { get; set; }
    public int CharacterCurrentCardCount { get; set; }
    public int CharacterMaxCardCount { get; set; }



    public Image characterImg;
    public TMP_Text characterName;
    public TMP_Text characterGrade;
    public TMP_Text characterLevel;
    public TMP_Text characterCurrentCardCount;
    public TMP_Text characterMaxCardCount;

    int normalRemainCard = 3;
    int rareRemainCard = 3;
    int epicRemainCard = 2;

    /// <summary>
    /// 상자에서 얻은 보상을 담은 딕셔너리.<br/>
    /// <b>Key:</b> 카드 ID - 각 유닛 타입을 나타내는 값.<br/>
    /// <b>Value:</b> 튜플로 구성되어 있으며,<br/>
    /// 첫 번째 항목은 카드의 등급, Normal, Rare, Epic 중 하나.<br/>
    /// 두 번째 항목은 해당 카드가 보상으로 몇 번 나왔는지를 나타내는 개수.<br/>
    /// </summary>
    private Dictionary<int, (Grade, int)> reward = new Dictionary<int, (Grade, int)>();


    private void Awake()
    {
        //OpenEpicChest();
        //PrintRewards();
    }

    public void OpenNormalChest()
    {
        int normalRemainCard = 3;
        int rareRemainCard = 1;

        while (normalRemainCard > 0)
        {
            NormalCard();
            normalRemainCard--;
        }

        while (rareRemainCard > 0)
        {
            RareCard();
            rareRemainCard--;
        }
    }

    public void OpenRareChest()
    {
        int normalRemainCard = 5;
        int rareRemainCard = 3;

        while (normalRemainCard > 0)
        {
            NormalCard();
            normalRemainCard--;
        }

        while (rareRemainCard > 0)
        {
            RareCard();
            rareRemainCard--;
        }
    }

    public void OpenEpicChest()
    {

        normalCards = System.Enum.GetValues(typeof(Normal)).Cast<int>().ToList();
        rareCards = System.Enum.GetValues(typeof(Rare)).Cast<int>().ToList();
        epicCards = System.Enum.GetValues(typeof(Epic)).Cast<int>().ToList();
            
        while (normalRemainCard > 0)
        {
            NormalCard();
            normalRemainCard--;
        }

        while (rareRemainCard > 0)
        {
            RareCard();
            rareRemainCard--;
        }

        while (epicRemainCard > 0)
        {
            EpicCard();
            epicRemainCard--;
        }
    }

    private void NormalCard()
    {
        int randomIndex = Random.Range(0, normalCards.Count);
        AddReward(normalCards[randomIndex], Grade.Normal);
        normalCards.RemoveAt(randomIndex);
    }

    private void RareCard()
    {
        int randomIndex = Random.Range(0, rareCards.Count);
        AddReward(rareCards[randomIndex], Grade.Rare);
        rareCards.RemoveAt(randomIndex);
    }

    private void EpicCard()
    {
        int randomIndex = Random.Range(0, epicCards.Count);
        AddReward(epicCards[randomIndex], Grade.Epic);
        epicCards.RemoveAt(randomIndex);
    }

    private void AddReward(int cardID, Grade grade)
    {
        if (reward.ContainsKey(cardID))
        {
            reward[cardID] = (grade, reward[cardID].Item2 + 1);
        }
        else
        {
            reward[cardID] = (grade, 1);
        }
    }

    private void PrintRewards()
    {
        foreach (var item in reward)
        {
            Debug.Log($"Grade: {item.Value.Item1}, Card ID: {item.Key}, Count: {item.Value.Item2}");
        }
    }
}

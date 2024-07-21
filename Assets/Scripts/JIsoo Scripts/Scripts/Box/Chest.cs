using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        Knight = 0,
        Archer = 1,
        Giant = 5
    }

    private enum Rare
    {
        Mage = 6,
        Crossbow = 3,
        Fireball = 4
    }

    private enum Epic
    {
        Balista = 2,
        Prince = 7
    }

    /// <summary>
    /// 상자에서 얻은 보상을 담은 딕셔너리.<br/>
    /// <b>Key:</b> 카드 ID - 각 유닛 타입을 나타내는 값.<br/>
    /// <b>Value:</b> 튜플로 구성되어 있으며,<br/>
    /// 첫 번째 항목은 카드의 등급, Normal, Rare, Epic 중 하나.<br/>
    /// 두 번째 항목은 해당 카드가 보상으로 몇 번 나왔는지를 나타내는 개수.<br/>
    /// </summary>
    private Dictionary<int, (Grade, int)> reward = new Dictionary<int, (Grade, int)>();

    public int totalRemainCard;    

    //private void Awake()
    //{
    //    OpenEpicChest();
    //    PrintRewards();
    //}

    public void OpenNormalChest()
    {
        int normalRemainCard = 3;
        int rareRemainCard = 1;
        totalRemainCard = normalRemainCard + rareRemainCard;

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
        PrintRewards();
    }

    public void OpenRareChest()
    {
        int normalRemainCard = 5;
        int rareRemainCard = 3;
        totalRemainCard = normalRemainCard + rareRemainCard;

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
        int normalRemainCard = 6;
        int rareRemainCard = 4;
        int epicRemainCard = 2;
        totalRemainCard = normalRemainCard + rareRemainCard + epicRemainCard;

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
        List<int> normalCards = System.Enum.GetValues(typeof(Normal)).Cast<int>().ToList();
        int randomIndex = Random.Range(0, normalCards.Count);
        AddReward(normalCards[randomIndex], Grade.Normal);
    }

    private void RareCard()
    {
        List<int> rareCards = System.Enum.GetValues(typeof(Rare)).Cast<int>().ToList();
        int randomIndex = Random.Range(0, rareCards.Count);
        AddReward(rareCards[randomIndex], Grade.Rare);
    }

    private void EpicCard()
    {
        List<int> epicCards = System.Enum.GetValues(typeof(Epic)).Cast<int>().ToList();
        int randomIndex = Random.Range(0, epicCards.Count);
        AddReward(epicCards[randomIndex], Grade.Epic);
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

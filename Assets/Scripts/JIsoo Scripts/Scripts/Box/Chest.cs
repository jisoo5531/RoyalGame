using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public enum Grade
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




    //public Image characterImg;
    //public TMP_Text characterName;
    //public TMP_Text characterGrade;
    //public TMP_Text characterLevel;
    //public TMP_Text characterCurrentCardCount;
    //public TMP_Text characterMaxCardCount;

    /// <summary>
    /// 상자에서 얻은 보상을 담은 딕셔너리.<br/>
    /// <b>Key:</b> 카드 ID - 각 유닛 타입을 나타내는 값.<br/>
    /// <b>Value:</b> 튜플로 구성되어 있으며,<br/>
    /// 첫 번째 항목은 카드의 등급, Normal, Rare, Epic 중 하나.<br/>
    /// 두 번째 항목은 해당 카드가 보상으로 몇 번 나왔는지를 나타내는 개수.<br/>
    /// </summary>
    private Dictionary<int, (Grade, int)> reward = new Dictionary<int, (Grade, int)>();
    public Dictionary<CharacterInfo, int> randomUnits = new Dictionary<CharacterInfo, int>();

    public int totalRemainCard;

    List<int> normalCards;
    List<int> rareCards;
    List<int> epicCards;

    //private void Awake()
    //{
    //    OpenEpicChest();
    //    PrintRewards();
    //}

    public void OpenNormalChest()
    {
        int normalRemainCard = 3;
        int rareRemainCard = 1;

        while (normalRemainCard > 0)
        {
            NormalCard(5);
            normalRemainCard--;
        }

        while (rareRemainCard > 0)
        {
            RareCard(5);
            rareRemainCard--;
        }
        totalRemainCard = randomUnits.Count;
    }

    public void OpenRareChest()
    {
        int normalRemainCard = 5;
        int rareRemainCard = 3;

        while (normalRemainCard > 0)
        {
            NormalCard(5);
            normalRemainCard--;
        }

        while (rareRemainCard > 0)
        {
            RareCard(5);
            rareRemainCard--;
        }

        totalRemainCard = randomUnits.Count;
    }

    public void OpenEpicChest()
    {
        int normalRemainCard = 3;
        int rareRemainCard = 3;
        int epicRemainCard = 2;
        normalCards = System.Enum.GetValues(typeof(Normal)).Cast<int>().ToList();
        rareCards = System.Enum.GetValues(typeof(Rare)).Cast<int>().ToList();
        epicCards = System.Enum.GetValues(typeof(Epic)).Cast<int>().ToList();
        randomUnits.Clear();

        while (normalRemainCard > 0)
        {
            NormalCard(1);
            normalRemainCard--;
        }

        while (rareRemainCard > 0)
        {
            RareCard(1);
            rareRemainCard--;
        }

        while (epicRemainCard > 0)
        {
            EpicCard(1);
            epicRemainCard--;
        }
        totalRemainCard = randomUnits.Count;
    }

    private void NormalCard(int amount)
    {
        int randomIndex = Random.Range(0, normalCards.Count);
        //AddReward(normalCards[randomIndex], Grade.Normal);

        AddRandomUnitList(normalCards[randomIndex], amount);
        normalCards.RemoveAt(randomIndex);
    }

    private void RareCard(int amount)
    {
        int randomIndex = Random.Range(0, rareCards.Count);
        //AddReward(rareCards[randomIndex], Grade.Rare);
        AddRandomUnitList(rareCards[randomIndex], amount);
        rareCards.RemoveAt(randomIndex);
    }

    private void EpicCard(int amount)
    {
        int randomIndex = Random.Range(0, epicCards.Count);
        //AddReward(epicCards[randomIndex], Grade.Epic);
        AddRandomUnitList(epicCards[randomIndex], amount);
        epicCards.RemoveAt(randomIndex);
    }

    //private void AddReward(int cardID, Grade grade)
    //{
    //    if (reward.ContainsKey(cardID))
    //    {
    //        reward[cardID] = (grade, reward[cardID].Item2 + 1);
    //    }
    //    else
    //    {
    //        reward[cardID] = (grade, 1);
    //    }
    //}

    private void AddRandomUnitList(int cardID, int amount)
    {
        for (int i = 0; i < CardInfoManager.instance.allCharacters.Count; i++)
        {
            if (cardID == CardInfoManager.instance.allCharacters[i].characterID)
            {
                randomUnits.Add(CardInfoManager.instance.allCharacters[i], amount);
                break;
            }
        }
    }

    public KeyValuePair<CharacterInfo, int> GetCharInfo(int index)
    {
        List<KeyValuePair<CharacterInfo, int>> infoList = randomUnits.ToList();
        KeyValuePair<CharacterInfo, int> rewardItem = default;

        if (index < infoList.Count)
        {
            rewardItem = infoList[index];
        }
        return rewardItem;
    }

    public KeyValuePair<int, (Grade, int)>? OnClickOpenCard(int index)
    {
        // reward 딕셔너리를 리스트로 변환
        List<KeyValuePair<int, (Grade, int)>> rewardList = reward.ToList();
        Debug.Log(rewardList.Count);

        if (index >= 0 && index < rewardList.Count)
        {
            var rewardItem = rewardList[index];
            Debug.Log($"Grade: {rewardItem.Value.Item1}, Card ID: {rewardItem.Key}, Count: {rewardItem.Value.Item2}");
            totalRemainCard -= 1;
            return rewardItem;
        }
        else
        {
            Debug.Log("Invalid index.");
            return null;
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

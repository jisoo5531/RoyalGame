using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingChest : MonoBehaviour
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

    private OnClickOpenChest openChest;

    List<int> normalCards;
    List<int> rareCards;
    List<int> epicCards;


    private void Awake()
    {
        openChest = GetComponent<OnClickOpenChest>();
    }

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
            Debug.Log("1");
            NormalCard(1);
            normalRemainCard--;
        }

        while (rareRemainCard > 0)
        {
            Debug.Log("2");
            RareCard(1);
            rareRemainCard--;
        }

        while (epicRemainCard > 0)
        {
            Debug.Log("3");
            EpicCard(1);
            epicRemainCard--;
        }
        totalRemainCard = randomUnits.Count;
        if (openChest.MJ_ShakeBox != null)
        {
            openChest.MJ_ShakeBox.DOAction();
        }
        if (openChest.MJ_MoveCard != null)
        {
            openChest.MJ_MoveCard.CardActive();
        }

        openChest.characterInfo = GetCharInfo(openChest.clickCount);
        openChest.clickCount++;
        openChest.Set_UI();
    }

    private void NormalCard(int amount)
    {
        int randomIndex = Random.Range(0, normalCards.Count);

        AddRandomUnitList(normalCards[randomIndex], amount);
        normalCards.RemoveAt(randomIndex);
    }

    private void RareCard(int amount)
    {
        int randomIndex = Random.Range(0, rareCards.Count);
        AddRandomUnitList(rareCards[randomIndex], amount);
        rareCards.RemoveAt(randomIndex);
    }

    private void EpicCard(int amount)
    {
        int randomIndex = Random.Range(0, epicCards.Count);
        AddRandomUnitList(epicCards[randomIndex], amount);
        epicCards.RemoveAt(randomIndex);
    }


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
            totalRemainCard -= 1;
        }
        return rewardItem;
    }

}

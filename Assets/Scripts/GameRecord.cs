using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRecord : MonoBehaviour
{
    #region public º¯¼ö
    public Transform content;
    public GameObject resultPrefab;
    #endregion

    private void Start()
    {
        GetGameResult();
    }

    public void GetGameResult()
    {
        int userID = DatabaseManager.Instance.userId;
        foreach(var id in DatabaseBattleRecordModel.SelectBattleRecord(userID))
        {
            GameObject gameRecord = Instantiate(resultPrefab, content);
            gameRecord.SetActive(true);
            gameRecord.GetComponent<GameRecordDataSetting>().my_UserID = DatabaseManager.Instance.userId;
            gameRecord.GetComponent<GameRecordDataSetting>().enemy_UserID = id.userId;
            gameRecord.GetComponent<GameRecordDataSetting>().recordID = id.recordId;
        }
    }
}

public class GameResultCardInfo
{
    public int CardID { get; set; }
    public string CardGrade { get; set; }
    public int UnitLevel { get; set; }

    public GameResultCardInfo(int id, string cardGrade, int unitLevel)
    {
        CardID = id;
        CardGrade = cardGrade;
        UnitLevel = unitLevel;
    }
}

public class GameResultUserInfo
{
    public string UserName { get; set; }
    public int CrownCount { get; set; }
    public string UserCurrentBattleCard { get; set; }

    public GameResultUserInfo(string userName, int crownCount, string userCurrentBattleCard)
    {
        UserName = userName;
        CrownCount = crownCount;
        UserCurrentBattleCard = userCurrentBattleCard;
    }
}

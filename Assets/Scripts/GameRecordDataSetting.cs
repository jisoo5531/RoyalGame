using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRecordDataSetting : MonoBehaviour
{
    #region public º¯¼ö
    public Image[] allyUnitImgs;
    public Image[] enemyUnitImgs;
    public TextMeshProUGUI allyUserName;
    public TextMeshProUGUI enemyUserName;
    public TextMeshProUGUI allyCrownCount;
    public TextMeshProUGUI enemyCrownCount;

    public int my_UserID;
    public int enemy_UserID;
    public int recordID;
    #endregion

    private string allyBattleCard;
    private string enemyBattleCard;

    private void Awake()
    {
        GameResultUserInfo allyGameResultUser = DatabaseBattleRecordModel.SelectAllyResultInfo(my_UserID, recordID);
        GameResultUserInfo enemyGameResultUser = DatabaseBattleRecordModel.SelectEnemyResultInfo(my_UserID, recordID);

        if (allyGameResultUser != null)
        {
            allyUserName.text = allyGameResultUser.UserName;
            allyCrownCount.text = allyGameResultUser.CrownCount.ToString();
            allyBattleCard = allyGameResultUser.UserCurrentBattleCard;
        }

        if (enemyGameResultUser != null)
        {
            enemyUserName.text = enemyGameResultUser.UserName;
            enemyCrownCount.text = enemyGameResultUser.CrownCount.ToString();
            enemyBattleCard = enemyGameResultUser.UserCurrentBattleCard;
        }

        if (!String.IsNullOrEmpty(allyBattleCard))
        {
            List<GameResultCardInfo> units = DatabaseBattleRecordModel.SelectCurrentBattleCard(allyBattleCard, my_UserID, recordID, true);

            for (int i = 0; i < allyUnitImgs.Length; i++)
            {
                allyUnitImgs[i].sprite = CardInfoManager.instance.characterImgs[units[i].CardID - 1];
                TextMeshProUGUI textMesh = allyUnitImgs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                textMesh.text = $"lvl.{units[i].UnitLevel}";
                textMesh.color = GetColor(units[i].CardGrade);
            }
        }

        if (!String.IsNullOrEmpty(enemyBattleCard))
        {
            List<GameResultCardInfo> units = DatabaseBattleRecordModel.SelectCurrentBattleCard(enemyBattleCard, enemy_UserID, recordID, false);

            for (int i = 0; i < allyUnitImgs.Length; i++)
            {
                enemyUnitImgs[i].sprite = CardInfoManager.instance.characterImgs[units[i].CardID - 1];
                TextMeshProUGUI textMesh = enemyUnitImgs[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                textMesh.text = $"lvl.{units[i].UnitLevel}";
                textMesh.color = GetColor(units[i].CardGrade);
            }
        }
    }

    private Color GetColor(string grade)
    {
        Color color = new Color();

        switch (grade)
        {
            case "ÀÏ¹Ý":
                color = new Color(126 / 255f, 1f, 251 / 255f);
                break;
            case "Èñ±Í":
                color = new Color(1f, 100 / 255f, 0f);
                break;
            case "¿µ¿õ":
                color = new Color(153 / 255f, 0f, 1f);
                break;
        }

        return color;
    }
}

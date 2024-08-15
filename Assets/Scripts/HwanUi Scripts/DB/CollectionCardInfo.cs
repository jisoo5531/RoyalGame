using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public int cardId;
    public string name;
    public int cost;
    public int level;
    public string grade;
    public int currentCardCount;
    public int maxCardCount;
    public Sprite img;
    public string type;
}

public class CollectionCardInfo : MonoBehaviour
{
    private int index = 0;
    private bool isCost = false;
    public TMP_Text btnName;


    public void OrderByClick()
    {
        if (isCost)
        {
            btnName.text = "Èñ±Íµµ ¼ø";
            SelectCardOrderByGrade();
            StartManager.m_Instance.InitializeCollectionImage();
            isCost = false;
        }
        else
        {
            btnName.text = "¿¤¸¯¼­ ¼ø";
            SelectCardOrderbyCost();
            StartManager.m_Instance.InitializeCollectionImage();
            isCost = true;
        }
    }

    public void SelectCardOrderbyCost()
    {
        SettingCardInfoManager.instance.charData.Clear();
        index = 0;
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            string selectAllCardInfo = $"SELECT CARD.cardID, CARD.cost, CARD.name, CARD.grade, CARD.type," +
                $"CASE WHEN UNIT.currentCardCount IS NOT NULL THEN UNIT.currentCardCount " +
                $"WHEN DEFENSE_TOWER.currentCardCount IS NOT NULL THEN DEFENSE_TOWER.currentCardCount " +
                $"WHEN MAGIC.currentCardCount IS NOT NULL THEN MAGIC.currentCardCount END AS currentCardCount, " +
                $"CASE WHEN UNIT.maxCardCount IS NOT NULL THEN UNIT.maxCardCount " +
                $"WHEN DEFENSE_TOWER.maxCardCount IS NOT NULL THEN DEFENSE_TOWER.maxCardCount " +
                $"WHEN MAGIC.maxCardCount IS NOT NULL THEN MAGIC.maxCardCount END AS maxCardCount, " +
                $"CASE WHEN UNIT.level IS NOT NULL THEN UNIT.level WHEN DEFENSE_TOWER.level IS NOT NULL THEN DEFENSE_TOWER.level " +
                $"WHEN MAGIC.level IS NOT NULL THEN MAGIC.level END AS level FROM CARD " +
                $"LEFT JOIN UNIT ON CARD.cardID = UNIT.cardID AND UNIT.userID = {DatabaseManager.Instance.userId} " +
                $"LEFT JOIN MAGIC ON CARD.cardID = MAGIC.cardID AND MAGIC.userID = {DatabaseManager.Instance.userId} " +
                $"LEFT JOIN DEFENSE_TOWER ON CARD.cardID = DEFENSE_TOWER.cardID AND DEFENSE_TOWER.userID = {DatabaseManager.Instance.userId} " +
                $"ORDER BY CARD.cost ASC";

            using (MySqlCommand cmd = new MySqlCommand(selectAllCardInfo, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(5) != 0 && reader?.GetInt32(5) != null)
                        {
                            int id = reader.GetInt32(0);
                            CharacterData characterData = new CharacterData
                            {
                                cardId = id,
                                cost = reader.GetInt32(1),
                                name = reader.GetString(2),
                                grade = reader.GetString(3),
                                type = reader.GetString(4),
                                currentCardCount = reader.GetInt32(5),
                                maxCardCount = reader.GetInt32(6),
                                level = reader.GetInt32(7),
                                img = CardInfoManager.instance.characterImgs[id - 1],
                            };
                            SettingCardInfoManager.instance.charData.Add(characterData);
                            SettingCardInfoManager.instance.slots[index].GetComponent<Collection>().InitUI(characterData);
                            index++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public void SelectCardOrderByGrade()
    {
        SettingCardInfoManager.instance.charData.Clear();
        index = 0;
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            string selectAllCardInfo = $"SELECT CARD.cardID, CARD.cost, CARD.name, CARD.grade, CARD.type, " +
                $"CASE WHEN UNIT.currentCardCount IS NOT NULL THEN UNIT.currentCardCount " +
                $"WHEN DEFENSE_TOWER.currentCardCount IS NOT NULL THEN DEFENSE_TOWER.currentCardCount " +
                $"WHEN MAGIC.currentCardCount IS NOT NULL THEN MAGIC.currentCardCount END AS currentCardCount, " +
                $"CASE WHEN UNIT.maxCardCount IS NOT NULL THEN UNIT.maxCardCount " +
                $"WHEN DEFENSE_TOWER.maxCardCount IS NOT NULL THEN DEFENSE_TOWER.maxCardCount " +
                $"WHEN MAGIC.maxCardCount IS NOT NULL THEN MAGIC.maxCardCount END AS maxCardCount, " +
                $"CASE WHEN UNIT.level IS NOT NULL THEN UNIT.level WHEN DEFENSE_TOWER.level IS NOT NULL THEN DEFENSE_TOWER.level " +
                $"WHEN MAGIC.level IS NOT NULL THEN MAGIC.level END AS level FROM CARD " +
                $"LEFT JOIN UNIT ON CARD.cardID = UNIT.cardID AND UNIT.userID = {DatabaseManager.Instance.userId} " +
                $"LEFT JOIN MAGIC ON CARD.cardID = MAGIC.cardID AND MAGIC.userID = {DatabaseManager.Instance.userId} " +
                $"LEFT JOIN DEFENSE_TOWER ON CARD.cardID = DEFENSE_TOWER.cardID AND DEFENSE_TOWER.userID = {DatabaseManager.Instance.userId} " +
                $"ORDER BY CASE CARD.grade WHEN 'ÀÏ¹Ý' THEN 1 WHEN 'Èñ±Í' THEN 2 WHEN '¿µ¿õ' THEN 3 END ASC";


            using (MySqlCommand cmd = new MySqlCommand(selectAllCardInfo, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(5) != 0 && reader?.GetInt32(5) != null)
                        {
                            int id = reader.GetInt32(0);
                            CharacterData characterData = new CharacterData
                            {
                                cardId = id,
                                cost = reader.GetInt32(1),
                                name = reader.GetString(2),
                                grade = reader.GetString(3),
                                type = reader.GetString(4),
                                currentCardCount = reader.GetInt32(5),
                                maxCardCount = reader.GetInt32(6),
                                level = reader.GetInt32(7),
                                img = CardInfoManager.instance.characterImgs[id - 1],
                            };
                            SettingCardInfoManager.instance.charData.Add(characterData);
                            SettingCardInfoManager.instance.slots[index].GetComponent<Collection>().InitUI(characterData);
                            index++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }
}

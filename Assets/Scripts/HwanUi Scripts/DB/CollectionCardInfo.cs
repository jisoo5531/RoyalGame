using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class CollectionCardInfo : MonoBehaviour
{
    private int index = 0;
    private bool isCost = false;
    public TMP_Text btnName;

    void Start()
    {
        SelectCardOrderByGrade();
    }

    public void OrderByClick()
    {
        if(isCost)
        {
            btnName.text = "Èñ±Íµµ ¼ø";
            SelectCardOrderByGrade();
        }
        else
        {
            btnName.text = "¿¤¸¯¼­ ¼ø";
            SelectCardOrderbyCost();
        }
    }

    public void SelectCardOrderbyCost()
    {
        try
        {
            string selectAllCardInfo = $"SELECT CARD.cardID, CARD.cost, CARD.name, CARD.grade, " +
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
                $"LEFT JOIN DEFENSE_TOWER ON CARD.cardID = DEFENSE_TOWER.cardIDMAGIC AND DEFENSE_TOWER.userID = {DatabaseManager.Instance.userId} " +
                $"ORDER BY CARD.cost ASC";


            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(selectAllCardInfo, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int cardId = reader.GetInt32(1);
                            int cost = reader.GetInt32(2);
                            string name = reader.GetString(3);
                            string grade = reader.GetString(4);
                            int currentCardCount = reader.GetInt32(5);
                            int maxCardCount = reader.GetInt32(6);
                            int level = reader.GetInt32(7);
                            SettingCardInfoManager.instance.slots[index].GetComponent<Collection>().InitUI(cardId, currentCardCount, maxCardCount, cost, grade, name, level);
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
        try
        {
            string selectAllCardInfo = $"SELECT CARD.cardID, CARD.cost, CARD.name, CARD.grade, " +
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
            

            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(selectAllCardInfo, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetInt32(5) != 0 && reader?.GetInt32(5) != null)
                            {
                                print(reader.GetInt32(5));
                                int cardId = reader.GetInt32(1);
                                int cost = reader.GetInt32(2);
                                string name = reader.GetString(3);
                                string grade = reader.GetString(4);
                                int currentCardCount = reader.GetInt32(5);
                                int maxCardCount = reader.GetInt32(6);
                                int level = reader.GetInt32(7);
                                SettingCardInfoManager.instance.slots[index].GetComponent<Collection>().InitUI(cardId, currentCardCount, maxCardCount, cost, grade, name, level);
                                index++;
                            }
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
